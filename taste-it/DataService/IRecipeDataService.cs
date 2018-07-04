using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using taste_it.Models;

namespace taste_it.DataService
{
    public interface IRecipeDataService
    {
        Task<IEnumerable<Recipe>> GetRecipesAsync();
        Task AddRecipeAsync(Recipe recipe,Category category, Tag tag);
        Task AddRecipeAsync(Recipe recipe, Category category, List<Tag> tags);
        Task AddToFavourites(User user, Recipe recipe);
        Task RemoveFavouriteRecipe(Recipe recipe, User user);
    }
}
