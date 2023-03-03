using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreweryWholesaleService.Core.Models.Beer
{
    public class RegisterNewBeerModel
    {

        public string Name { get; set; } = null!;
        public double AlcoholContent { get; set; }
        public decimal Price { get; set; }
    }
}
