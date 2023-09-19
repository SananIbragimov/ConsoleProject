using MarketManagement.Services.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketManagement.Helpers
{
    public static class SubMenuHelper
    {
        public static void DisplayProductMenu()
        {
            int selectedOption;

            do
            {
                Console.WriteLine("1. Add Product");
                Console.WriteLine("2. Update Product");
                Console.WriteLine("3. Delete Product");
                Console.WriteLine("4. Show Products");
                Console.WriteLine("5. Show Products By Category");
                Console.WriteLine("6. Show Products By Price Range");
                Console.WriteLine("7. Search Products By Name");

                Console.WriteLine("0. Exit");
                Console.WriteLine("----------------------------");
                Console.WriteLine("Please, select an option:");

                while (!int.TryParse(Console.ReadLine(), out selectedOption))
                {
                    Console.WriteLine("Please enter valid option:");
                }

                switch (selectedOption)
                {
                    case 1:
                        MenuService.MenuAddProduct();
                        break;
                    case 2:
                        MenuService.MenuUpdateProduct();
                        break;
                    case 3:
                        MenuService.MenuDeleteProduct();
                        break;
                    case 4:
                        MenuService.MenuShowProducts();
                        break;
                    case 5:
                        MenuService.MenuShowProductsByCategory();
                        break;
                    case 6:
                        MenuService.MenuShowProductsByPriceRange();
                        break;
                    case 7:
                        MenuService.MenuSearchProductsByName();
                        break;
                    case 0:
                        break;
                    default:
                        Console.WriteLine("No such option!");
                        break;
                }
            } while (selectedOption != 0);
        }

        public static void DisplaySaleMenu()
        {
            int selectedOption;

            do
            {
                Console.WriteLine("1. Add Sale");
                Console.WriteLine("2. Withdrawal Product From Sale");
                Console.WriteLine("3. Delete Sale");
                Console.WriteLine("4. Show Sales");
                Console.WriteLine("5. Show Sales By Date Range");
                Console.WriteLine("6. Show Sales By Price Range");
                Console.WriteLine("7. Show Sales By Specific Date");
                Console.WriteLine("8. Show Sale By Specific ID");

                Console.WriteLine("0. Exit");
                Console.WriteLine("----------------------------");
                Console.WriteLine("Please, select an option:");

                while (!int.TryParse(Console.ReadLine(), out selectedOption))
                {
                    Console.WriteLine("Please enter valid option:");
                }

                switch (selectedOption)
                {
                    case 1:
                        MenuService.MenuAddSale();
                        break;
                    case 2:
                        MenuService.MenuWithdrawalProductFromSale();
                        break;
                    case 3:

                        break;
                    case 4:
                        MenuService.MenuGetSales();
                        break;
                    case 5:

                        break;
                    case 6:

                        break;
                    case 7:

                        break;
                    case 8:

                        break;
                    case 0:
                        break;
                    default:
                        Console.WriteLine("No such option!");
                        break;
                }
            } while (selectedOption != 0);
        }
    }
}
