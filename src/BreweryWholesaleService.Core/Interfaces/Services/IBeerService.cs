using BreweryWholesaleService.Core.Helper;
using BreweryWholesaleService.Core.Models.Beer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreweryWholesaleService.Core.Interfaces.Services
{


    public interface IBeerService
    {


        Task<int> CreateNewBeer(RegisterNewBeerModel NewBeerModel, string BreweryUserID);


        Task<IEnumerable<_Beer>> GetBeersByBreweryName(string BreweryName);

        Task<_Beer> GetBeerByName(string beerName);


        Task<int> DeleteBeerByName(string BeerName);
    }
}
