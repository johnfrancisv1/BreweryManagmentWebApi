using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreweryWholesaleService.Core.Interfaces.Repositories
{
    public interface IUserRepository
    {

        Task<string> GetUserIDByUserName(string UserName);
       
    }
}
