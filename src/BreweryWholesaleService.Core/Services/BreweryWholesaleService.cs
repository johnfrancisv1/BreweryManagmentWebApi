using BreweryWholesaleService.Core.Helper;
using BreweryWholesaleService.Core.Interfaces.Repositories;
using BreweryWholesaleService.Core.Interfaces.Services;
using BreweryWholesaleService.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreweryWholesaleService.Core.Services
{
    public class BreweryManagmentService : IBreweryManagmentService
    {
        IBeerRepository _bearRepository;
        public BreweryManagmentService(IBeerRepository _bearRepository)
        {
            this._bearRepository = _bearRepository ?? throw new ArgumentNullException(nameof(_bearRepository));
        }
        public Task<int> CreateNewBeer(_Beer newBear)
        {
            return _bearRepository.CreateNewBeer(newBear);
        }

        public Task<int> DeleteBeerByName(string beerName)
        {
            return _bearRepository.DeleteBeerByName(beerName);
        }

        public Task<List<_Beer>> GetBeersByBrewery(string breweryName)
        {
            return _bearRepository.GetBeersByBrewery(breweryName);
        }
    }
}
