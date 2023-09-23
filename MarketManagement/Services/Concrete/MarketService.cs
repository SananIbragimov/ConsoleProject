using MarketManagement.Data.Enums;
using MarketManagement.Data.Models;
using MarketManagement.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MarketManagement.Services.Concrete
{
    public class MarketService : IMarket
    {
        private List<Product> _products = new();
        private List<Sale> _sales = new();

        // This method adds products to the _products list
        public int AddProduct(string name, Category category, decimal price, int quantity)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new Exception("Fullname can't be empty!");

            if (!Enum.IsDefined(typeof(Category), category))
                throw new Exception("Category isn't exist");

            if (price <= 0)
                throw new Exception("Price can't be less than or equal to 0!");

            if (quantity <= 0)
                throw new Exception("Quantity can't be less than or equal to 0!");

            var existingProduct = _products.FirstOrDefault(x => x.Name == name && x.Category == category);
            if (existingProduct != null)
            {
                existingProduct.Price = price;
                existingProduct.Quantity += quantity;
                return existingProduct.Id;
            }
            else
            {
                var product = new Product
                {
                    Name = name,
                    Category = category,
                    Price = price,
                    Quantity = quantity
                };
                _products.Add(product);

                return product.Id;
            }


        }

        // This method updates the existing product
        public int UpdateProduct(int id, string name, Category category, decimal price, int quantity)
        {
            if (id <= 0)
                throw new Exception("Id can't be less than or equal to 0!");

            var existingProduct = _products.FirstOrDefault(x => x.Id == id);
            if (existingProduct == null)
                throw new Exception($"Product with Id: {id} not found");

            existingProduct.Name = name;
            existingProduct.Category = category;
            existingProduct.Price = price;
            existingProduct.Quantity = quantity;

            return existingProduct.Id;
        }

        // This method removes the product from the _products list
        public bool DeleteProduct(int id)
        {
            if (id <= 0)
                throw new Exception("Id can't be less than or equal to 0!");

            var deleteProduct = _products.FirstOrDefault(x => x.Id == id);
            if (deleteProduct == null)
                throw new Exception("Product not found");

            Console.WriteLine($"Are you sure you want to delete product '{deleteProduct.Name}'? (yes/no):");
            while (true)
            {
                string confirm = Console.ReadLine()!.Trim().ToLower();
                if (confirm == "yes")
                {
                    // Remove the product from _products
                    _products.Remove(deleteProduct);
                    return true;
                }
                else if (confirm == "no")
                {
                    return false;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter 'yes' or 'no':");
                    continue;
                }
            }
        }

        // This method returns the _products list
        public List<Product> GetProducts()
        {
            return _products;
        }

        // This method displays the products by their category
        public List<Product> ShowProductsByCategory(Category category)
        {
            if (!Enum.IsDefined(typeof(Category), category))
                throw new Exception("Category isn't exist");

            var productListByCategory = _products.Where(x => x.Category == category).ToList();
            return productListByCategory;
        }

        // This method displays the products by their price range
        public List<Product> ShowProductsByRangePrice(decimal min, decimal max)
        {
            if (min <= 0 && max <= 0)
                throw new Exception("Values can't be less than 0!");

            var productListByPrice = _products.Where(x => x.Price <= max && x.Price >= min).ToList();
            return productListByPrice;
        }

        // This method displays the products by their name
        public List<Product> SearchProductsByName(string text)
        {
            if (String.IsNullOrWhiteSpace(text))
                throw new Exception("Text can't be empty!");

            var productListByName = _products.Where(x => x.Name.Contains(text)).ToList();
            return productListByName;
        }

        // This method adds sales to the _sales list
        private int _saleId = 1;
        public int AddSale(List<SaleItem> saleItems, DateTime dateTime)
        {
            if (saleItems == null || !saleItems.Any())
                throw new Exception("There are no sale items");

            var sale = new Sale()
            {
                Id = _saleId,
                DateTime = dateTime,
                SaleItems = new List<SaleItem>()
            };

            foreach (var item in saleItems)
            {
                if (item == null)
                {
                    // Handle the case where a sale item is null (e.g., log the issue)
                    continue; // Skip this sale item and proceed with the next one
                }

                if (item.Quantity <= 0)
                    throw new Exception("Quantity can't be less than 0!");

                var product = _products.FirstOrDefault(x => x.Id == item.ProductId);
                if (product is null)
                    throw new Exception($"Product with ID {item.ProductId} not found.");

                if (product.Quantity < item.Quantity)
                    throw new Exception("Not enough quantity available for sale");

                item.TotalPrice = product.Price * item.Quantity;

                if (sale.SaleItems == null)
                    sale.SaleItems = new List<SaleItem>(); // Ensure SaleItems is not null

                var existingSaleItem = sale.SaleItems.FirstOrDefault(x => x.ProductId == item.ProductId);
                if (existingSaleItem != null)
                {
                    existingSaleItem.Quantity += item.Quantity;
                    existingSaleItem.TotalPrice += item.TotalPrice;
                }
                else
                {
                    sale.SaleItems.Add(new SaleItem()
                    {
                        SaleId = sale.Id,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        TotalPrice = item.TotalPrice
                    });
                }

                product.Quantity -= item.Quantity;
            }

            if (_sales == null)
                _sales = new List<Sale>(); // Ensure _sales is not null

            _sales.Add(sale);
            _saleId++;
            return sale.SaleItems.Count;
        }


        // This method refund the product from sale
        public void ReturnProductFromSale()
        {
            try
            {
                while (true)
                {

                    Console.WriteLine("Enter saleId: ");
                    if (!int.TryParse(Console.ReadLine(), out int nextSaleId))
                    {
                        Console.WriteLine("Invalid saleId. Please enter a valid number.");
                        continue;
                    }

                    Console.WriteLine("Enter productId: ");
                    if (!int.TryParse(Console.ReadLine(), out int nextProductId))
                    {
                        Console.WriteLine("Invalid productId. Please enter a valid number.");
                        continue;
                    }

                    Console.WriteLine("Enter count: ");
                    if (!int.TryParse(Console.ReadLine(), out int nextCount) || nextCount <= 0)
                    {
                        Console.WriteLine("Invalid count. Please enter a positive number.");
                        continue;
                    }


                    if (nextSaleId <= 0)
                        throw new Exception("saleId can't be less than 0!");

                    if (nextProductId <= 0)
                        throw new Exception("productId can't be less than 0!");

                    if (nextCount <= 0)
                        throw new Exception("Count can't be less than 0!");

                    var sale = _sales.FirstOrDefault(x => x.Id == nextSaleId);
                    if (sale == null)
                        throw new Exception($"Sale with Id {nextSaleId} not found!");

                    var saleItem = sale.SaleItems.FirstOrDefault(x => x.ProductId == nextProductId);
                    if (saleItem == null)
                        throw new Exception($"SaleItem with Id {nextProductId} not found!");

                    var product = _products.FirstOrDefault(x => x.Id == saleItem.ProductId);

                    if (saleItem.Quantity < nextCount)
                        throw new Exception($"Maximum {saleItem.Quantity} items can be removed from the product");

                    if (saleItem.Quantity > 0)
                    {

                        saleItem.Quantity -= nextCount;
                        product!.Quantity += nextCount;
                        saleItem.TotalPrice -= product.Price * nextCount;
                        sale.Amount -= saleItem.TotalPrice;

                        if (saleItem.Quantity == 0)
                        {
                            sale.SaleItems.Remove(saleItem);
                        }


                    }

                    Console.Write("Do you want to delete another item? (yes/no): ");
                    var response = Console.ReadLine()!.Trim().ToLower();
                    if (response == "no")
                    {
                        break;
                    }
                    else if (response != "yes")
                    {
                        Console.WriteLine("Invalid response. Please enter 'yes' or 'no'.");
                        continue;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // This method completely deletes the sale
        public bool DeleteSale(int saleId)
        {

            if (saleId <= 0)
                throw new Exception("SaleId can't be less than 0!");

            var sale = _sales.FirstOrDefault(x => x.Id == saleId);
            if (sale == null)
                throw new Exception("SaleId not found");


            Console.WriteLine($"Are you sure you want to delete sale with SaleId = '{sale.Id}'? (yes/no):");
            while (true)
            {
                var confirm = Console.ReadLine()!.Trim().ToLower();

                if (confirm == "yes")
                {
                    foreach (var item in sale.SaleItems)
                    {
                        var products = _products.FirstOrDefault(x => x.Id == item.ProductId);
                        if (products == null)
                            throw new Exception("Product not found");

                        if (item.SaleId == sale.Id)
                        {
                            products.Quantity += item.Quantity;
                        }
                    }
                    _sales.Remove(sale);
                    return true;
                }
                else if (confirm == "no")
                {
                    return false;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter 'yes' or 'no':");
                    continue;
                }
            }

        }

        // This method returns the _sales list
        public List<Sale> GetSales()
        {
            return _sales;
        }

        // This method displays the sales by date range
        public List<Sale> ShowSalesByDateRange(DateTime startDate, DateTime endDate)
        {

            if (startDate > endDate)
                throw new Exception("StartDate can't be larger than EndDate");

            var sales = _sales.Where(x => x.DateTime >= startDate && x.DateTime <= endDate).ToList();

            if (sales is null)
                throw new Exception("Sale List not found");

            return sales;

        }

        // This method displays the sales by price range
        public List<Sale> ShowSalesByPriceRange(decimal min, decimal max)
        {

            if (min > max)
                throw new Exception("Min can't be larger than max");

            if (min <= 0 || max <= 0)
                throw new Exception("Min or max cannot be less than or equal to 0");

            var sales = _sales.Where(x => x.Amount >= min && x.Amount <= max).ToList();
            if (sales is null)
                throw new Exception("Sale List not found");

            return sales;

        }

        // This method displays the sales according to a specific date
        public List<Sale> ShowSaleByDate(DateTime date)
        {

            var sales = _sales
         .Where(x =>
            x.DateTime.Year == date.Year &&
            x.DateTime.Month == date.Month &&
            x.DateTime.Day == date.Day &&
            x.DateTime.Hour == date.Hour &&
            x.DateTime.Minute == date.Minute &&
            x.DateTime.Second == date.Second)
         .ToList();

            if (sales.Count == 0)
                throw new Exception("No sales found for the specified date.");

            return sales;

        }

        // This method displays the SaleItems by SaleId
        public Sale ShowSaleItemsBySaleId(int saleId)
        {

            if (saleId <= 0)
                throw new Exception("SaleId can't be less than 0!");

            var equalSaleId = _sales.FirstOrDefault(x => x.Id == saleId);
            if (equalSaleId is null)
                throw new Exception($"Sale with SaleId = {saleId} is not available");


            return equalSaleId;

        }
    }
}
