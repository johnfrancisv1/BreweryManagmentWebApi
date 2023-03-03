using BreweryWholesaleService.Api.Helper;
using BreweryWholesaleService.Core.Helper;
using BreweryWholesaleService.Core.Interfaces.Services;
using BreweryWholesaleService.Core.Models.Sales;
using BreweryWholesaleService.Core.Models.Stock;
using BreweryWholesaleService.Core.Services;
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
    public class SalesController : ControllerBase
    {
        private readonly ISalesService _SalesService;

        public SalesController(ISalesService SalesService)
        {
            this._SalesService = SalesService;
        }


        [HttpGet("UpdateBeerQuantity")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = RollNames.WholeSaler)]
        public async Task<IActionResult> UpdateBeerQuantity([FromBody] QuoteRequest QuoteRequest)
        {

            var WholeSalerUserID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (WholeSalerUserID == null)
            {
                return Unauthorized();
            }


            if (QuoteRequest == null)
            {
                return BadRequest();
            }

            try
            {
                SaleQuote SaleQuote  = await _SalesService.GetSaleQuote(QuoteRequest, WholeSalerUserID);
                return Ok(SaleQuote);
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
