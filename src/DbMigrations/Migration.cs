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
        private readonly string _reportPath;
        private readonly UpgradeEngine _upgrader;
        public Migration(string connectionString, string dbSchema, string reportPath)
        {
            _connectionString = connectionString;
            _dbSchema = dbSchema;
            _upgrader = CreateUpgrader();
            _reportPath = reportPath;
        }

        public void Migrate()
        {
            ValidateConnection();

            //generate report html or perform upgrade to DB
            if (_upgrader.IsUpgradeRequired())
            {
                _upgrader.PerformUpgrade();
            }

        }
        public void ShowScriptsHistory()
        {
            ValidateConnection();

            var executedScripts = _upgrader.GetExecutedScripts().AsReadOnly();
            var toExecuteScripts = _upgrader.GetScriptsToExecute().AsReadOnly();

            Console.Clear();
            //show executed scripts

            Console.WriteLine("------------------------------------");
            foreach (var script in executedScripts)
            {
                Console.WriteLine(script);
            }

            //show pending scripts
            foreach (var script in toExecuteScripts)
            {
                Console.WriteLine(script.Name);
            }
            Console.WriteLine("------------------------------------");

        }

        public void GetHtmlUpgradeReport()
        {
            ValidateConnection();

            _upgrader.GenerateUpgradeHtmlReport("path");

        }

        public void ShowOptions(int option)
        {

            //switch (option)
            //{
            //    when option == 1: Console.WriteLine(); break;
            //}
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
    }
}