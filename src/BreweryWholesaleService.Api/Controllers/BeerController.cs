
using BreweryWholesaleService.Api.Helper;
using BreweryWholesaleService.Core.EntityModels;
using BreweryWholesaleService.Core.Helper;
using BreweryWholesaleService.Core.Interfaces.Services;
using BreweryWholesaleService.Core.Models.Beer;
using BreweryWholesaleService.Core.Services;
using BreweryWholesaleService.Infrastructure.EntityModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BreweryWholesaleService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BeerController : ControllerBase
    {
        private readonly IBeerService _beerService;
        private readonly UserManager<ApplicationUser> _UserManager;
        public BeerController(IBeerService beerService, UserManager<ApplicationUser> UserManager)
        {
            this._beerService = beerService;
            this._UserManager = UserManager;
        }

        [HttpPost("AddNewBear")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "Brewery")]
        public async Task<IActionResult> AddNewBear([FromBody] RegisterNewBeerModel NewBearModel)
        {

           var UserID = User.FindFirstValue(ClaimTypes.NameIdentifier);
           
            

            if(NewBearModel == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            try
            {
                int result = await _beerService.CreateNewBeer(NewBearModel, UserID);

                return Ok(result);
            }
            catch (MyException E)
            {
                return StatusCode(HttpRespondHelper.GetStatusCode(E.ExceptionCode), E.Message);

            }
            catch (Exception E)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }




        }


        [HttpDelete("DeleteBear/{BearName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "Brewery")]
        public async Task<IActionResult> DeleteBeer(string BearName)
        {

            var UserID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (UserID == null)
            {
                return Unauthorized();
            }


            try
            {
               int Result = await _beerService.DeleteBeerByName(BearName);
                return NoContent();
            }catch(MyException E)
            {
                return StatusCode(HttpRespondHelper.GetStatusCode(E.ExceptionCode), E.Message);
              
            }
            catch (Exception E)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }

         

   


        }





        [HttpGet("GetBeersByBrewery/{breweryName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetBeersByBrewery(string breweryName)
        {
            if (String.IsNullOrEmpty(breweryName))
            {
                return BadRequest();
            }
          

            try
            {
                var resault = await _beerService.GetBeersByBreweryName(breweryName);
                
                return Ok(resault);
            }
            catch (MyException E)
            {
                return StatusCode(HttpRespondHelper.GetStatusCode(E.ExceptionCode), E.Message);

            }
            catch (Exception E)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }








        }


    }
}
