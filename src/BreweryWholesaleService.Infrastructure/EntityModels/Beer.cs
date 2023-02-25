using System;
using System.Collections.Generic;

namespace BreweryWholesaleService.Infrastructure.EntityModels
{
    public partial class Beer
    {
        public Beer()
        {
            Stocks = new HashSet<Stock>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public double AlcoholContent { get; set; }
        public decimal Price { get; set; }
        public string BreweryId { get; set; } = null!;

        public virtual ApplicationUser Brewery { get; set; } = null!;
        public virtual ICollection<Stock> Stocks { get; set; }
    }
}
