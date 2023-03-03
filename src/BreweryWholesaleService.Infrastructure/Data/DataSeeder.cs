using BreweryWholesaleService.Core.EntityModels;
using BreweryWholesaleService.Core.Interfaces.Services;
using BreweryWholesaleService.Infrastructure.EntityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
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
        private readonly BreweryContext _DbContext;
        private readonly UserManager<ApplicationUser> _UserManager;
        private readonly RoleManager<IdentityRole> _RoleManager;
        private readonly ILogger _Logger;
        public DataSeeder(BreweryContext DbContext, UserManager<ApplicationUser> UserManager, RoleManager<IdentityRole> RoleManager, ILogger Logger) 
        {
            this._DbContext = DbContext;
            this._RoleManager = RoleManager;
            this._Logger = Logger; 
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
                                _Logger.LogError(err.Description);
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

                    var user = await _UserManager.FindByNameAsync(userPair.Item1.UserName);
                    if (user == null)
                    {
                        IdentityResult Resault = await _UserManager.CreateAsync(userPair.Item1, "TestPassword01");

                        if (!Resault.Succeeded)
                        {
                            foreach (var err in Resault.Errors)
                            {
                                _Logger.LogError(err.Description);
                            }
                           
                        }


                    }



                    bool isUserinRoll = await _UserManager.IsInRoleAsync(user, userPair.Item2);
                    if (!isUserinRoll)
                    {
                        IdentityResult rollResault = await _UserManager.AddToRoleAsync(user, userPair.Item2);
                        if (!rollResault.Succeeded)
                        {
                            foreach (var err in rollResault.Errors)
                            {
                                _Logger.LogError(err.Description);
                            }
                           
                        }
                    }



                }
                
                Beer b1 = new Beer()
                {
                    Name = "Leffe Blonde",
                    AlcoholContent = 6.6,
                    Price = 2.2m
                };

                Beer b2 = new Beer()
                {
                    Name = "Demon",
                    AlcoholContent = 6.6,
                    Price = 2.2m
                };

                _DbContext.Beers.Add(b1);
                _DbContext.Beers.Add(b2);

              //  await _DbContext.SaveChangesAsync();

                Stock GeneDrinksStock = new Stock()
                {
                    Bear = b1,
                    WholeSaler = GeneDrinksWholeSaler
                };
                _DbContext.Stocks.Add(GeneDrinksStock);

                await _DbContext.SaveChangesAsync();


                scope.Complete();


            }
        }
    }
}
