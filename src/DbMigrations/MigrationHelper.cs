namespace DbMigrations
{
    public static class MigrationHelper
    {
        public static MenuOption ShowMenu()
        {
            Console.WriteLine("Main Menu - Please select an option:");
            Console.WriteLine("1.- Check DataBase current status");
            Console.WriteLine("2.- Generate Migration HTML Report");
            Console.WriteLine("3.- Perform Migration");
            Console.WriteLine("4.- Exit");

            Console.Write("Selection: ");
            var option = Console.ReadLine();

            var selection = ValidateSelection(option);
            if (selection == MenuOption.None)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("IT WAS NOT A VALID OPTION");
                Console.ResetColor();
                ShowMenu();
            }

            return selection;
        }

        private static MenuOption ValidateSelection(string? option)
        {
            if (string.IsNullOrEmpty(option))
            {
                return MenuOption.None;
            }

            bool parseToInt = int.TryParse(option, out int optionNumber);
            if (!parseToInt || optionNumber <= 0)
            {
                return MenuOption.None;
            }

            var enumlenght = Enum.GetValues(typeof(MenuOption)).Length;
            if (optionNumber >= enumlenght) 
            {
                return MenuOption.None; 
            }

            bool parseToEnum = Enum.TryParse(option, out MenuOption result);
            if (!parseToEnum)
            {
                return MenuOption.None;
            }

            return result;
        }
    }
}
