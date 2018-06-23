using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using taste_it.Models;

namespace taste_it.DataService
{
    public class RecipeDataService : IRecipeDataService
    {
        public async Task AddRecipeAsync(Recipe recipe,Category category, Tag tag)
        {
            var dbContext = new TasteItDbEntities();

            dbContext.Recipes.Add(recipe);
            var currentRecipe = await dbContext.Recipes.FindAsync(recipe);

            if(currentRecipe !=null)
            {

                dbContext.Have_category.Add(new Have_category{ id_c = category.id_c, id_r = currentRecipe.id_r });
                dbContext.Have_tags.Add(new Have_tags {id_t = tag.id_t,id_r=currentRecipe.id_r });
            }


            await dbContext.SaveChangesAsync();
        }

        public async Task AddRecipeAsync(Recipe recipe, List<Category> categories, List<Tag> tags)
        {
            var dbContext = new TasteItDbEntities();

            dbContext.Recipes.Add(recipe);
            var currentRecipe = await dbContext.Recipes.FindAsync(recipe);

            if (currentRecipe != null)
            {
                foreach (var category in categories)
                {
                    dbContext.Have_category.Add(new Have_category { id_c = category.id_c, id_r = currentRecipe.id_r });
                }

                foreach (var tag in tags)
                {
                    dbContext.Have_tags.Add(new Have_tags { id_t = tag.id_t, id_r = currentRecipe.id_r });
                }
                
            }

            await dbContext.SaveChangesAsync();
        }

        public async Task AddToFavourites(User user, Recipe recipe)
        {
            var dbContext = new TasteItDbEntities();

            dbContext.Have_favourites.Add(new Have_favourites { id_u = user.id_u, id_r = recipe.id_r });

            await dbContext.SaveChangesAsync();

        }

        public Task<IEnumerable<Recipe>> FindByCategory(Category category)
        {
            // var dbContext = new TasteItDbEntities();

            throw new NotImplementedException();
        }

        public Task<IEnumerable<Recipe>> FindByTagAsync(Tag tag)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Recipe>> FindFavouritesAsync(User user)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Recipe>> GetRecipesAsync()
        {
            var dbContext = new TasteItDbEntities();

            return await dbContext.Recipes.AsNoTracking().ToListAsync();
        }
    }
}
