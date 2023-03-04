using BreweryWholesaleService.Core.EntityModels;
using BreweryWholesaleService.Core.Enums;
using BreweryWholesaleService.Core.Helper;
using BreweryWholesaleService.Core.Interfaces.Repositories;
using BreweryWholesaleService.Core.Interfaces.Services;
using BreweryWholesaleService.Core.Models.Beer;
using BreweryWholesaleService.Core.Models.Identity;
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
    public class StockService : IStockService
    {
       private readonly IStockRepositoy _StockRepository;
        private readonly IBeerRepository _BeerRepository;
        private readonly UserManager<ApplicationUser> _UserManager;
    //    private readonly UserManager

        public StockService(IStockRepositoy StockRepository , IBeerRepository BeerRepository, UserManager<ApplicationUser> _UserManager)
        {
            this._BeerRepository = BeerRepository;
            this._StockRepository = StockRepository;
            this._UserManager = _UserManager;
        }
        public async Task<int> AddBeerToWholeSaler(RegisterBeertoWholeSalerModel RegisterModel)
        {
            _Beer beer = await _BeerRepository.GetBeerByName(RegisterModel.BeerName);
            ApplicationUser user = await _UserManager.FindByNameAsync(RegisterModel.WholeSalerName);
           bool IsUserInWholeSalerRoll = await _UserManager.IsInRoleAsync(user, RollNames.WholeSaler);
            if (!IsUserInWholeSalerRoll)
            {
                throw new MyException((int)ExceptionCodes.UnprocessableEntity, "User must be in WholeSaler Roll");
              
            }

            _Stock StockRecord = new _Stock()
            {
                BearId = beer.Id,
                WholeSalerId = user.Id,
                Quantity = RegisterModel.Quntity
            };

          return  await  _StockRepository.AddNewStockRecord(StockRecord);

          

        }

        public async Task<int> UpdateBeerQuantity(StockUpdateRequest StockUpdate_Request,string WholeSalerId)
        {
            _Beer beer = await _BeerRepository.GetBeerByName(StockUpdate_Request.BeerName);
            if(beer == null)
            {
                throw new MyException((int)ExceptionCodes.UnprocessableEntity, "Beer Record Not Found ");
            }
            _Stock StockRecord = await _StockRepository.GetStockRecord(beer.Id, WholeSalerId);
            if(StockRecord == null)
            {
                throw new MyException((int)ExceptionCodes.UnprocessableEntity, "Beer must be listed on the whole saler list ");
            }

            StockRecord.Quantity = StockUpdate_Request.Quntity;


            return  await _StockRepository.UpdateStockRecord(StockRecord);

           
        }
    }
}
