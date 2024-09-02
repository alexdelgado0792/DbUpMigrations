namespace DbMigrations.Entities
{
    internal class Script
    {
        public string Name { get; set; }
        public bool IsExecuted { get; set; }

        public Script(string scriptName, bool isExecuted)
        {
            Name = scriptName;
            IsExecuted = isExecuted;
        }
    }
}
