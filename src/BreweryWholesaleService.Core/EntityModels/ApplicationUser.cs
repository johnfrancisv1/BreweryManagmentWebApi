using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreweryWholesaleService.Core.EntityModels
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(100)]


        public string Name { get; set; }


        public ApplicationUser()
        {
            Beers = new HashSet<Beer>();
            Stocks = new HashSet<Stock>();
        }

        public virtual ICollection<Beer> Beers { get; set; }
        public virtual ICollection<Stock> Stocks { get; set; }
    }
}
