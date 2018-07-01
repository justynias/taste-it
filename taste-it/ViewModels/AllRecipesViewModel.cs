using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using taste_it.Additionals.ContentNavigationService;
using taste_it.Additionals.Messages;
using taste_it.DataService;
using taste_it.Models;
using System.Globalization;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using System.Diagnostics;

namespace taste_it.ViewModels
{
    public class AllRecipesViewModel : ViewModelBase, IPageViewModel
    {
        private IRecipeDataService _recipeDataService;
        private ObservableCollection<Recipe> recipesCollection;
        private User _currentUser;
        public ICommand AddRecipeToFavouritesCommand { get; private set; }

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
            Messenger.Default.Register<CurrentUserMessage>(this, this.HandleCurrentUserMessage);
            AddRecipeToFavouritesCommand = new RelayCommand<object>(AddRecipeToFavourites);

            //LoadRecipes();
        }

        private async void LoadRecipes() //when view is loading after getting message with current user
        {

            var recipes = await _recipeDataService.GetRecipesAsync();
            RecipesCollection.Clear();
            foreach (var item in recipes)
            {
                if(item.Have_favourites.Any(u => u.id_u==_currentUser.id_u))
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

        private void AddRecipeToFavourites(object parameter)
        {
            int id = Convert.ToInt32(parameter);
            var currentRecipe = RecipesCollection.First(r => r.id_r == id); 
            //check if already exist 

            _recipeDataService.AddToFavourites(_currentUser, currentRecipe);

        }
        //private void RemoveTag(object parameter)
        //{
        //    string name = (string)parameter;
        //    Tags.Remove(Tags.Where(i => i.name == name).Single());
        //    RaisePropertyChanged(() => Tags);
        ////}

    }
}
