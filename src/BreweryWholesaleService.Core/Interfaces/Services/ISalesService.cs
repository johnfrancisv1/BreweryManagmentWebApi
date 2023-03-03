using BreweryWholesaleService.Core.Helper;
using BreweryWholesaleService.Core.Models.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreweryWholesaleService.Core.Interfaces.Services
{
    public interface ISalesService
    {
        Task<SaleQuote> GetSaleQuote(QuoteRequest quoteRequest,  string WholeSalerID);

    }
}
