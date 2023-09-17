﻿using MarketManagement.Data.Enums;
using MarketManagement.Data.Models;
using MarketManagement.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MarketManagement.Services.Concrete
{
    public class MarketService : IMarket
    {
        private List<Product> _products = new();


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

        public List<Product> GetProducts()
        {
            return _products;
        }

        public List<Product> ShowProductByCategory(Category category)
        {
            if (!Enum.IsDefined(typeof(Category), category))
                throw new Exception("Category isn't exist");

            var productListByCategory = _products.Where(x=>x.Category == category).ToList();
            return productListByCategory;
        }

        public List<Product> SearchProductByName(string text)
        {
            throw new NotImplementedException();
        }

        public List<Product> ShowProductByRangePrice(decimal min, decimal max)
        {
            throw new NotImplementedException();
        }

    }
}
