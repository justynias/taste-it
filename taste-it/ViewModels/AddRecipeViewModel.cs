using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
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
        private bool isTagExistingAlready;
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
                for (int i = 1; i <= 10; i++)
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
        public bool IsTagExistingAlready
        {
            get
            {
                return isTagExistingAlready;
            }

            set
            {
                Set(ref isTagExistingAlready, value);
            }
        }
        #endregion

        //ctr
        public AddRecipeViewModel(IRecipeDataService recipeData, ITagDataService tagData, ICategoryDataService categoryData, IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;
            _recipeDataService = recipeData;
            _tagDataService = tagData;
            _categoryDataService = categoryData;

            LoadCategories();

            LoadTags();

            AddRecipeCommand = new RelayCommand(AddRecipe, ()=>IsRecipeValid()); // disable button on validation
            ResetRecipeCommand = new RelayCommand(ResetRecipe);
            AddTagCommand = new RelayCommand(AddTag);
            RemoveTagCommand = new RelayCommand<object>(RemoveTag);


        }

        #region methods
        private async void AddRecipe()
        {
            var newRecipe = new Recipe() { name = RecipeName, ingredients = RecipeIngredients, description = Description, complexity = Complexity, duration = Duration };
            await AddTagsToDatabase();
            var tagList = new List<Tag>(Tags);
            await _recipeDataService.AddRecipeAsync(newRecipe, CurrentCategory, tagList);
            ResetRecipe();
        }
        private void LoadTags()
        {
            Tags = new ObservableCollection<Tag>();
        }
        private void RemoveTag(object parameter)
        {
            string name = (string)parameter;
            Tags.Remove(Tags.Where(i => i.name == name).Single());
        }
        private void AddTag()
        {
            CurrentTag = new Tag() { name = TagName };
            isTagExistingAlready = Tags.Any(item => item.name == CurrentTag.name);
            if (!isTagExistingAlready && Tags.Count<11)
            {
                Tags.Add(CurrentTag);

            }
            TagName = string.Empty;
        }
        private void ResetRecipe()
        {
            RecipeName = string.Empty;
            RecipeIngredients = string.Empty;
            Description = string.Empty;
            Tags.Clear();
            CurrentCategory = null;
            Complexity = 0;
            Duration = 0;

        }
        private async Task AddTagsToDatabase()
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
                    if (!IsRecipeNameValid(out result))
                    {
                        return result;
                    }

                }
                else if (columnName == "RecipeIngredients")
                {
                    if (!IsRecipeIngredientsValid(out result))
                    {
                        return result;
                    }

                }
                else if (columnName == "Duration")
                {
                    if (!IsDurationValid(out result))
                    {
                        return result;
                    }

                }
                else if (columnName == "Description")
                {
                    if (!IsDescriptionValid(out result))
                    {
                        return result;
                    }

                }
                else if (columnName == "TagName")
                {

                    if (!IsTagsValid(out result))
                    {
                        return result;
                    }
                }
                else if (columnName == "CurrentCategory")
                {

                    if (!IsCategoryValid(out result))
                    {
                        return result;
                    }
                }
                else if (columnName == "Complexity")
                {

                    if (!IsComplexityValid(out result))
                    {
                        return result;
                    }
                }

                return result;
            }
        }

        public string Error
        {
            get { return null; }
        }




        private bool IsRecipeNameValid(out string message)
        {
            message = string.Empty;

            if (string.IsNullOrWhiteSpace(RecipeName) || string.IsNullOrEmpty(RecipeName))
            {
                message = "Field should not be empty";
                return false;
            }
            else if (RecipeName.Length > 50)
            {
                message = "Length of name should not exceed more than 50 char";
                return false;
            }
            return true;
        }
        private bool IsTagsValid(out string message)
        {
            message = string.Empty;
            if (isTagExistingAlready)
            {
                message = "Tag already exists!";
                return false;
            }
            else if (Tags.Count < 2)
            {
                message = "Number of tags must be minimum 2";
                return false;
            }
            else if (Tags.Count > 10)
            {
                message = "Number of tags must be maximum 10";
                return false;
            }
            return true;
        }
        private bool IsRecipeIngredientsValid(out string message)
        {
            message = string.Empty;

            if (string.IsNullOrWhiteSpace(RecipeIngredients) || string.IsNullOrEmpty(RecipeIngredients))
            {
                message = "Field should not be empty";
                return false;
            }
            else if (RecipeIngredients.Length > 500)
            {
                message = "the maximum number of characters is 500";
                return false;

            }
            return true;

        }
        private bool IsDescriptionValid(out string message)
        {
            message = string.Empty;

            if (string.IsNullOrWhiteSpace(RecipeIngredients) || string.IsNullOrEmpty(RecipeIngredients))
            {
                message = "Field should not be empty";
                return false;
            }
            else if (RecipeIngredients.Length > 1000)
            {
                message = "the maximum number of characters is 1000";
                return false;

            }
            return true;

        }
        private bool IsDurationValid(out string message)
        {
            message = string.Empty;
            if (Duration == 0)
            {
                message = "Insert duration value!";
                return false;
            }
            else if (double.IsNaN(Duration))
            {
                message = "Duration must have numeric value";
                return false;
            }
            else return true;
        }

        private bool IsCategoryValid(out string message)
        {
            message = string.Empty;
            if (CurrentCategory == null)
            {
                message = "Select Category!";
                return false;
            }
            else return true;
        }
        private bool IsComplexityValid(out string message)
        {
            message = string.Empty;
            if (Complexity == 0)
            {
                message = "Select complexity!";
                return false;
            }
            else return true;
        }
        private bool IsRecipeValid()
        {
            string temp;
            return (IsCategoryValid(out temp) && IsComplexityValid(out temp) && IsDescriptionValid(out temp)
                && IsDurationValid(out temp) && IsRecipeIngredientsValid(out temp) && IsRecipeNameValid(out temp) && IsTagsValid(out temp));
        }

        #endregion
    }
}