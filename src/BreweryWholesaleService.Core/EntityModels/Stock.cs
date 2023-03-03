using System;
using System.Collections.Generic;

namespace BreweryWholesaleService.Core.EntityModels
{
    public partial class Stock
    {
        public int Id { get; set; }
        public string WholeSalerId { get; set; } = null!;
        public int BearId { get; set; }
        public int Quantity { get; set; }

        public virtual Beer Bear { get; set; } = null!;
        public virtual ApplicationUser WholeSaler { get; set; } = null!;
    }
}
