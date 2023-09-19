﻿using MarketManagement.Data.Enums;
using MarketManagement.Data.Models;
using MarketManagement.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
            string confirm = Console.ReadLine()!.Trim().ToLower();
            if (confirm == "yes")
            {
                _products.Remove(deleteProduct);
                return true;
            }
            return false;
        }

        // This method returns the _products list
        public List<Product> GetProducts()
        {
            return _products;
        }

        public List<Product> ShowProductsByCategory(Category category)
        {
            if (!Enum.IsDefined(typeof(Category), category))
                throw new Exception("Category isn't exist");

            var productListByCategory = _products.Where(x => x.Category == category).ToList();
            return productListByCategory;
        }


        public List<Product> ShowProductsByRangePrice(decimal min, decimal max)
        {
            if (min <= 0 && max <= 0)
                throw new Exception("Values can't be less than 0!");

            var productListByPrice = _products.Where(x => x.Price <= max && x.Price >= min).ToList();
            return productListByPrice;
        }

        public List<Product> SearchProductsByName(string text)
        {
            if (String.IsNullOrWhiteSpace(text))
                throw new Exception("Text can't be empty!");

            var productListByName = _products.Where(x => x.Name.Contains(text)).ToList();
            return productListByName;
        }

        public int AddSale(List<SaleItem> saleItems, DateTime dateTime)
        {

            if (saleItems == null || !saleItems.Any())
                throw new Exception("There are no sale items");

            var sale = new Sale()
            {
                DateTime = dateTime,
                SaleItems = new List<SaleItem>()
            };

            decimal totalPrice = 0;
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
                sale.SaleItems.Add(saleItem);
                product.Quantity -= saleItem.Quantity;

            }

            _sales.Add(sale);

            return sale.SaleItems.Count;
        }

        public List<Sale> GetSales()
        {
            return _sales;
        }

        public int WithdrawalProductFromSale(int saleId, int productId)
        {
            if (saleId <= 0) 
                throw new Exception("saleId can't be less than 0!");

            if (productId <= 0)
                throw new Exception("productId can't be less than 0!");

            var sale = _sales.FirstOrDefault(x=>x.Id == saleId);
            if (sale == null)
                throw new Exception($"Sale with Id {saleId} not found!");

            var saleItem = sale.SaleItems.FirstOrDefault(x=>x.ProductId == productId);
            if (saleItem == null)
                throw new Exception($"SaleItem with Id {productId} not found!");

            var product = _products.FirstOrDefault(x=>x.Id == saleItem.ProductId);
            product!.Quantity += saleItem.Quantity;
            sale.SaleItems.Remove(saleItem);

            return productId;
        }
    }
}
