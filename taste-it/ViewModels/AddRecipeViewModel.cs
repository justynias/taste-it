using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using taste_it.Additionals.NavigationService;
using taste_it.DataService;
using taste_it.Models;

namespace taste_it.ViewModels
{
   
    public class AddRecipeViewModel : ViewModelBase, IDataErrorInfo
    {
        #region private fields
        private User currentUser; // to merged author with the recipe // MVVM message?
        private IFrameNavigationService _navigationService;
        private readonly IRecipeDataService _recipeDataService;
        private readonly ICategoryDataService _categoryDataService; 
        private readonly ITagDataService _tagDataService;  

        private string recipeName;
        private string recipeIngredients;
        private string description;
        private int duration;   //bind from combobox? default value / bind to progress bar
        private int complexity;
        private Tag currentTag;
        private string tagName;
        private Category currentCategory; //selected item (radio button)
        private ObservableCollection<Tag> tags; //itemControl, validation?
        private ObservableCollection<Category> categoriesCollection;
        private TimeSpan durationTime;

        #endregion
        #region Properties

        public ICommand AddRecipeCommand { get; private set; }
        public ICommand ResetRecipeCommand { get; private set; }
        public ICommand AddTagCommand { get; private set; }
        public ICommand RemoveTagCommand { get; private set; }



        public string RecipeName
        {
            get
            {
                return recipeName;
            }

            set
            {

                Set(ref recipeName, value);

            }
        }
        public TimeSpan DurationTime
        {
            get
            {
                durationTime = TimeSpan.FromMinutes(Duration);
                return durationTime;
            }

            set
            {
                Set(ref durationTime, value);
            }
        }
        public string RecipeIngredients
        {
            get
            {
                return recipeIngredients;
            }

            set
            {

                Set(ref recipeIngredients, value);

            }
        }
        public string Description
        {
            get
            {
                return description;
            }

            set
            {

                Set(ref description, value);

            }
        }

        public int Duration
        {
            get
            {
                return duration;
            }

            set
            {
                if (double.IsNaN(value))
                    Set(ref duration, 0);
                else
                    Set(ref duration, value);

                RaisePropertyChanged(() => DurationTime);

            }
        }
        public int Complexity
        {
            get
            {
                return complexity;
            }

            set
            {

                Set(ref complexity, value);

            }
        }
        public ObservableCollection<Category> CategoriesCollection
        {
            get
            {
                return categoriesCollection;
            }

            set
            {

                Set(ref categoriesCollection, value);

            }
        }
        public Category CurrentCategory
        {
            get
            {
                return currentCategory;
            }

            set
            {

                Set(ref currentCategory, value);

            }
        }

        public ObservableCollection<Tag> Tags
        {
            get
            {
                return tags;
            }

            set
            {

                Set(ref tags, value);

            }
        }
        public List<int> availableComplexities
        {
            get
            {
                List<int> vals = new List<int>();
                for(int i = 1; i <= 10; i++)
                {
                    vals.Add(i);
                }
                return vals;
            }
        }
        public Tag CurrentTag
        {
            get
            {
                return currentTag;
            }

            set
            {
                Set(ref currentTag, value);
            }
        }

        public string TagName
        {
            get
            {
                return tagName;
            }

            set
            {
                Set(ref tagName, value);
            }
        }
        #endregion

        //ctr
        public AddRecipeViewModel(IRecipeDataService  recipeData,ITagDataService tagData, ICategoryDataService categoryData, IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;
            _recipeDataService = recipeData;
            _tagDataService = tagData;
            _categoryDataService = categoryData;

            LoadCategories();

            LoadTags();

            AddRecipeCommand = new RelayCommand(AddRecipe); // disable button on validation
            ResetRecipeCommand = new RelayCommand(ResetRecipe);
            AddTagCommand = new RelayCommand(AddTag);
            RemoveTagCommand = new RelayCommand<object>(RemoveTag);


        }

        #region methods
        private void AddRecipe()
        {
            //var newRecipe = new Recipe() { name = RecipeName, ingredients = RecipeIngredients, description = Description, complexity = Complexity, duration = Duration };
            //AddTags();
            //var tagList = new List<Tag>(Tags);
            //_recipeDataService.AddRecipeAsync(newRecipe, CurrentCategory, tagList);
            //ResetRecipe();
        }
        private async void LoadTags()
        {
            var tagsTemp = await _tagDataService.GetTagsAsync();
            Tags = new ObservableCollection<Tag>();
            foreach (var tag in tagsTemp)
            {
                Tags.Add(tag);
            }
        }
        private void RemoveTag(object parameter)
        {
            int id = (int)parameter;
            Tags.Remove(Tags.Where(i => i.id_t == id).Single());
        }
        private void AddTag()
        {
            CurrentTag = new Tag() { name = TagName, id_t = Tags.Count };
            Tags.Add(CurrentTag);
        }
        private void ResetRecipe()
        {
            RecipeName = string.Empty;
            RecipeIngredients = string.Empty;
            Description = string.Empty;
            Tags.Clear();
            //CurrentCategory=default?
            //Complexity = default value;
            //Duration = default value;

        }
        private async void AddTags()
        {
            if (Tags != null)
            {
                foreach (var t in Tags)
                {
                    var existingTag = await _tagDataService.FindTag(t);
                    if (existingTag != null)
                    {
                        t.id_t = existingTag.id_t;
                    }
                    else
                    {
                        await _tagDataService.AddTagAsync(t);
                        var addedTag = await _tagDataService.FindTag(t);
                        if (addedTag != null)
                        {
                            t.id_t = addedTag.id_t;
                        }
                    }

                }
            }

        }

        private async void LoadCategories()
        {
            var categoriesTemp = await _categoryDataService.GetCategoriesAsync();
            CategoriesCollection = new ObservableCollection<Category>();
            foreach (var c in categoriesTemp)
            {
                CategoriesCollection.Add(c);
            }
        }
        #endregion


        #region IDataErrorInfo Members
        public string this[string columnName] //fields should not be empty (tags validation)
        {                                     //radio button and progress bar->default value
            get                               //taglist could be empty     
            {
                string result = string.Empty;
                if (columnName == "RecipeName")
                {
                    if (IsFieldEmpty(RecipeName)) { return "Field should not be empty"; }

                }
                else if (columnName == "RecipeIngredients")
                {
                    if (IsFieldEmpty(RecipeIngredients)) { return "Field should not be empty"; }

                }
                else if (columnName == "Duration")
                {
                    if (double.IsNaN(Duration)) { return "Field must be numeric value"; }

                }
                else if (columnName == "Description")
                {
                    if (IsFieldEmpty(Description)) { return "Field should not be empty"; }

                }
                return result;
            }
        }

        public string Error
        {
            get { return null; }
        }

       

        private bool IsFieldEmpty(string field)
        {
            return (string.IsNullOrWhiteSpace(field) || string.IsNullOrEmpty(field));
        }

        #endregion

    }
}