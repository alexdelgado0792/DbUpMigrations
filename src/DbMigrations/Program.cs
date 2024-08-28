using Microsoft.Extensions.Configuration;

namespace DbMigrations
{
    internal class Program
    {
        static int Main(string[] args)
        {
            int exitCode = 0;
            try
            {
                var configuration = BuildConfig();

                var database = configuration.GetConnectionString("ConnectionString");
                var dbSchema = configuration.GetConnectionString("DbSchema");
                var reportHtmlPath = configuration.GetSection("HtmlReportPath").Value;

                if (string.IsNullOrEmpty(database) || string.IsNullOrEmpty(dbSchema) || string.IsNullOrEmpty(reportHtmlPath)) 
                {
                    throw new ArgumentException("Database configuration is not set properly. Please check Schema and Connection string.");
                }

               var migration = new Migration(database, dbSchema, reportHtmlPath);
                migration.ShowScriptsHistory();
                //migration.Migrate();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
                exitCode = 1;
            }
            

            return exitCode;
        }

        private static IConfiguration BuildConfig()
        {

            var builder = new ConfigurationBuilder();
            return builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
        }
    }
}
