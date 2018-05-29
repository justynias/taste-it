using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using taste_it.Models;

namespace taste_it.DataService
{
    public class UserDataService : IUserDataService
    {
        private TasteItDbEntities dbContext;
        public UserDataService()
        {
            dbContext = new TasteItDbEntities();
        }
        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await dbContext.Users.AsNoTracking().ToListAsync();   
        }
        public async Task AddUserAsync(User user)
        {
            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();
        }
    }
}
