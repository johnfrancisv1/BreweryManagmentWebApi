using AutoMapper;
using BreweryWholesaleService.Core.EntityModels;
using BreweryWholesaleService.Core.Models.Stock;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreweryWholesaleService.Infrastructure.Mapper
{
    public class StockProfile : Profile
    {

        public StockProfile() 
        {
            CreateMap<Stock, _Stock>()
                .ForMember(S => S.BearId, opt => opt.MapFrom(S => S.BeerId))
                 .ForMember(S => S.Quantity, opt => opt.MapFrom(S => S.Quantity))
                  .ForMember(S => S.WholeSalerId, opt => opt.MapFrom(S => S.WholeSalerId))
                   .ForPath(S => S.Beer, opt => opt.MapFrom(S => S.Beer));

            CreateMap<_Stock, Stock>()
                .ForMember(S => S.BeerId, opt => opt.MapFrom(S => S.BearId))
                 .ForMember(S => S.Quantity, opt => opt.MapFrom(S => S.Quantity))
                  .ForMember(S => S.WholeSalerId, opt => opt.MapFrom(S => S.WholeSalerId))
                   .ForPath(S => S.Beer, opt => opt.MapFrom(S => S.Beer));

        }
       
    }
}
