
using BreweryWholesaleService.Core.EntityModels;
using BreweryWholesaleService.Core.Models.Identity;
using BreweryWholesaleService.Infrastructure.EntityModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Transactions;

namespace BreweryWholesaleService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserManagerController : ControllerBase
    {


        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationContext _dbContext;
        private readonly RoleManager<IdentityRole> _rollManager;
        private readonly ILogger _logger;
        public UserManagerController(IConfiguration configuration, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationContext _dbContex, RoleManager<IdentityRole> rollManager, ILogger<UserManagerController> _logger)
        {
            this._configuration = configuration;
            this._signInManager = signInManager;
            this._userManager = userManager;
            this._rollManager = rollManager;
            this._dbContext = _dbContex;
            this._logger = _logger;
        }

        [AllowAnonymous]
        [HttpGet("getToken")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetToken([FromQuery] LoginModel LogIn)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                ApplicationUser user = await _userManager.FindByNameAsync(LogIn.UserName);
                if (user != null)
                {
                    var signInResault = await _signInManager.CheckPasswordSignInAsync(user, LogIn.Password, false);
                    if (signInResault.Succeeded)
                    {

                        var rollNames = await _userManager.GetRolesAsync(user);
                        var tokenhandler = new JwtSecurityTokenHandler();
                        var key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("SecurityKey"));
                        var tokenDescriptor = new SecurityTokenDescriptor
                        {
                            Subject = new ClaimsIdentity(new Claim[]
                            {
                      new Claim(ClaimTypes.NameIdentifier ,user.Id ),
                    new Claim(ClaimTypes.Name ,LogIn.UserName ),


                            }),
                            Expires = DateTime.UtcNow.AddDays(365),
                            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                        };
                        foreach (var roll in rollNames)
                        {
                            tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Role, roll));
                        }


                        var token = tokenhandler.CreateToken(tokenDescriptor);
                        var tokenStr = tokenhandler.WriteToken(token);

                        return Ok(new  { Token = tokenStr });
                    }
                    else
                    {
                        return Unauthorized("faild , Try again");
                    }
                }
                return Unauthorized("faild , Try again");
            }
            catch(Exception E)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
           



            // return unauthorized


        }


        [AllowAnonymous]
        [HttpPost("RegisterDefaults")]
        public async Task<ActionResult> RegisterDefaults([FromBody] LoginModel logInModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                if (logInModel.UserName == "JohnFrancis" && logInModel.Password == "TestPassword")
                {
                    using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                    {
                        foreach (var rollName in new string[] { "Brewery", "WholeSaler" })
                        {
                            var IsRollExistes = await _rollManager.RoleExistsAsync(rollName);
                            if (!IsRollExistes)
                            {
                                IdentityResult Resault = await _rollManager.CreateAsync(new IdentityRole() { Name = rollName });
                                if (!Resault.Succeeded)
                                {
                                    foreach (var err in Resault.Errors)
                                    {
                                        _logger.LogError(err.Description);
                                    }
                                    return StatusCode((int)HttpStatusCode.InternalServerError);
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

                        ApplicationUser WholeSaler1 = new ApplicationUser()
                        {
                            Name = "WholeSaler1",
                            UserName = "WholeSaler1"
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
                        new Tuple<ApplicationUser, string>(WholeSaler1 ,"WholeSaler" ),
                        new Tuple<ApplicationUser, string>(WholeSaler2 ,"WholeSaler" ),

                    };


                        foreach (var userPair in Users)
                        {

                            var user = await _userManager.FindByNameAsync(userPair.Item1.UserName);
                            if (user == null)
                            {
                                IdentityResult Resault = await _userManager.CreateAsync(userPair.Item1, "TestPassword01");

                                if (!Resault.Succeeded)
                                {
                                    foreach (var err in Resault.Errors)
                                    {
                                        _logger.LogError(err.Description);
                                    }
                                    return StatusCode((int)HttpStatusCode.InternalServerError);
                                }


                            }



                            bool isUserinRoll = await _userManager.IsInRoleAsync(user, userPair.Item2);
                            if (!isUserinRoll)
                            {
                                IdentityResult rollResault = await _userManager.AddToRoleAsync(user, userPair.Item2);
                                if (!rollResault.Succeeded)
                                {
                                    foreach (var err in rollResault.Errors)
                                    {
                                        _logger.LogError(err.Description);
                                    }
                                    return StatusCode((int)HttpStatusCode.InternalServerError);
                                }
                            }



                        }



                        scope.Complete();


                    }

                    return Ok();
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch(Exception E)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        
        }
    }
}
