using ConsoleTables;
using MarketManagement.Data.Enums;
using MarketManagement.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketManagement.Services.Concrete
{
    public class MenuService
    {
        private static IMarket marketService = new MarketService();
        public static void MenuAddProduct()
        {
            try
            {
                Console.WriteLine("Enter Product name:");
                string name = Console.ReadLine()!.ToLower();

                Console.WriteLine("Enter Category name:");
                Category category = Enum.Parse<Category>(Console.ReadLine()!.ToLower());

                Console.WriteLine("Enter Product Price:");
                decimal price = decimal.Parse(Console.ReadLine()!);

                Console.WriteLine("Enter Product Quantity:");
                int quantity = int.Parse(Console.ReadLine()!);

                int id = marketService.AddProduct(name, category, price, quantity);
                Console.WriteLine($"Product with ID:{id} was created!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void MenuUpdateProduct()
        {
            try
            {
                Console.WriteLine("Enter Product ID:");
                int id = int.Parse(Console.ReadLine()!);

                Console.WriteLine("Enter Product name:");
                string name = Console.ReadLine()!.ToLower();

                Console.WriteLine("Enter Category name:");
                Category category = Enum.Parse<Category>(Console.ReadLine()!.ToLower());

                Console.WriteLine("Enter Product Price:");
                decimal price = decimal.Parse(Console.ReadLine()!);

                Console.WriteLine("Enter Product Quantity:");
                int quantity = int.Parse(Console.ReadLine()!);

                int updateID = marketService.UpdateProduct(id, name, category, price, quantity);
                Console.WriteLine($"Product with ID:{updateID} was updated!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void MenuDeleteProduct()
        {
            try
            {
                Console.WriteLine("Enter the id of the product you want to delete:");
                int id = int.Parse(Console.ReadLine()!);

                var deleteID = marketService.DeleteProduct(id);
                if (deleteID)
                {
                    Console.WriteLine("Product deleted");
                }
                else
                {
                    Console.WriteLine("Product not deleted");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void MenuShowProducts()
        {
            var productList = marketService.GetProducts();
            var table = new ConsoleTable("ID", "Name", "Category", "Price", "Quantity");

            foreach (var product in productList)
            {
                table.AddRow(product.Id, product.Name, product.Category, product.Price, product.Quantity);
            }

            table.Write();
        }
    }
}
