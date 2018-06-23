using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using taste_it.Models;

namespace taste_it.DataService
{
    public class CategoryDataService : ICategoryDataService
    {
        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            var dbContext = new TasteItDbEntities();
            return await dbContext.Categories.AsNoTracking().ToListAsync();
        }
    }
}
