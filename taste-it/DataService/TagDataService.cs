using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using taste_it.Models;

namespace taste_it.DataService
{
    public class TagDataService : ITagDataService
    {
        public async Task AddTagAsync(Tag tag)
        {
            var dbContext = new TasteItDbEntities();
            dbContext.Tags.Add(tag);
            await dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Tag>> GetTagsAsync()
        {
            var dbContext = new TasteItDbEntities();
            return await dbContext.Tags.AsNoTracking().ToListAsync();
        }
        public async Task<Tag> FindTag(Tag tag)
        {
            var dbContext = new TasteItDbEntities();
            Tag current = await dbContext.Tags.FirstOrDefaultAsync(t => t.name == tag.name);
            return current;
        }
    }
}
