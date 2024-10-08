﻿using Microsoft.Extensions.Configuration;

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
                var reportHtmlPath = configuration.GetSection("HtmlReportFileName").Value;

                if (string.IsNullOrEmpty(database) || string.IsNullOrEmpty(dbSchema) || string.IsNullOrEmpty(reportHtmlPath))
                {
                    throw new ArgumentException("Database configuration is not set properly. Please check Schema and Connection string.");
                }

                var option = MigrationHelper.ShowMenu();

                //reset console values
                Console.Clear();
                Console.ResetColor();

                var migration = new Migration(database, dbSchema, reportHtmlPath);
                switch (option)
                {
                    case MenuOption.None:
                        Console.WriteLine("This is not a valid behaviour, finishing program execution.");
                        break;
                    case MenuOption.DbStatus:
                        migration.ShowScriptsHistory();
                        break;
                    case MenuOption.HtmlReport:
                        migration.GetHtmlUpgradeReport();
                        break;
                    case MenuOption.Migrate:
                        migration.Migrate();
                        break;
                    case MenuOption.Exit:
                        Console.WriteLine("Exiting...");
                        break;
                    default:
                        Console.WriteLine("This is not a valid behaviour, finishing program execution.");
                        break;
                }

                //clean coloring
                Console.ResetColor();

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
