using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

     
        private ObservableCollection<Recipe> filteredRecipesCollection;

        public ICommand AddRecipeToFavouritesCommand { get; private set; }
        public ICommand RemoveRecipeToFavouritesCommand
        { get; private set; }
        public ICommand NavigateToCurrentRecipeCommand { get; private set; }
        public string name
        {
            get
            {
                return "Favourite Recipes";
            }
        }


        public ObservableCollection<Recipe> FilteredRecipesCollection
        {
            get
            {
                return filteredRecipesCollection;
            }

            set
            {
                Set(ref filteredRecipesCollection, value);
            }
        }

        public FavouriteRecipesViewModel()
        {

            AddRecipeToFavouritesCommand = new RelayCommand<object>(AddRecipeToFavourites);
            RemoveRecipeToFavouritesCommand = new RelayCommand<object>(RemoveRecipeToFavourites);

            Messenger.Default.Register<RecipesCollectionMessage>(this, this.HandleRecipesCollectionMessage);

        }
        private void HandleRecipesCollectionMessage(RecipesCollectionMessage message)
        {
            FilteredRecipesCollection = new ObservableCollection<Recipe>();
            var recipes = message.RecipesCollection;
            foreach (var item in recipes)
            {
                if (item.isFavourite)
                {
                    FilteredRecipesCollection.Add(item);
                    Debug.WriteLine(item.id_r);
                }
            }
            RaisePropertyChanged(() => FilteredRecipesCollection);

        }
        
        private void NavigateToCurrentRecipe(object parameter)
        {
            var currentRecipe = (Recipe)parameter;
            Messenger.Default.Send<NavigationWithCurrentRecipeMessage>(new NavigationWithCurrentRecipeMessage
            {
                CurrentRecipe = currentRecipe

            });
        }
   
        private void RemoveRecipeToFavourites(object parameter)
        {
            int id = Convert.ToInt32(parameter);
            Messenger.Default.Send<RemoveRecipeToFavMessage>(new RemoveRecipeToFavMessage
            {
                RecipeIndex = id

            });
        }

        private void AddRecipeToFavourites(object parameter)
        {
            int id = Convert.ToInt32(parameter);
            Messenger.Default.Send<AddRecipeToFavMessage>(new AddRecipeToFavMessage
            {
                RecipeIndex = id

            });
        }
    }
}
