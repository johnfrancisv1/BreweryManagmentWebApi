using BreweryWholesaleService.Core.Helper;
using BreweryWholesaleService.Core.Models.Stock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreweryWholesaleService.Core.Interfaces.Services
{

    public interface IStockService
    {
        Task<int> AddBeerToWholeSaler(RegisterBeertoWholeSalerModel RegisterModel);

        Task<int> UpdateBeerQuantity(StockUpdateRequest StockUpdate_Request, string WholeSalerId);



    }
}
