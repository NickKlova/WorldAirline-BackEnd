using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WADatabase.Administration.Clients
{
    public class WorldAirlinesClient
    {
        private ConfigurationBuilder _builder { get; set; }
        private string _configPath = "C:\\Users\\KlovaNick\\Desktop\\WorldAirlines\\BackEnd\\WorldAirlineServer\\WADatabase\\Settings\\";
        private IConfiguration _config { get; set; }
        private string _connectionString { get; set; }
        private DbContextOptionsBuilder<WorldAirlinesContext> _optionsBuilder { get; set; }
        private DbContextOptions<WorldAirlinesContext> _options { get; set; }
        public WorldAirlinesContext context { get; set; }

        public WorldAirlinesClient()
        {
            _builder = new ConfigurationBuilder();
            _builder.SetBasePath(_configPath);
            _builder.AddJsonFile("db-config.json");

            _config = _builder.Build();

            _connectionString = _config.GetConnectionString("DefaultConnection");

            _optionsBuilder = new DbContextOptionsBuilder<WorldAirlinesContext>();

            _options = _optionsBuilder.UseSqlServer(_connectionString).Options;

            context = new WorldAirlinesContext(_options);
        }
    }
}
