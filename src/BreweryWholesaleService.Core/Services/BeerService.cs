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
        private readonly IUserRepository _userRepository;
     
        public BeerService(IBeerRepository _bearRepository, IUserRepository _userRepository)
        {
            this._bearRepository = _bearRepository ?? throw new ArgumentNullException(nameof(_bearRepository));
            this._userRepository = _userRepository ?? throw new ArgumentNullException(nameof(_userRepository));
        }
        public async Task<int> CreateNewBeer(RegisterNewBeerModel NewBeerModel,string BreweryUserID)
        {
            try
            {
              

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
            string UserID = await _userRepository.GetUserIDByUserName(breweryName);
            if(String.IsNullOrEmpty(UserID))
            {
                throw new MyException((int)ExceptionCodes.RecordNotFound, "Invaild Brewery User Name");
            }
           
            try
            {
             
                return await _bearRepository.GetBeersByBreweryID(UserID);
              
            }
            catch (Exception E)
            {
                throw new MyException((int)ExceptionCodes.DbError, E.Message, E.InnerException);
            }


        
        }
    }
}
