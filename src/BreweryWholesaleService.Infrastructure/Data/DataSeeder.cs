using BreweryWholesaleService.Core.EntityModels;
using BreweryWholesaleService.Core.Helper;
using BreweryWholesaleService.Core.Interfaces.Services;
using BreweryWholesaleService.Infrastructure.EntityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace BreweryWholesaleService.Infrastructure.Data
{
    public class DataSeeder : IDataSeeder
    {
        private readonly ApplicationContext _DbContext;
        private readonly UserManager<ApplicationUser> _UserManager;
        private readonly RoleManager<IdentityRole> _RoleManager;
       
        public DataSeeder(ApplicationContext DbContext, UserManager<ApplicationUser> UserManager, RoleManager<IdentityRole> RoleManager) 
        {
            this._DbContext = DbContext;
            this._RoleManager = RoleManager;
            this._UserManager = UserManager;
        }

        public async Task SeedData() 
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                foreach (var rollName in new string[] { "Brewery", "WholeSaler" })
                {
                    var IsRollExistes = await _RoleManager.RoleExistsAsync(rollName);
                    if (!IsRollExistes)
                    {
                        IdentityResult Resault = await _RoleManager.CreateAsync(new IdentityRole() { Name = rollName });
                        if (!Resault.Succeeded)
                        {
                            foreach (var err in Resault.Errors)
                            {
                                throw new Exception(string.Join(",", Resault.Errors));
                            }
                          
                        }
                    }
                }

                ApplicationUser appUser_Abbaye = new ApplicationUser()
                {
                    Name = "Abbaye de Leffe",
                    UserName = "Abbaye"
                };



                ApplicationUser appUser_Heineken = new ApplicationUser()
                {
                    Name = "Heineken",
                    UserName = "Heineken"
                };

                ApplicationUser GeneDrinksWholeSaler = new ApplicationUser()
                {
                    Name = "GeneDrinks",
                    UserName = "GeneDrinks"
                };

                ApplicationUser WholeSaler2 = new ApplicationUser()
                {
                    Name = "WholeSaler2",
                    UserName = "WholeSaler2"
                };


                List<Tuple<ApplicationUser, string>> Users = new List<Tuple<ApplicationUser, string>>()
                    {
                        new Tuple<ApplicationUser, string>(appUser_Abbaye ,"Brewery" ),
                        new Tuple<ApplicationUser, string>(appUser_Heineken ,"Brewery" ),
                        new Tuple<ApplicationUser, string>(GeneDrinksWholeSaler ,"WholeSaler" ),
                        new Tuple<ApplicationUser, string>(WholeSaler2 ,"WholeSaler" ),
                };


                foreach (var userPair in Users)
                {
                   
              
                        IdentityResult Resault = await _UserManager.CreateAsync(userPair.Item1, "TestPassword01");

                        if (!Resault.Succeeded)
                        {
                            throw new Exception(string.Join(",", Resault.Errors));
                           
                        }


                    

                    var user = await _UserManager.FindByNameAsync(userPair.Item1.UserName);

                  
                        IdentityResult rollResault = await _UserManager.AddToRoleAsync(user, userPair.Item2);
                        if (!rollResault.Succeeded)
                        {
                            foreach (var err in rollResault.Errors)
                            {
                                throw new Exception(string.Join(",", rollResault.Errors));
                            }
                           
                        }
                    



                }
                
                Beer Beer_LeffeBlonde = new Beer()
                {
                    Name = "Leffe Blonde",
                    AlcoholContent = 6.6,
                    Price = 2.2m,
                     BreweryId = GeneDrinksWholeSaler.Id
                };

                Beer Beer_Demon = new Beer()
                {
                    Name = "Demon",
                    AlcoholContent = 6.6,
                    Price = 2.2m,
                    BreweryId = GeneDrinksWholeSaler.Id
                };

                //_DbContext.Beers.Add(Beer_LeffeBlonde);
                //_DbContext.Beers.Add(Beer_Demon);

              //  await _DbContext.SaveChangesAsync();

                Stock GeneDrinksLeffeBlondeStock = new Stock()
                {
                    Beer = Beer_LeffeBlonde,
                    WholeSaler = GeneDrinksWholeSaler,
                    Quantity = 5
                };
                Stock GeneDrinksDemonStock = new Stock()
                {
                    Beer = Beer_Demon,
                    WholeSaler = GeneDrinksWholeSaler,
                    Quantity = 7
                };
                _DbContext.Stocks.Add(GeneDrinksLeffeBlondeStock);
                _DbContext.Stocks.Add(GeneDrinksDemonStock);

                await _DbContext.SaveChangesAsync();


                scope.Complete();


            }
        }
    }
}
