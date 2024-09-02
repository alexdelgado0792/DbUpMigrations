using DbMigrations.Entities;
using DbUp;
using DbUp.Engine;
using DbUp.Helpers;
using DbUp.Support;
using System.Reflection;

namespace DbMigrations
{
    public class Migration
    {
        private readonly string _connectionString;
        private readonly string _dbSchema;
        private readonly string _reportFileName;
        private readonly UpgradeEngine _upgrader;

        public Migration(string connectionString, string dbSchema, string reportFileName)
        {
            _connectionString = connectionString;
            _dbSchema = dbSchema;
            _upgrader = CreateUpgrader();
            _reportFileName = reportFileName;
        }

        public void Migrate()
        {
            ValidateConnection();

            if (_upgrader.IsUpgradeRequired())
            {
                var result = _upgrader.PerformUpgrade();
                if (result.Successful)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Success!");
                    Console.ResetColor();
                }
                else 
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(result.Error);
                    Console.ResetColor();
                }

            }
            else
            {
                Console.WriteLine("Up to Date, No migration needed.");
            }

        }
        public void ShowScriptsHistory()
        {
            ValidateConnection();

            var myscripts = new List<Script>();
            _upgrader.GetExecutedScripts().ForEach(x => myscripts.Add(new Script(x, true)));
            _upgrader.GetScriptsToExecute().ForEach(x => myscripts.Add(new Script(x.Name, false)));

            TableHeader();
            Console.WriteLine("\n--------------------------------------------------------------------------");

            foreach (var script in myscripts.AsReadOnly())
            {
                Console.ForegroundColor = script.IsExecuted ? ConsoleColor.Green : ConsoleColor.Red;
                Console.WriteLine($"{script.Name}");
                Console.ResetColor();
            }

            Console.WriteLine("--------------------------------------------------------------------------");
        }

        public void GetHtmlUpgradeReport()
        {
            ValidateConnection();
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var currentTime = DateTime.Now;
            var fullPath = $"{path}\\{_reportFileName}_{currentTime.Month}{currentTime.Day}{currentTime.Year}_{currentTime.Ticks}.html";

            _upgrader.GenerateUpgradeHtmlReport(fullPath);

            Console.WriteLine($"Report generated at this location: {fullPath}");
        }

        private void ValidateConnection()
        {
            bool dbStabishConnection = _upgrader.TryConnect(out string msg);

            if (!dbStabishConnection)
            {
                throw new Exception(msg);
            }
        }

        private UpgradeEngine CreateUpgrader()
        {
            return DeployChanges.To
                         .SqlDatabase(_connectionString, _dbSchema)
                         .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly(), new SqlScriptOptions { ScriptType = ScriptType.RunOnce })
                         .LogToConsole()
                         .WithTransactionPerScript()
                         .Build();
        }

        private static void TableHeader()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("GREEN: Executed");
            Console.ResetColor();
            Console.Write(" | ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("RED: Pending");
            Console.ResetColor();
        }
    }
}