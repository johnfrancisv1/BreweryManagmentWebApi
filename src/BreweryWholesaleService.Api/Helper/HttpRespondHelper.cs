using BreweryWholesaleService.Core.Enums;

namespace BreweryWholesaleService.Api.Helper
{
    public class HttpRespondHelper
    {
        public static int GetStatusCode(int exceptionCodes)
        {
            switch (exceptionCodes)
            {
                case (int)ExceptionCodes.RecordNotFound:
                    return StatusCodes.Status404NotFound;
                case (int)ExceptionCodes.UnAuthorized:
                    return StatusCodes.Status401Unauthorized;
                case (int)ExceptionCodes.InvaildServiceDataRequest:
                    return StatusCodes.Status400BadRequest;
                case (int)ExceptionCodes.InvaildUserID:
                    return StatusCodes.Status401Unauthorized;
                case (int)ExceptionCodes.UnprocessableEntity:
                    return StatusCodes.Status422UnprocessableEntity;
                case (int)ExceptionCodes.DbError:
                    return StatusCodes.Status500InternalServerError;
                default:
                    return StatusCodes.Status500InternalServerError;
            }
        }
    }
}
