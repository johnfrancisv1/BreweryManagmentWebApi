using AutoMapper;
using BreweryWholesaleService.Core.Enums;
using BreweryWholesaleService.Core.Helper;
using BreweryWholesaleService.Core.Interfaces.Repositories;
using BreweryWholesaleService.Core.Models;
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

        private readonly BreweryContext _dbContext;
        private readonly IMapper _mapper;

        public BeerRepository(BreweryContext dbContext, IMapper mapper)
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

            _dbContext.Add(newBear);

              int r = await _dbContext.SaveChangesAsync();
            newBear.Id = beer.Id;
            return   beer.Id ;
        }

        public async Task<int> DeleteBeerByName(string BeerName)
        {
           Beer b = await _dbContext.Beers.Where(b => b.Name == BeerName).FirstOrDefaultAsync();
            if (b == null)
            {
                 
                var exp = new Exception("Record Not Found");
                exp.Data.Add("Code", ResultCodes.RecordNotFound);
                throw exp;
            }

            _dbContext.Entry(b).State = EntityState.Deleted;
           int r = await _dbContext.SaveChangesAsync();

            return r;

        }

        public async Task<List<_Beer>> GetBeersByBrewery(string breweryName)
        {
            string breweryID = await _dbContext.Users.Where(u => u.Name == breweryName).Select(u => u.Id).FirstOrDefaultAsync();
            if (string.IsNullOrEmpty(breweryID))
            {
                var exp = new Exception("Invaild User ID");
                exp.Data.Add("Code", ResultCodes.InvaildUserID);
                throw exp;
            }
           List<Beer> Beers = await _dbContext.Beers.Where(b => b.BreweryId == breweryID).ToListAsync();
            return _mapper.Map<List<_Beer>>(Beers);
             


        }
    }
}
