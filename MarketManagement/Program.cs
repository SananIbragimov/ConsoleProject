using MarketManagement.Helpers;

namespace MarketManagement
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int selectedOption;

            Console.WriteLine("Welcome to Store!\n");

            do
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("1. For managing products");
                Console.WriteLine("2. For managing sales");
                Console.ForegroundColor= ConsoleColor.DarkRed;
                Console.WriteLine("0. Exit");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("----------------------------");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("Please, select an option:");

                while (!int.TryParse(Console.ReadLine(), out selectedOption))
                {
                    Console.WriteLine("Please enter valid option:");
                }

                switch (selectedOption)
                {
                    case 1:
                        SubMenuHelper.DisplayProductMenu();
                        break;
                    case 2:
                        SubMenuHelper.DisplaySaleMenu();
                        break;
                    case 0:
                        Console.WriteLine("Bye!");
                        break;
                    default:
                        Console.WriteLine("No such option!");
                        break;
                }
            } while (selectedOption != 0);
        }
    }
}