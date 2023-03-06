using BreweryWholesaleService.Core.EntityModels;
using BreweryWholesaleService.Core.Enums;
using BreweryWholesaleService.Core.Helper;
using BreweryWholesaleService.Core.Interfaces.Repositories;
using BreweryWholesaleService.Core.Interfaces.Services;
using BreweryWholesaleService.Core.Models.Identity;
using BreweryWholesaleService.Core.Models.Sales;
using BreweryWholesaleService.Core.Models.Stock;
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

        private readonly UserManager<ApplicationUser> _UserManager;
        public SalesService(IStockRepositoy StockRepositoy, UserManager<ApplicationUser> UserManager)
        {
            this._StockRepositoy = StockRepositoy;
            this._UserManager = UserManager;

        }
        public async Task<SaleQuote> GetSaleQuote(QuoteRequest quoteRequest)
        {
          

            if (quoteRequest.RequestedItems == null || quoteRequest.RequestedItems.Count == 0)
            {
                throw new MyException((int)ExceptionCodes.InvaildServiceDataRequest, String.Format("The order cannot be empty"));
            }

            Dictionary<string, QuoteItemRequest> RequestedItemsDic = new Dictionary<string, QuoteItemRequest>();
            foreach (var item in quoteRequest.RequestedItems)
            {
               
                if (RequestedItemsDic.ContainsKey(item.BeerName))
                {
                    throw new MyException((int)ExceptionCodes.UnprocessableEntity, String.Format("There can't be any duplicate in the order"));
                }
                RequestedItemsDic.Add(item.BeerName, item);

            }


            ApplicationUser User = await _UserManager.FindByNameAsync(quoteRequest.WholeSalerName);

            bool IsUserWholeSaler = await _UserManager.IsInRoleAsync(User, RollNames.WholeSaler);
            if (!IsUserWholeSaler)
            {
                throw new MyException((int)ExceptionCodes.UnAuthorized, String.Format("The wholesaler must exist"));
            }

            Result<SaleQuote> Result = new Result<SaleQuote>();
            List<_Stock> StockRecords = await _StockRepositoy.GetQuoteRequestedStockRecords(quoteRequest);

            SaleQuote saleQuote = new SaleQuote();
            foreach (var item in StockRecords) 
            {
                if (item.WholeSalerId != User.Id)
                {
                    throw new MyException((int)ExceptionCodes.UnAuthorized, String.Format("The beer must be sold by the wholesaler"));

                }
               

                if (RequestedItemsDic[item.Beer.Name].Quantity > item.Quantity)
                {
                    throw new MyException((int)ExceptionCodes.UnprocessableEntity, String.Format("The number of beers ordered cannot be greater than the wholesaler's stock"));
                }

                saleQuote.AddQuoteItem(new QuoteItem() { Beer = item.Beer, Quantity = RequestedItemsDic[item.Beer.Name].Quantity });
            }

            saleQuote.ClientName = quoteRequest.ClientName;
            return saleQuote;


        }








    }
}
