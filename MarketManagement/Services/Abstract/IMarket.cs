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
        // Methods for products
        public int AddProduct(string name, Category category, decimal price, int quantity);
        public int UpdateProduct(int id, string name, Category category, decimal price, int quantity);
        public bool DeleteProduct(int id);
        public List<Product> GetProducts();
        public List<Product> ShowProductsByCategory(Category category);
        public List<Product> ShowProductsByRangePrice(decimal min, decimal max);
        public List<Product> SearchProductsByName(string text);

        // Methods for sales
        public int AddSale(List<SaleItem> saleItems, DateTime dateTime);
        public List<Sale> GetSales();
        public int WithdrawalProductFromSale(int saleId, int productId);

    }
}
