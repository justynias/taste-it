using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using taste_it.Additionals.ContentNavigationService;
using taste_it.Additionals.Messages;
using taste_it.DataService;
using taste_it.Models;

namespace taste_it.ViewModels
{
    public class AllRecipesViewModel : ViewModelBase, IPageViewModel
    {
        private IRecipeDataService _recipeDataService;
        private ObservableCollection<Recipe> recipesCollection;
        public string name
        {
            get
            {
                return "Recipes";
            }
        }

        public ObservableCollection<Recipe> RecipesCollection
        {
            get
            {
                return recipesCollection;
            }

            set
            {
                Set(ref recipesCollection, value);
            }
        }

        public AllRecipesViewModel(IRecipeDataService recipeData)
        {
            _recipeDataService = recipeData;
            RecipesCollection = new ObservableCollection<Recipe>();
            LoadRecipes();
        }

        private async void LoadRecipes() //when view is loading after getting message with current user
        {

            var recipes = await _recipeDataService.GetRecipesAsync();
            RecipesCollection.Clear();
            foreach (var item in recipes)
            {
                RecipesCollection.Add(item);
            }
            RaisePropertyChanged(() => RecipesCollection);

        }

    }
}
