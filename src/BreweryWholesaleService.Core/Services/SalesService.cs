using BreweryWholesaleService.Core.Enums;
using BreweryWholesaleService.Core.Helper;
using BreweryWholesaleService.Core.Interfaces.Repositories;
using BreweryWholesaleService.Core.Interfaces.Services;
using BreweryWholesaleService.Core.Models.Beer;
using BreweryWholesaleService.Core.Models.Identity;
using BreweryWholesaleService.Core.Models.Sales;
using BreweryWholesaleService.Core.StaticData;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreweryWholesaleService.Core.Services
{
    public class SalesService : ISalesService
    {
        private readonly IStockRepositoy _StockRepositoy;
      

        public SalesService(IStockRepositoy StockRepositoy )
        {
            this._StockRepositoy = StockRepositoy;

        }
        public async Task<SaleQuote> GetSaleQuote(QuoteRequest quoteRequest, string WholeSalerId)
        {
            //_ApplicationUser User = await _ApplicationUserManager.FindUserByID(WholeSalerId);
           
            //bool IsUserWholeSaler = await  _ApplicationUserManager.IsUserInRoll(User, RollNames.WholeSaler);
            //if (!IsUserWholeSaler)
            //{
            //    throw new MyException((int)ExceptionCodes.UnAuthorized, String.Format("The wholesaler must exist"));
            //}

            if (quoteRequest.RequestedItems == null || quoteRequest.RequestedItems.Count == 0)
            {
                throw new MyException((int)ExceptionCodes.InvaildServiceDataRequest, String.Format("The order cannot be empty"));
            }
            Result<SaleQuote> Result = new Result<SaleQuote>();
            List<_Stock> StockRecords = await _StockRepositoy.GetQuoteRequestedStockRecords(quoteRequest);

            Dictionary<string, _Stock> StockDic = new Dictionary<string, _Stock>();
            foreach(var item in StockRecords) 
            {
                if (item.WholeSalerId != WholeSalerId)
                {
                    throw new MyException((int)ExceptionCodes.UnAuthorized, String.Format("The beer must be sold by the wholesaler"));
                  
                }
                if (StockDic.ContainsKey(item.Beer.Name))
                {
                    throw new MyException((int)ExceptionCodes.InvaildServiceDataRequest, String.Format("There can't be any duplicate in the order"));
                }
                StockDic.Add(item.Beer.Name, item);

            }

            SaleQuote saleQuote = new SaleQuote();
            foreach (var item in quoteRequest.RequestedItems) 
            {
                if (!StockDic.ContainsKey(item.BeerName))
                {
                    //
                    throw new MyException((int)ExceptionCodes.InvaildServiceDataRequest, String.Format("The beer must be sold by the wholesaler"));
                }

                if (StockDic[item.BeerName].Quantity < item.Quantity)
                {
                    throw new MyException((int)ExceptionCodes.InvaildServiceDataRequest, String.Format("The number of beers ordered cannot be greater than the wholesaler's stock"));
                }

                saleQuote.AddQuoteItem(new QuoteItem() { Beer = StockDic[item.BeerName].Beer });
            }


            return saleQuote;


        }
    }
}
