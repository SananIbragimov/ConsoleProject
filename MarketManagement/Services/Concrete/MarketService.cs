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
                throw new Exception("Price can't be less than 0!");

            if (quantity <= 0)
                throw new Exception("Quantity can't be less than 0!");

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
                throw new Exception("Id can't be less than 0!");

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
                throw new Exception("Id can't be less than 0!");

            var deleteProduct = _products.FirstOrDefault(x => x.Id == id);
            if (deleteProduct == null)
                throw new Exception("Product not found");

            Console.WriteLine($"Are you sure you want to delete product '{deleteProduct.Name}'? (yes/no):");
            while (true)
            {
                string confirm = Console.ReadLine()!.Trim().ToLower();
                if (confirm == "yes")
                {
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
        public int AddSale(List<SaleItem> saleItems, DateTime dateTime)
        {

            if (saleItems == null || !saleItems.Any())
                throw new Exception("There are no sale items");


            var sale = new Sale()
            {
                DateTime = dateTime,
                SaleItems = new List<SaleItem>()
            };

            //decimal totalPrice = 0;
            foreach (var item in saleItems)
            {

                if (item.Quantity <= 0)
                    throw new Exception("Quantity can't be less than 0!");

                var product = _products.FirstOrDefault(x => x.Id == item.ProductId);
                if (product is null)
                    throw new Exception($"Product with ID {item.ProductId} not found.");

                if (product.Quantity < item.Quantity)
                    throw new Exception("Not enough quantity available for sale");

                item.TotalPrice = product.Price * item.Quantity;
                //totalPrice += item.TotalPrice;

                var saleItem = new SaleItem()
                {
                    SaleId = sale.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    TotalPrice = item.TotalPrice
                };
                var existingSaleItem = sale.SaleItems.FirstOrDefault(x => x.ProductId == item.ProductId);
                if (existingSaleItem != null)
                {
                    existingSaleItem.Quantity += item.Quantity;
                    existingSaleItem.TotalPrice += item.TotalPrice;
                }
                else
                {
                    sale.SaleItems.Add(saleItem);
                }

                product.Quantity -= saleItem.Quantity;

            }

            _sales.Add(sale);

            return sale.SaleItems.Count;
        }

        // This method refund the product from sale
        public int ReturnProductFromSale(int saleId, int productId, int count)
        {
            if (saleId <= 0)
                throw new Exception("saleId can't be less than 0!");

            if (productId <= 0)
                throw new Exception("productId can't be less than 0!");

            if (count <= 0)
                throw new Exception("Count can't be less than 0!");

            var sale = _sales.FirstOrDefault(x => x.Id == saleId);
            if (sale == null)
                throw new Exception($"Sale with Id {saleId} not found!");

            var saleItem = sale.SaleItems.FirstOrDefault(x => x.ProductId == productId);
            if (saleItem == null)
                throw new Exception($"SaleItem with Id {productId} not found!");

            var product = _products.FirstOrDefault(x => x.Id == saleItem.ProductId);

            if (saleItem.Quantity < count)
                throw new Exception($"Maximum {saleItem.Quantity} items can be removed from the product");

            if (saleItem.Quantity > 0)
            {

                saleItem.Quantity -= count;
                product!.Quantity += count;
                saleItem.TotalPrice -= product.Price * count;
                sale.Price -= saleItem.TotalPrice;

                if (saleItem.Quantity == 0)
                {
                    sale.SaleItems.Remove(saleItem);
                }

                while (true)
                {
                    Console.Write("Do you want to delete another item? (yes/no): ");
                    var response = Console.ReadLine()!.Trim().ToLower();

                    if (response == "no")
                    {
                        break;
                    }
                    else if (response != "yes")
                    {
                        Console.WriteLine("Invalid input. Please enter 'yes' or 'no'.");
                    }
                    else
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

                        ReturnProductFromSale(nextSaleId, nextProductId, nextCount);
                    }
                }
            }

            return productId;
        }

        // This method completely deletes the sale
        public int DeleteSale(int saleId)
        {
            if (saleId <= 0)
                throw new Exception("SaleId can't be less than 0!");

            var sale = _sales.FirstOrDefault(x => x.Id == saleId);
            if (sale == null)
                throw new Exception("SaleId not found");



            foreach (var item in sale.SaleItems)
            {
                var products = _products.Where(x => x.Id == item.ProductId).ToList();
                foreach (var product in products)
                {
                    if (item.SaleId == sale.Id)
                    {
                        product.Quantity += item.Quantity;
                    }
                }

            }

            _sales.Remove(sale);

            return sale.Id;
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
            if (min > max || min <= 0 || max <= 0)
                throw new Exception("Min or max range is not correct");

            var sales = _sales.Where(x => x.Price >= min && x.Price <= max).ToList();
            if (sales is null)
                throw new Exception("Sale List not found");

            return sales;

        }

        // This method displays the sales according to a specific date
        public List<Sale> ShowSaleByDate(DateTime date)
        {
            var sales = _sales.Where(x => x.DateTime == date).ToList();
            if (sales is null)
                throw new Exception("SaleList not found");

            return sales;
        }

        public List<Sale> ShowSaleItemsBySaleId(int saleId)
        {
            if (saleId <= 0)
                throw new Exception("SaleId can't be less than 0!");

            var equalSaleId = _sales.Where(x => x.Id == saleId).ToList();
            if (equalSaleId is null)
                throw new Exception($"Sale with SaleId = {saleId} is not available");

            return equalSaleId;

        }
    }
}
