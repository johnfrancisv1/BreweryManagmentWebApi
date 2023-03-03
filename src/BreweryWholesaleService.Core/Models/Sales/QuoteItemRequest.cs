using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreweryWholesaleService.Core.Models.Sales
{
    public class QuoteItemRequest
    {
        public string BeerName { get; set; }
        public int Quantity { get; set; }
    }


}
