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
        private ObservableCollection<Recipe> filteredRecipesCollection;

        private User _currentUser;
        public ICommand AddRecipeToFavouritesCommand { get; private set; }
        public ICommand RemoveRecipeToFavouritesCommand { get; private set; }

        private string filterRecipeName;
        private ObservableCollection<Tag> filterTags;
        private ObservableCollection<Category> filterCategories;
        public string name
        {
            get
            {
                return "Recipes";
            }
        }

        public ObservableCollection<Recipe> RecipesCollection { get; set; }
        //{
        //    get
        //    {
        //        return recipesCollection;
        //    }

        //    set
        //    {
        //        Set(ref recipesCollection, value);
        //    }
        //}
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
        public AllRecipesViewModel(IRecipeDataService recipeData)
        {
            _recipeDataService = recipeData;
            RecipesCollection = new ObservableCollection<Recipe>();
            FilteredRecipesCollection = new ObservableCollection<Recipe>();


            AddRecipeToFavouritesCommand = new RelayCommand<object>(AddRecipeToFavourites);
            RemoveRecipeToFavouritesCommand = new RelayCommand<object>(RemoveRecipeToFavourites);
            

            Messenger.Default.Register<FiltersMessage>(this, this.HandleFiltersMessage);
            Messenger.Default.Register<CurrentUserMessage>(this, this.HandleCurrentUserMessage);

        }

        private void HandleFiltersMessage(FiltersMessage message)
        {
            this.filterCategories = message.FilterCategories;
            this.filterTags = message.FilterTags ;
            this.filterRecipeName = message.FilterName ;
            Filter();
            Debug.WriteLine(FilteredRecipesCollection.Count());

        }
        private void HandleCurrentUserMessage(CurrentUserMessage message)
        {
            this._currentUser = message.CurrentUser;
            Debug.WriteLine(_currentUser.name);
            //after know which user is logged
            LoadRecipes();

        }
        private void Filter()  //(name) (tags) (categories) (name, caegories) (name, tags) (categories, tags) (categories, tags, name)
        {
            //if(filterRecipeName != string.Empty && filterTags.Count==0 && filterCategories.Count()==0)
            //{
            //    var filteredByName = FilterByName();
            //}
            //else if (filterTags.Count > 0 && filterRecipeName != string.Empty && filterCategories.Count() == 0)
            //{
            //    var filteredByTags = FilterByTags();
            //}
            //else if (filterCategories.Count() > 0 && filterRecipeName != string.Empty && filterTags.Count == 0)
            //{
            //    var filteredByCategories = FilterByCategories();
            //}
            //else if (filterRecipeName == string.Empty && filterTags.Count > 0 && filterCategories.Count() > 0)
            //{
            //    var filteredByCategories = FilterByCategories();
            //    var filteredByTags = FilterByTags();

            //}
            //else if(filterRecipeName != string.Empty && filterTags.Count > 0 && filterCategories.Count() == 0)
            //{
            //    var filteredByCategories = FilterByCategories();
            //    var filteredByTags = FilterByTags();

            //}
            //else if (filterRecipeName != string.Empty && filterCategories.Count() > 0 && filterTags.Count == 0)
            //{
            //    var filteredByCategories = FilterByCategories();
            //    var filteredByName = FilterByName();
            //}
            List<Recipe> filteredRecipes = new List<Recipe>();

            if (filterRecipeName != string.Empty)
            {

                if (filterCategories.Count() == 0 && filterTags.Count == 0)
                {
                    var filteredByName = FilterByName();
                    filteredRecipes.AddRange(filteredByName);
                }
                else if(filterCategories.Count()> 0 && filterTags.Count == 0)
                {
                    var filteredByCategories = FilterByCategories();
                    var filteredByName = FilterByName();
                    filteredRecipes.AddRange(filteredByCategories.Intersect(filteredByName));
                }
                else if(filterCategories.Count() == 0 && filterTags.Count > 0)
                {
                    var filteredByName = FilterByName();
                    var filteredByTags = FilterByTags();
                    filteredRecipes.AddRange(filteredByName.Intersect(filteredByTags));
                }
                else
                {
                    var filteredByCategories = FilterByCategories();
                    var filteredByName = FilterByName();
                    var filteredByTags = FilterByTags();
                    HashSet<Recipe> hashSet = new HashSet<Recipe>(filteredByCategories);
                    hashSet.IntersectWith(filteredByName);
                    hashSet.IntersectWith(filteredByTags);
                    filteredRecipes.AddRange(hashSet.ToList());
                }
            }
            else if(filterTags.Count > 0)
            {
                if (filterCategories.Count > 0)
                {
                    var filteredByCategories = FilterByCategories();
                    var filteredByTags = FilterByTags();
                    filteredRecipes.AddRange(filteredByCategories.Intersect(filteredByTags));
                }
                else
                {
                    var filteredByTags = FilterByTags();
                    filteredRecipes.AddRange(filteredByTags);

                }
            }
            else if(filterCategories.Count > 0)
            {
                var filteredByCategories = FilterByCategories();
                filteredRecipes.AddRange(filteredByCategories);
            }
            FilteredRecipesCollection.Clear();
            foreach (var item in filteredRecipes)
            {
                FilteredRecipesCollection.Add(item);
            }
        }

        private List<Recipe> FilterByCategories()
        {
            List<Recipe> filteredRecipes = new List<Recipe>();
            foreach(var category in filterCategories)
            {
                foreach(var r in RecipesCollection)
                {
                    if(r.Have_category.Any(c => c.id_c == category.id_c))
                    {
                        filteredRecipes.Add(r);
                    }
                }
            }
            return filteredRecipes;
        }

        private List<Recipe> FilterByName()
        {
            var filteredRecipes = RecipesCollection.Where(r => r.name.StartsWith(filterRecipeName)).ToList(); // or consists?
            return filteredRecipes;
        }

        private List<Recipe> FilterByTags()
        {
            List<Recipe> filteredRecipes = new List<Recipe>();
            foreach (var tag in filterTags)
            {
                foreach (var r in RecipesCollection)
                {
                    if (r.Have_tags.Any(t=> t.Tag.name == tag.name))  //loading tags eralier to fasten
                    {
                        filteredRecipes.Add(r);
                    }
                }

            }
            return filteredRecipes;
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
       
        private void RemoveRecipeToFavourites(object parameter)
        {
            int id = Convert.ToInt32(parameter);
            var currentRecipe = RecipesCollection.First(r => r.id_r == id);
            Debug.WriteLine("Usuwam " + currentRecipe.name);
            _recipeDataService.RemoveFavouriteRecipe(currentRecipe, _currentUser);
            // delete relationship (currentRecipe, currentUser)
        }

        private void AddRecipeToFavourites(object parameter)
        {
            int id = Convert.ToInt32(parameter);
            var currentRecipe = RecipesCollection.First(r => r.id_r == id);
            Debug.WriteLine("Dodaje");
            _recipeDataService.AddToFavourites(_currentUser, currentRecipe);

        }
      
    }
}
