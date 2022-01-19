using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;
using AWSDatabase.Administration.Managment;
using JWTAuth.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WADatabase.Administration.Clients;
using WADatabase.Administration.Managment.Interfaces;
using WADatabase.Administration.Managment;
using AWSDatabase.Administration.Interfaces;

namespace WorldAirlineServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WorldAirlineServer", Version = "v1" });
            });

            services.AddCors(options =>
            {
                options.AddPolicy("WACorsPolicy", builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            });

            string dbAWSAccessKey = Configuration.GetValue<string>("awsDynamoDb:access_key");
            string dbAWSSecretKey = Configuration.GetValue<string>("awsDynamoDb:secret_key");
            var credentials = new BasicAWSCredentials(dbAWSAccessKey, dbAWSSecretKey);
            var config = new AmazonDynamoDBConfig()
            {
                RegionEndpoint = RegionEndpoint.USEast2
            };
            var client = new AmazonDynamoDBClient(credentials, config);
            services.AddSingleton<IAmazonDynamoDB>(client);
            services.AddSingleton<IDynamoDBContext, DynamoDBContext>();

            services.AddSingleton<IAuth, AuthManagment>();
            services.AddSingleton<IAccount, AccountManagment>();
            services.AddSingleton<WorldAirlinesClient>();
            services.AddSingleton<IAirport, AirportManagment>();
            services.AddSingleton<ICrew, CrewManagment>();
            services.AddSingleton<IPassenger, PassengerManagment>();
            services.AddSingleton<IPilot, PilotManagment>();
            services.AddSingleton<IPlane, PlaneManagment>();
            services.AddSingleton<IRole, RoleManagment>();
            services.AddSingleton<ITicket, TicketManagment>();
            services.AddSingleton<IWay, WayManagment>();

            string authSecretKEY = Configuration.GetValue<string>("jwt:encryption_code");
            AuthConfiguration.KEY = authSecretKEY;

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "bearer";
                options.DefaultChallengeScheme = "bearer";
            }).AddJwtBearer("bearer", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = AuthConfiguration.GetSymmetricSecurityKey(),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WorldAirlineServer v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
