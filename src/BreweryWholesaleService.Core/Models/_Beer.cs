using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreweryWholesaleService.Core.Models
{
    public class _Beer
    {
        public string Name { get; set; } = null!;
        public double AlcoholContent { get; set; }
        public decimal Price { get; set; }
    }
}
