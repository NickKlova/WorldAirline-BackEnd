using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WADatabase.Administration.Clients
{
    public class WorldAirlinesClient : IAsyncDisposable
    {
        private ConfigurationBuilder _builder { get; set; }
        private string _configPath = "D:\\Studies\\WorldAirlines\\WorldAirline-BackEnd\\WorldAirlineServer\\";
        private IConfiguration _config { get; set; }
        private string _connectionString { get; set; }
        private DbContextOptionsBuilder<WorldAirlinesContext> _optionsBuilder { get; set; }
        private DbContextOptions<WorldAirlinesContext> _options { get; set; }
        public WorldAirlinesContext context { get; set; }

        public WorldAirlinesClient()
        {
            _builder = new ConfigurationBuilder();
            _builder.SetBasePath(_configPath);
            _builder.AddJsonFile("appsettings.json");

            _config = _builder.Build();

            _connectionString = _config.GetSection("mssql:ConnectionStrings:DefaultConnection").Value;

            _optionsBuilder = new DbContextOptionsBuilder<WorldAirlinesContext>();

            _options = _optionsBuilder.UseSqlServer(_connectionString).Options;

            context = new WorldAirlinesContext(_options);
        }

        public ValueTask DisposeAsync()
        {
            ValueTask response = context.DisposeAsync();

            context = new WorldAirlinesContext(_options);

            return response;
        }
    }
}
