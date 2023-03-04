using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreweryWholesaleService.Core.Enums
{
   public enum ExceptionCodes : int { RecordNotFound = 0 , InvaildUserID = 1 , UnAuthorized = 2 , DbError =3, InvaildServiceDataRequest = 4 , UnprocessableEntity = 5}
    
}
