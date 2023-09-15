using MarketManagement.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketManagement.Data.Models
{
    public class Sell : BaseModel
    {
        private static int id = 1;
        public Sell()
        {
            Id = id;
            id++;
        }

        public decimal Price { get; set; }
        public List<SellItem> SellItems { get; set; }
        public DateTime DateTime { get; set; }
    }
}
