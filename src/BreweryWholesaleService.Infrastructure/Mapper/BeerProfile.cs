using AutoMapper;
using BreweryWholesaleService.Core.EntityModels;
using BreweryWholesaleService.Core.Models.Beer;
using BreweryWholesaleService.Infrastructure.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreweryWholesaleService.Infrastructure.Mapper
{
    public class BeerProfile : Profile
    {
       
            public BeerProfile()
            {
                // Default mapping when property names are same
                CreateMap<Beer, _Beer>();

            }
        
    }
}
