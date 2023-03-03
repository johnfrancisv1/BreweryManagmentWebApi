using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreweryWholesaleService.Core.Models.Stock
{
    public class RegisterBeertoWholeSalerModel
    {
       public string BeerName { get; set; }
        public string WholeSalerName { get; set; }
        public int Quntity { get; set; }
    }
}
