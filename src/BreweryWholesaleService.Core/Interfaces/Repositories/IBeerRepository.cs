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
        Task<Result<int>> CreateNewBeer(_Beer newBear);



        Task<Result<List<_Beer>>> GetBeersByBrewery(string breweryName);
       

        Task<Result<int>> DeleteBeerByName(string BeerName);
       
    }
}
