using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using taste_it.Additionals.ContentNavigationService;
using taste_it.Additionals.Messages;
using taste_it.DataService;
using taste_it.Models;

namespace taste_it.ViewModels
{
    class FavouriteRecipesViewModel : ViewModelBase, IPageViewModel
    {

        private User _currentUser;
        private IRecipeDataService _recipeDataService;
        private ObservableCollection<Recipe> recipesCollection;
        public ICommand AddRecipeToFavouritesCommand { get; private set; }
        public ICommand RemoveRecipeToFavouritesCommand
        { get; private set; }
        public string name
        {
            get
            {
                return "Favourite Recipes";
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

        public FavouriteRecipesViewModel(IRecipeDataService recipeData)
        {
            _recipeDataService = recipeData;
            Messenger.Default.Register<CurrentUserMessage>(this, this.HandleCurrentUserMessage);
            RecipesCollection = new ObservableCollection<Recipe>();
            AddRecipeToFavouritesCommand = new RelayCommand<object>(AddRecipeToFavourites);
            RemoveRecipeToFavouritesCommand = new RelayCommand<object>(RemoveRecipeToFavourites);
        }

      private async void LoadRecipes() //when view is loading after getting message with current user
        {

            var recipes = await _recipeDataService.FindFavouritesAsync(_currentUser);

            RecipesCollection.Clear();
            foreach (var item in recipes)
            {
                if (item.Have_favourites.Any(u => u.id_u == _currentUser.id_u))
                {
                    item.isFavourite = true;
                }
                RecipesCollection.Add(item);
            }
            RaisePropertyChanged(() => RecipesCollection);

        }
        private void HandleCurrentUserMessage(CurrentUserMessage message)
        {
            this._currentUser = message.CurrentUser;
            LoadRecipes();

        }
        private void RemoveRecipeToFavourites(object parameter)
        {
            int id = Convert.ToInt32(parameter);
            var currentRecipe = RecipesCollection.First(r => r.id_r == id);
            _recipeDataService.RemoveFavouriteRecipe(currentRecipe, _currentUser);
            // delete relationship (currentRecipe, currentUser)
        }

        private void AddRecipeToFavourites(object parameter)
        {
            int id = Convert.ToInt32(parameter);
            var currentRecipe = RecipesCollection.First(r => r.id_r == id);
            _recipeDataService.AddToFavourites(_currentUser, currentRecipe);

        }
    }
}
