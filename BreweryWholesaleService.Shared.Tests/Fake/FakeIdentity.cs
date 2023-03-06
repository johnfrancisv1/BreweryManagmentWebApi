using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BreweryWholesaleService.Api.Tests.Fake
{
    public class FakeIdentity
    {
        public static ClaimsPrincipal GenerateFakeIdentity(string RollName)
        {
            ClaimsIdentity ClaimsIdentity = new ClaimsIdentity(new Claim[]
                            {
                      new Claim(ClaimTypes.NameIdentifier ,Guid.NewGuid().ToString() ),
                    new Claim(ClaimTypes.Name ,"test" ),
                    new Claim(ClaimTypes.Role, RollName)


                            });

            ClaimsPrincipal CP = new ClaimsPrincipal(ClaimsIdentity);


            return CP;
        }
    }
}
