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

        public static void MenuShowProductsByCategory()
        {
            try
            {
                var enumTable = new ConsoleTable("Category list:");

                foreach (var value in Enum.GetValues(typeof(Category)))
                {
                    enumTable.AddRow(value);
                }
                enumTable.Write();

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

                Console.WriteLine("Enter datetime");
                var dateTime = DateTime.ParseExact(Console.ReadLine()!, "dd.MM.yyyy HH:mm:ss", null);

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
            var tableSale = new ConsoleTable("Sale Id", "Price", "SaleItems Count", "DateTime");
            foreach (var sale in saleList)
            {
                sale.Price = 0;
                foreach (var item in sale.SaleItems)
                {
                    sale.Price += item.TotalPrice;
                }
                tableSale.AddRow(sale.Id, sale.Price, sale.SaleItems.Count, sale.DateTime);
            }
            tableSale.Write();

        }

        public static void MenuReturnProductFromSale()
        {
            try
            {
                Console.WriteLine("Enter SaleId: ");
                int saleId = int.Parse(Console.ReadLine()!);

                Console.WriteLine("Enter ProductId: ");
                int productId = int.Parse(Console.ReadLine()!);

                Console.WriteLine("Enter Quantity: ");
                int count = int.Parse(Console.ReadLine()!);

                var withdrawProduct = marketService.ReturnProductFromSale(saleId, productId, count);
                Console.WriteLine($"Withdraw the Product with ProductId={withdrawProduct}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void MenuDeleteSale()
        {
            Console.WriteLine("Enter SaleId:");
            int saleId = int.Parse(Console.ReadLine()!);

            var deleteSale = marketService.DeleteSale(saleId);
            Console.WriteLine($"Delete the sale with SaleId={deleteSale} from sale list");
        }

        public static void MenuShowSalesByDateRange()
        {
            Console.WriteLine("Enter Start date:");
            DateTime startDate = DateTime.ParseExact(Console.ReadLine()!, "dd.MM.yyyy HH:mm:ss", null);

            Console.WriteLine("Enter End date:");
            DateTime endDate = DateTime.ParseExact(Console.ReadLine()!, "dd.MM.yyyy HH:mm:ss", null);

            var salesByDateRange = marketService.ShowSalesByDateRange(startDate, endDate);

            var table = new ConsoleTable("Sale Id", "Price", "SaleItems Count", "DateTime");
            foreach (var sale in salesByDateRange)
            {
                table.AddRow(sale.Id, sale.Price, sale.SaleItems.Count, sale.DateTime);
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

            var table = new ConsoleTable("Sale Id", "Price", "SaleItems Count", "DateTime");
            foreach (var sale in saleByPriceRange)
            {
                table.AddRow(sale.Id, sale.Price, sale.SaleItems.Count, sale.DateTime);
            }

            table.Write();
        }

        public static void MenuShowSaleByDate()
        {
            Console.WriteLine("Enter datetime:");
            var date = DateTime.ParseExact(Console.ReadLine()!, "dd.MM.yyyy HH:mm:ss", null);

            var saleByDate = marketService.ShowSaleByDate(date);

            var table = new ConsoleTable("Sale Id", "Price", "SaleItems Count", "DateTime");
            foreach (var sale in saleByDate)
            {
                table.AddRow(sale.Id, sale.Price, sale.SaleItems.Count, sale.DateTime);
            }

            table.Write();
        }

        public static void MenuShowSaleItemsBySaleId()
        {
            Console.WriteLine("Enter the sale id of the saleitem you want to show");
            int saleId = int.Parse(Console.ReadLine()!);

            var saleBySaleId = marketService.ShowSaleItemsBySaleId(saleId);
            var productList = marketService.GetProducts();

            Console.WriteLine("SaleItems by saleId");

            var tableSaleItem = new ConsoleTable("Sale Id", "Product Name", "Product Price", "Quantity", "Total Price");
            foreach (var sale in saleBySaleId)
            {
                foreach (var item in sale.SaleItems)
                {
                    var product = productList.FirstOrDefault(x => x.Id == item.ProductId);
                    if (product == null)
                        throw new Exception("Product not found");

                    tableSaleItem.AddRow(item.SaleId, product.Name, product.Price, item.Quantity, item.TotalPrice);
                }

            }
            tableSaleItem.Write();
        }
    }
}
