using AutoMapper;
using BreweryWholesaleService.Core.EntityModels;
using BreweryWholesaleService.Core.Enums;
using BreweryWholesaleService.Core.Helper;
using BreweryWholesaleService.Core.Interfaces.Repositories;
using BreweryWholesaleService.Core.Models.Beer;
using BreweryWholesaleService.Infrastructure.EntityModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreweryWholesaleService.Infrastructure.Repositories
{
    public class BeerRepository : IBeerRepository
    {

        private readonly ApplicationContext _dbContext;
        private readonly IMapper _mapper;

        public BeerRepository(ApplicationContext dbContext, IMapper mapper)
        {
            this._dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this._mapper = mapper;
        }
        public async Task<int> CreateNewBeer(_Beer newBear)
        {
            Beer beer = new Beer()
            {
                Id = newBear.Id,
                BreweryId = newBear.BreweryId,
                AlcoholContent = newBear.AlcoholContent,
                Name = newBear.Name,
                Price = newBear.Price

            };

            _dbContext.Add(beer);

              int r = await _dbContext.SaveChangesAsync();
            newBear.Id = beer.Id;
            return   beer.Id ;
        }

        public async Task<int> DeleteBeer(_Beer _beer)
        {
          

            Beer beer = _mapper.Map<Beer>(_beer);
            
            _dbContext.Attach(beer);
            _dbContext.Entry(beer).State = EntityState.Deleted;
           int r = await _dbContext.SaveChangesAsync();

            return r;

        }

        public async Task<_Beer> GetBeerByName(string beerName)
        {
            Beer beer = await _dbContext.Beers.AsNoTracking().Where(b => b.Name == beerName).SingleOrDefaultAsync();

            return _mapper.Map<_Beer>(beer);
        }

        public async Task<List<_Beer>> GetBeersByBreweryID(string breweryID)
        {
          
           List<Beer> Beers = await _dbContext.Beers.AsNoTracking().Where(b => b.BreweryId == breweryID).ToListAsync();
            return _mapper.Map<List<_Beer>>(Beers);
             


        }
    }
}
