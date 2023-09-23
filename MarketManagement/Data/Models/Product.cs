using MarketManagement.Data.Common;
using MarketManagement.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketManagement.Data.Models
{
    public class Product : BaseModel, ICloneable
    {
        private static int id = 1;
        public Product()
        {
            Id = id;
            id++;
        }
        public string Name { get; set; }
        public Category Category { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public object Clone()
        {
            return new Product
            {
                Id = this.Id,
                Name = this.Name,
                Category = this.Category,
                Price = this.Price,
                Quantity = this.Quantity
            };
        }

    }

}
