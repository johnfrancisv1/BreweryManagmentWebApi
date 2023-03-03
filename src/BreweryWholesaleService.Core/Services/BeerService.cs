using BreweryWholesaleService.Core.EntityModels;
using BreweryWholesaleService.Core.Enums;
using BreweryWholesaleService.Core.Helper;
using BreweryWholesaleService.Core.Interfaces.Repositories;
using BreweryWholesaleService.Core.Interfaces.Services;
using BreweryWholesaleService.Core.Models.Beer;
using BreweryWholesaleService.Core.Models.Identity;

using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreweryWholesaleService.Core.Services
{
    public class BeerService : IBeerService
    {
        private readonly IBeerRepository _bearRepository;
        private readonly UserManager<ApplicationUser> _UserManager;
     
        public BeerService(IBeerRepository _bearRepository, UserManager<ApplicationUser> _UserManager)
        {
            this._bearRepository = _bearRepository ?? throw new ArgumentNullException(nameof(_bearRepository));
            this._UserManager = _UserManager ?? throw new ArgumentNullException(nameof(_UserManager));
        }
        public async Task<int> CreateNewBeer(RegisterNewBeerModel NewBeerModel,string BreweryUserID)
        {
            try
            {
                if (String.IsNullOrEmpty(NewBeerModel.Name))
                {
                    throw new MyException((int)ExceptionCodes.InvaildServiceDataRequest, "Bear Name Cant be Empty");
                   // return StatusCode(StatusCodes.Status400BadRequest, );

                }
                if (NewBeerModel.Price <= 0)
                {
                    throw new MyException((int)ExceptionCodes.InvaildServiceDataRequest, "Bear Price Must be positive");
                 

                }

                _Beer Bear = new _Beer()
                {
                    Name = NewBeerModel.Name,
                    Price = NewBeerModel.Price,
                    AlcoholContent = NewBeerModel.AlcoholContent,
                    BreweryId = BreweryUserID
                };
                return  await _bearRepository.CreateNewBeer(Bear);
              
             }
            catch(Exception E)
            {
                throw new MyException( (int)ExceptionCodes.DbError, E.Message, E.InnerException);
            }
           
        }

        public async Task<int> DeleteBeerByName(string beerName)
        {
            try
            {
                if (String.IsNullOrEmpty(beerName))
                {
                     throw new MyException((int)ExceptionCodes.InvaildServiceDataRequest, "Beer Name Cant be empty");

                }
                _Beer beer = await _bearRepository.GetBeerByName(beerName);
                if (beer == null)
                {
                    throw new MyException((int)ExceptionCodes.RecordNotFound, "Beer Record Not Found");
                   
                }


                return  await _bearRepository.DeleteBeer(beer);
              
            }
            catch(Exception E)
            {
                throw new MyException((int)ExceptionCodes.DbError,E.Message,E.InnerException);
            }
             
        }

        public async Task<_Beer> GetBeerByName(string beerName)
        {
            try
            {
                if (String.IsNullOrEmpty(beerName))
                {
                    throw new MyException((int)ExceptionCodes.InvaildServiceDataRequest, "Beer Name Cant be empty");

                }

                _Beer beer = await _bearRepository.GetBeerByName(beerName);
                if (beer == null)
                {
                    throw new MyException((int)ExceptionCodes.RecordNotFound, "Beer Record Not Found");
                }

                return beer;
            }
            catch (Exception E)
            {
                throw new MyException((int)ExceptionCodes.DbError, E.Message, E.InnerException);
            }
        }

        public async Task<IEnumerable<_Beer>> GetBeersByBreweryName(string breweryName)
        {
         ApplicationUser User = await _UserManager.FindByNameAsync(breweryName);
            if(User == null)
            {
                throw new MyException((int)ExceptionCodes.RecordNotFound, "Invaild Brewery User Name");
            }
            if (String.IsNullOrEmpty(breweryName))
            {
                throw new MyException((int)ExceptionCodes.InvaildServiceDataRequest, "Brewery Name Cant be empty");

            }
            try
            {
              ApplicationUser user = await  _UserManager.FindByNameAsync(breweryName);
                return await _bearRepository.GetBeersByBreweryID(user.Name);
              
            }
            catch (Exception E)
            {
                throw new MyException((int)ExceptionCodes.DbError, E.Message, E.InnerException);
            }


        
        }
    }
}
