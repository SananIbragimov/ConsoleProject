using MarketManagement.Data.Enums;
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

            if (category.ToString() != Enum.GetNames(typeof(Category)).ToString())
                throw new Exception("Category isn't exist");

            if (price <= 0)
                throw new Exception("Price can't be less than 0!");

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

        public int DeleteProduct(int id)
        {
            throw new NotImplementedException();
        }

        public List<Product> GetProducts()
        {
            throw new NotImplementedException();
        }

        public List<Product> SearchProductByName(string text)
        {
            throw new NotImplementedException();
        }

        public List<Product> ShowProductByCategory(Category category)
        {
            throw new NotImplementedException();
        }

        public List<Product> ShowProductByRangePrice(decimal min, decimal max)
        {
            throw new NotImplementedException();
        }

        public int UpdateProduct(int id)
        {
            throw new NotImplementedException();
        }
    }
}
