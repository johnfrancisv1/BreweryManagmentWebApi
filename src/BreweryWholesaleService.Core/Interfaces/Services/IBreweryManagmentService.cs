using BreweryWholesaleService.Core.Helper;
using BreweryWholesaleService.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreweryWholesaleService.Core.Interfaces.Services
{


    public interface IBreweryManagmentService
    {


        Task<int> CreateNewBeer(_Beer newBear);


        Task<List<_Beer>> GetBeersByBrewery(string breweryID);


        Task<int> DeleteBeerByName(string BeerName);
    }
}
