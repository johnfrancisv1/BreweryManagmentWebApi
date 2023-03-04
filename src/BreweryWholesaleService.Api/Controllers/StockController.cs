using BreweryWholesaleService.Api.Helper;
using BreweryWholesaleService.Core.Helper;
using BreweryWholesaleService.Core.Interfaces.Services;
using BreweryWholesaleService.Core.Models.Beer;
using BreweryWholesaleService.Core.Models.Stock;
using BreweryWholesaleService.Core.StaticData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;

namespace BreweryWholesaleService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IStockService _StockService ;

        public StockController(IStockService StockService)
        {
            this._StockService= StockService ;  
        }

        [HttpPost("AddBeerToWholeSaler")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddBeerToWholeSaler([FromBody] RegisterBeertoWholeSalerModel RegisterBeertoWholeSalerModel)
        {
            if (RegisterBeertoWholeSalerModel == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
               int resault = await _StockService.AddBeerToWholeSaler(RegisterBeertoWholeSalerModel);
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



        [HttpPut("UpdateBeerQuantity")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = RollNames.WholeSaler)]
        public async Task<IActionResult> UpdateBeerQuantity([FromBody] StockUpdateRequest StockUpdateRequest)
        {

            var WholeSalerUserID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (WholeSalerUserID == null)
            {
                return Unauthorized();
            }


            if (StockUpdateRequest == null)
            {
                return BadRequest();
            }

            try
            {
                int resault = await _StockService.UpdateBeerQuantity(StockUpdateRequest, WholeSalerUserID);
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
