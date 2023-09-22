using ConsoleTables;
using MarketManagement.Data.Enums;
using MarketManagement.Data.Models;
using MarketManagement.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketManagement.Services.Concrete
{
    public class MenuService 
    {
        private static IMarket marketService = new MarketService();

        // MenuService static methods for products
        public static void MenuAddProduct()
        {
            try
            {
                Console.WriteLine("Enter Product name:");
                string name = Console.ReadLine()!.ToLower();

                Console.WriteLine("Enter Product Price:");
                decimal price = decimal.Parse(Console.ReadLine()!);

                Console.WriteLine("Enter Product Quantity:");
                int quantity = int.Parse(Console.ReadLine()!);

                
                Console.ResetColor();
                Console.WriteLine("Enter Category name:");
                var enumTable = new ConsoleTable("Category list:");
                foreach (var value in Enum.GetValues(typeof(Category)))
                {
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    enumTable.AddRow(value);
                }
                enumTable.Write();
                Console.ResetColor();
                Category category = Enum.Parse<Category>(Console.ReadLine()!.ToLower());

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

        public static void MenuShowProductsByCategory()
        {
            try
            {
                var enumTable = new ConsoleTable("Category list:");

                foreach (var value in Enum.GetValues(typeof(Category)))
                {
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    enumTable.AddRow(value);
                }
                enumTable.Write();
                Console.ResetColor();
                Console.WriteLine("Choose the category:");
                Category category = Enum.Parse<Category>(Console.ReadLine()!.ToLower());


                var productListByCategory = marketService.ShowProductsByCategory(category);

                var table = new ConsoleTable("ID", "Name", "Category", "Price", "Quantity");
                foreach (var product in productListByCategory)
                {
                    table.AddRow(product.Id, product.Name, product.Category, product.Price, product.Quantity);
                }

                table.Write();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void MenuShowProductsByPriceRange()
        {
            try
            {
                Console.WriteLine("Enter minimum price");
                decimal min = decimal.Parse(Console.ReadLine()!);

                Console.WriteLine("Enter maximum price");
                decimal max = decimal.Parse(Console.ReadLine()!);

                var productListByPrice = marketService.ShowProductsByRangePrice(min, max);

                var table = new ConsoleTable("ID", "Name", "Category", "Price", "Quantity");
                foreach (var product in productListByPrice)
                {
                    table.AddRow(product.Id, product.Name, product.Category, product.Price, product.Quantity);
                }

                table.Write();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        public static void MenuSearchProductsByName()
        {
            try
            {
                Console.WriteLine("Enter the search word");
                string text = Console.ReadLine()!;

                var productListByName = marketService.SearchProductsByName(text);

                var table = new ConsoleTable("ID", "Name", "Category", "Price", "Quantity");
                foreach (var product in productListByName)
                {
                    table.AddRow(product.Id, product.Name, product.Category, product.Price, product.Quantity);
                }

                table.Write();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // MenuService static methods for sales
        public static void MenuAddSale()
        {
            try
            {

                Console.WriteLine("Enter count of sale items");
                int itemCount = int.Parse(Console.ReadLine()!);

                var saleItems = new List<SaleItem>();
                for (int i = 1; i <= itemCount; i++)
                {
                    Console.WriteLine($"Enter product Id for SaleItem {i}");
                    int id = int.Parse(Console.ReadLine()!);

                    Console.WriteLine($"Enter Product quantity for ProductId {id}");
                    int quantity = int.Parse(Console.ReadLine()!);

                    saleItems.Add(new SaleItem()
                    {
                        ProductId = id,
                        Quantity = quantity
                    });
                }

                Console.WriteLine("Enter datetime:");

                DateTime dateTime;
                string userInput = Console.ReadLine()!;

                if (!string.IsNullOrEmpty(userInput))
                {
                    dateTime = DateTime.ParseExact(userInput, "dd/MM/yyyy HH:mm:ss", null);
                }
                else
                {
                    dateTime = DateTime.Now;
                }

                Console.WriteLine("Selected Date and Time: " + dateTime);

                var saleItemsCount = marketService.AddSale(saleItems, dateTime);
                Console.WriteLine($"SaleItems count: {saleItemsCount}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void MenuGetSales()
        {
            var saleList = marketService.GetSales();
            var tableSale = new ConsoleTable("Sale Id", "Amount", "SaleItems Count", "DateTime");
            foreach (var sale in saleList)
            {
                sale.Amount = 0;
                foreach (var item in sale.SaleItems)
                {
                    sale.Amount += item.TotalPrice;
                }
                tableSale.AddRow(sale.Id, sale.Amount, sale.SaleItems.Count, sale.DateTime);
            }
            tableSale.Write();

        }

        public static void MenuReturnProductFromSale()
        {
            try
            {
                marketService.ReturnProductFromSale();
                Console.WriteLine($"Return the SaleItem from Sale");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void MenuDeleteSale()
        {
            try
            {
                Console.WriteLine("Enter the saleId of the sale you want to delete:");
                int saleId = int.Parse(Console.ReadLine()!);

                var deleteSaleId = marketService.DeleteSale(saleId);
                if (deleteSaleId)
                {
                    Console.WriteLine("Sale deleted");
                }
                else
                {
                    Console.WriteLine("Sale not deleted");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void MenuShowSalesByDateRange()
        {
            Console.WriteLine("Enter start date (dd/MM/yyyy HH:mm:ss):");
            DateTime startDate;
            string startInput = Console.ReadLine()!;

            Console.WriteLine("Enter end date (dd/MM/yyyy HH:mm:ss):");
            DateTime endDate;
            string endInput = Console.ReadLine()!;

            if (!string.IsNullOrEmpty(startInput))
            {
                startDate = DateTime.ParseExact(startInput, "dd/MM/yyyy HH:mm:ss", null);
            }
            else
            {
                startDate = DateTime.Now;
            }

            if (!string.IsNullOrEmpty(endInput))
            {
                endDate = DateTime.ParseExact(endInput, "dd/MM/yyyy HH:mm:ss", null);
            }
            else
            {
                endDate = DateTime.Now;
            }

            Console.WriteLine("Selected Start Date: " + startDate);
            Console.WriteLine("Selected End Date: " + endDate);

            var salesByDateRange = marketService.ShowSalesByDateRange(startDate, endDate);

            var table = new ConsoleTable("Sale Id", "Amount", "SaleItems Count", "DateTime");
            foreach (var sale in salesByDateRange)
            {
                table.AddRow(sale.Id, sale.Amount, sale.SaleItems.Count, sale.DateTime);
            }

            table.Write();
        }

        public static void MenuShowSalesByPriceRange()
        {
            Console.WriteLine("Enter minimum price");
            decimal min = decimal.Parse(Console.ReadLine()!);

            Console.WriteLine("Enter minimum price");
            decimal max = decimal.Parse(Console.ReadLine()!);

            var saleByPriceRange = marketService.ShowSalesByPriceRange(min, max);

            var table = new ConsoleTable("Sale Id", "Amount", "SaleItems Count", "DateTime");
            foreach (var sale in saleByPriceRange)
            {
                table.AddRow(sale.Id, sale.Amount, sale.SaleItems.Count, sale.DateTime);
            }

            table.Write();
        }

        public static void MenuShowSaleByDate()
        {
            Console.WriteLine("Enter datetime:");

            DateTime date;
            string userInput = Console.ReadLine()!;

            if (!string.IsNullOrEmpty(userInput))
            {
                date = DateTime.ParseExact(userInput, "dd/MM/yyyy HH:mm:ss", null);
            }
            else
            {
                date = DateTime.Now;
            }

            Console.WriteLine("Selected Date and Time: " + date);

            var saleByDate = marketService.ShowSaleByDate(date);

            var table = new ConsoleTable("Sale Id", "Amount", "SaleItems Count", "DateTime");
            foreach (var sale in saleByDate)
            {
                table.AddRow(sale.Id, sale.Amount, sale.SaleItems.Count, sale.DateTime);
            }

            table.Write();
        }

        public static void MenuShowSaleItemsBySaleId()
        {
            try
            {
                Console.WriteLine("Enter the sale id of the saleitem you want to show");
                int saleId = int.Parse(Console.ReadLine()!);

                var saleBySaleId = marketService.ShowSaleItemsBySaleId(saleId);
                var productList = marketService.GetProducts();

                Console.WriteLine("SaleItems by saleId");

                var tableSaleItem = new ConsoleTable("Sale Id", "Product Name", "Product Price", "Quantity", "Total Price");

                foreach (var item in saleBySaleId.SaleItems)
                {
                    var product = productList.FirstOrDefault(x => x.Id == item.ProductId);
                    if (product == null)
                        throw new Exception("Product not found");

                    tableSaleItem.AddRow(item.SaleId, product.Name, product.Price, item.Quantity, item.TotalPrice);
                }

                tableSaleItem.Write();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
