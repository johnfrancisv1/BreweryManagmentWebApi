using BreweryWholesaleService.Core.Interfaces.Repositories;
using BreweryWholesaleService.Infrastructure.EntityModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreweryWholesaleService.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationContext _DbContext;
        public UserRepository(ApplicationContext DbContext)
        {
            this._DbContext= DbContext;
        }
        public async Task<string> GetUserIDByUserName(string UserName)
        {
           return await _DbContext.Users.AsNoTracking().Where(u => u.UserName == UserName).Select(U => U.Id).SingleOrDefaultAsync();
        }

       
    }
}
