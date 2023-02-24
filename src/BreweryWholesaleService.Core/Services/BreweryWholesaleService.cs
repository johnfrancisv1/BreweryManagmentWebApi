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
    public class BreweryWholesale_Service : IBreweryWholesale_Service
    {
        IBeerRepository _bearRepository;
        public BreweryWholesale_Service(IBeerRepository _bearRepository)
        {
            this._bearRepository = _bearRepository ?? throw new ArgumentNullException(nameof(_bearRepository));
        }
        public Task<Result<int>> CreateNewBeer(_Beer newBear)
        {
            return _bearRepository.CreateNewBeer(newBear);
        }

        public Task<Result<int>> DeleteBeerByName(string beerName)
        {
            throw new NotImplementedException();
        }

        public Task<Result<List<_Beer>>> GetBeersByBrewery(string breweryName)
        {
            throw new NotImplementedException();
        }
    }
}
