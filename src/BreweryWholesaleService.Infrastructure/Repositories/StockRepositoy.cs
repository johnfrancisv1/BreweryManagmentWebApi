using AutoMapper;
using BreweryWholesaleService.Core.EntityModels;
using BreweryWholesaleService.Core.Enums;
using BreweryWholesaleService.Core.Helper;
using BreweryWholesaleService.Core.Interfaces.Repositories;
using BreweryWholesaleService.Core.Models.Beer;
using BreweryWholesaleService.Core.Models.Sales;
using BreweryWholesaleService.Infrastructure.EntityModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreweryWholesaleService.Infrastructure.Repositories
{
    public class StockRepositoy : IStockRepositoy
    {
        private readonly BreweryContext _dbContext;
        private readonly IMapper _mapper;

        public StockRepositoy(BreweryContext dbContext, IMapper mapper)
        {
            this._dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this._mapper = mapper;
        }
        public async Task<int> AddNewStockRecord(_Stock _StockRecord)
        {
            Stock stockRecord = _mapper.Map<Stock>(_StockRecord);
            _dbContext.Add(stockRecord);

             await _dbContext.SaveChangesAsync();
            _StockRecord.Id = stockRecord.Id;
            return stockRecord.Id;

        }

        public async Task<List<_Stock>> GetQuoteRequestedStockRecords(QuoteRequest quoteRequest)
        {
            List<string> BeerName = new List<string>(); 
            foreach(var item in quoteRequest.RequestedItems)
            {
                BeerName.Add(item.BeerName);
            }
          List<Stock> StockRecords = await  _dbContext.Stocks.Include(s => s.Bear).AsNoTracking().Where(s => BeerName.Contains(s.Bear.Name)).ToListAsync();
            return _mapper.Map<List<_Stock>>(StockRecords);
        }

        public async Task<int> RemoveStockRecord(int Id)
        {
            Stock stockRecord = await _dbContext.FindAsync<Stock>(Id);
            if(stockRecord == null)
            {
                var exp = new Exception("Record Not Found");
                exp.Data.Add("Code", ExceptionCodes.RecordNotFound);
                throw exp;
            }
            _dbContext.Entry(stockRecord).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> UpdateStockRecord(_Stock StockRecord)
        {
            Stock stockRecord = _mapper.Map<Stock>(StockRecord);
            _dbContext.Attach(stockRecord);
            _dbContext.Entry(stockRecord).Property(sr => sr.Quantity).IsModified = true;
            return await _dbContext.SaveChangesAsync();
        }
    }
}
