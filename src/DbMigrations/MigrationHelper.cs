namespace DbMigrations
{
    public static class MigrationHelper
    {
        public static void ShowMenu()
        {
            Console.WriteLine("1.- Check DataBase current status");
            Console.WriteLine("2.- Generate Migration HTML Report");
            Console.WriteLine("3.- Perform Migration");
            Console.WriteLine("4.- Exit");
        }
    }
}
