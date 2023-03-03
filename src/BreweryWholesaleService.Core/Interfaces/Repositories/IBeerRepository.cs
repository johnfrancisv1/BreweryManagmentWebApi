using BreweryWholesaleService.Core.Helper;
using BreweryWholesaleService.Core.Models.Beer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreweryWholesaleService.Core.Interfaces.Repositories
{
    public interface IBeerRepository
    {
        Task<int> CreateNewBeer(_Beer newBear);

        Task<_Beer> GetBeerByName(string beerName);

        Task<List<_Beer>> GetBeersByBreweryID(string breweryID);

      


        Task<int> DeleteBeer(_Beer beer);


       
    }
}
