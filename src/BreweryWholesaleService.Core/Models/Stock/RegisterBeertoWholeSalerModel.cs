using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreweryWholesaleService.Core.Models.Stock
{
    public class RegisterBeertoWholeSalerModel
    {
        [Required]
       public string BeerName { get; set; }

        [Required]
        public string WholeSalerName { get; set; }

        [Required]
        [Range(0, int.MaxValue , ErrorMessage = "Only positive number  allowed")]
        public int Quntity { get; set; }
    }
}
