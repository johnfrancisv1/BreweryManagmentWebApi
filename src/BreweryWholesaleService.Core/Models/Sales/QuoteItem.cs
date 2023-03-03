using BreweryWholesaleService.Core.Models.Beer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreweryWholesaleService.Core.Models.Sales
{
    public class QuoteItem
    {
        public _Beer Beer { get; set; }
        public int Quantity { get; set; }

        public decimal TotalPrice {
            get 
            {
                return Beer.Price * Quantity;
            }
        }
    }
}
