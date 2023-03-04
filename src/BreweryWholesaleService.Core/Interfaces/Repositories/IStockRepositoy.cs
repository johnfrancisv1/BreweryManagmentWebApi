using BreweryWholesaleService.Core.Helper;
using BreweryWholesaleService.Core.Models.Sales;
using BreweryWholesaleService.Core.Models.Stock;
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

        Task<_Stock> GetStockRecord(int BeerID, String WholeSalerUserID);

        Task<List<_Stock>> GetQuoteRequestedStockRecords(QuoteRequest quoteRequest);

    }
}
