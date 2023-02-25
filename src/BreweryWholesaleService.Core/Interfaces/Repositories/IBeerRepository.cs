using BreweryWholesaleService.Core.Helper;
using BreweryWholesaleService.Core.Models;
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



        Task<List<_Beer>> GetBeersByBrewery(string breweryName);
       

        Task<int> DeleteBeerByName(string BeerName);
       
    }
}
