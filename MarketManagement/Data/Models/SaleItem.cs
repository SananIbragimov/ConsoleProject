using MarketManagement.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketManagement.Data.Models
{
    public class SaleItem : BaseModel
    {
        private static int id = 1;
        public SaleItem()
        {
            Id = id;
            id++;
        }

        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}
