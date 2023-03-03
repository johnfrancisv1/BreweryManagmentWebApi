using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreweryWholesaleService.Core.Models.Sales
{
    public class QuoteRequest
    {
        public string ClientName { get; set; }
      
        public List<QuoteItemRequest>  RequestedItems { get; set; }


    }
}
