using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreweryWholesaleService.Core.Models.Beer
{
    public class RegisterNewBeerModel
    {

        [Required]
        public string Name { get; set; } = null!;
        [Required]
        [Range(0.0,100.0 , ErrorMessage = "Only positive number range from 0 to 100 allowed.")]
        public double AlcoholContent { get; set; }

        [Required]
        [Range(0.0, (double)decimal.MaxValue, ErrorMessage = "Only positive number  allowed.")]
        public decimal Price { get; set; }
    }
}
