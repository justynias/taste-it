using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using taste_it.Models;

namespace taste_it.DataService
{
    public interface ITagDataService
    {
        Task<IEnumerable<Tag>> GetTagsAsync();
        Task AddTagAsync(Tag tag);
    }
}
