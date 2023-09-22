using MarketManagement.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketManagement.Data.Models
{
    public class Sale : BaseModel
    {
        private static int id = 1;
        public Sale()
        {
            Id = id;
            id++;
        }

        public decimal Amount { get; set; }
        public List<SaleItem> SaleItems { get; set; }
        public DateTime DateTime { get; set; }
    }
}
