using BreweryWholesaleService.Core.Helper;
using BreweryWholesaleService.Core.Models.Beer;
using BreweryWholesaleService.Core.Models.Sales;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreweryWholesaleService.Core.Interfaces.Repositories
{
    public interface IStockRepositoy
    {
         Task<int> AddNewStockRecord(_Stock StockRecord);

        Task<int> UpdateStockRecord(_Stock StockRecord);

        Task<int> RemoveStockRecord(int Id);

        Task<List<_Stock>> GetQuoteRequestedStockRecords(QuoteRequest quoteRequest);

    }
}
