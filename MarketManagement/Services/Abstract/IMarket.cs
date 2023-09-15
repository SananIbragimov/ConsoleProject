using MarketManagement.Data.Enums;
using MarketManagement.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketManagement.Services.Abstract
{
    public interface IMarket
    {
        public int AddProduct(string name, Category category, decimal price, int quantity);
        public int UpdateProduct(int id);
        public int DeleteProduct(int id);
        public List<Product> GetProducts();
        public List<Product> ShowProductByCategory(Category category);
        public List<Product> ShowProductByRangePrice(decimal min, decimal max);
        public List<Product> SearchProductByName(string text);

    }
}
