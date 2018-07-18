using MyApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using SmartFormat;
using System.IO;

namespace MyApp
{
    public class DbContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        private const string userSecretsStoreName = "MyApp";
        private IConfiguration Configuration { get; set; }

        /// <summary>
        /// This is the method that 'dotnet ef migrations add [Migration_Name]' will call to get the DbContext.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public DataContext CreateDbContext(string[] args)
        {
            GetConfiguration();

            var builder = new DbContextOptionsBuilder<DataContext>();
            //builder.EnableSensitiveDataLogging();
            builder.UseSqlServer(ParseConnectionString());

            return new DataContext(builder.Options);
        }

        /// <summary>
        /// Bind the config data to the typed object
        /// and then parse the properties into the connection string template.
        /// </summary>
        /// <returns></returns>
        private string ParseConnectionString()
        {
            // create DbOptions object to store the secrets in a statically-typed way
            var dbOptions = new DbOptions();
            // bind the config props to the optionss object
            Configuration.GetSection("DbOptions").Bind(dbOptions);

            // get the connection string template and parse the options object into it
            string conTemplate = Configuration["ConnectionStrings:AzureDevConnectionTemplate"];

            return Smart.Format(conTemplate, dbOptions);
        }

        /// <summary>
        /// Build the configuration with User Secrets and the 
        /// appsettings.json file for the connection string template.
        /// </summary>
        private void GetConfiguration()
        {
            var configBuilder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: false)
               .AddUserSecrets(userSecretsStoreName);

            Configuration = configBuilder.Build();
        }
    }
}
