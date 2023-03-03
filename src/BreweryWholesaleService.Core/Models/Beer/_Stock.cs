using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreweryWholesaleService.Core.Models.Beer
{
    public class _Stock
    {
        public int Id { get; set; }
        public string WholeSalerId { get; set; } = null!;
        public int BearId { get; set; }
        public int Quantity { get; set; }

        public _Beer Beer { get; set; } = null!;
    }
}
