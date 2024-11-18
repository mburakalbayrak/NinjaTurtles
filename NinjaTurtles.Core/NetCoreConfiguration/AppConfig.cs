
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaTurtles.Core.NetCoreConfiguration
{
    public class AppConfig
    {
        public static IConfiguration Configuration 
        { 
            get 
            {
                var configurationBuilder = new ConfigurationBuilder();
                var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
                configurationBuilder.AddJsonFile(path, false);
                configurationBuilder.AddEnvironmentVariables();
                var root = configurationBuilder.Build();
                return root;
            } 
        }
    }
}
