using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using taste_it.Additionals.ContentNavigationService;
using taste_it.Additionals.Messages;
//using taste_it.Additionals.Messages;
using taste_it.Additionals.NavigationService;
using taste_it.DataService;
using taste_it.Models;

namespace taste_it.ViewModels
{

    public class AddRecipeViewModel : ViewModelBase, IDataErrorInfo, IPageViewModel
    {
        #region private fields
        private IFrameNavigationService _navigationService;
        private readonly IRecipeDataService _recipeDataService;
        private readonly ICategoryDataService _categoryDataService;
        private readonly ITagDataService _tagDataService;

        private string recipeName;
        private string recipeIngredients;
        private string description;
        private string duration;   
        private int complexity;
        private Tag currentTag;
        private string tagName;
        private Category currentCategory;
        private ObservableCollection<Tag> tags; 
        private ObservableCollection<Category> categoriesCollection;
        private TimeSpan durationTime;
        private string tagError;
        private bool durationValid;
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
                int newDuration;
                Int32.TryParse(Duration, out newDuration);
                durationTime = TimeSpan.FromMinutes(newDuration);
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

        public string Duration
        {
            get
            {
                return duration;
            }

            set
            {
                var regex = new Regex("[^0-9]+");
                durationValid = !regex.IsMatch(value);

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

        #endregion

        //ctr
        public AddRecipeViewModel(IRecipeDataService recipeData, ITagDataService tagData, ICategoryDataService categoryData)
        {
            _recipeDataService = recipeData;
            _tagDataService = tagData;
            _categoryDataService = categoryData;
            LoadCategories();

            LoadTags();

            AddRecipeCommand = new RelayCommand(AddRecipe, ()=>IsRecipeValid()); 
            ResetRecipeCommand = new RelayCommand(ResetRecipe);
            AddTagCommand = new RelayCommand(AddTag);
            RemoveTagCommand = new RelayCommand<object>(RemoveTag);


        }

        #region methods
        private async void AddRecipe()
        {
            var newRecipe = new Recipe() { name = RecipeName, ingredients = RecipeIngredients, description = Description, complexity = Complexity, duration = int.Parse(Duration) };
            await AddTagsToDatabase();
            var tagList = new List<Tag>(Tags);
            await _recipeDataService.AddRecipeAsync(newRecipe, CurrentCategory, tagList);
            NotifyAboutRefresh();
            ResetRecipe();
        }
        private void ResetRecipe()
        {
            RecipeName = string.Empty;
            RecipeIngredients = string.Empty;
            Description = string.Empty;
            Tags.Clear();
            CurrentCategory = null;
            Complexity = 1;
            Duration = "0";

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
        private void LoadTags()
        {
            Tags = new ObservableCollection<Tag>();
        }
        private void RemoveTag(object parameter)
        {
            string name = (string)parameter;
            Tags.Remove(Tags.Where(i => i.name == name).Single());
            RaisePropertyChanged(() => Tags);
        }
        private void AddTag()
        {
            CurrentTag = new Tag() { name = TagName };
            if(IsTagsValid(ref tagError))
            {
                CurrentTag.name = "#" + CurrentTag.name;
                Tags.Add(CurrentTag);
                RaisePropertyChanged(() => Tags);
            }

            TagName = string.Empty;
        }
       
        private void SetLoaderOn()
        {
            Messenger.Default.Send<GenericMessage<bool>>(new GenericMessage<bool>(true));
        }
        private void SetLoaderOff()
        {
            Messenger.Default.Send<GenericMessage<bool>>(new GenericMessage<bool>(false));
        }
        private void NotifyAboutRefresh()
        {
            Messenger.Default.Send<RefreshMessage>(new RefreshMessage());
        }
        #endregion
        #region validation
        private bool IsTagsValid(ref string message)
        {
            message = string.Empty;

            if (Tags.Count > 10)
            {
                message = "Number of tags must be maximum 10";
                return false;
            }
            else if (string.IsNullOrWhiteSpace(TagName) || string.IsNullOrEmpty(TagName))
            {
                message = "Field should not be empty";
                return false;
            }
            else if (TagName.Length > 20)
            {
                message = "Tag should contains maximum 20 characters";
                return false;
            }
            else if (TagName.Contains(" "))
            {
                message = "Tag should not contains spaces!";
                return false;
            }
            else if (Tags.Any(item => item.name.Substring(1) == CurrentTag.name))
            {
                message = "Tag already exists!";
                return false;
            }
            
            return true;
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

        private bool IsTagsNumberValid(out string message)
        {
            message = string.Empty;
            if (Tags.Count < 2)
            {
                message = "Number of tags must be minimum 2";
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

            if (string.IsNullOrWhiteSpace(Description) || string.IsNullOrEmpty(Description))
            {
                message = "Field should not be empty";
                return false;
            }
            else if (Description.Length > 1000)
            {
                message = "the maximum number of characters is 1000";
                return false;

            }
            return true;

        }
        private bool IsDurationValid(out string message)
        {
            message = string.Empty;
            if (Duration == "0")
            {
                message = "Insert duration value!";
                return false;
            }
            else if (!durationValid)
            {
                durationValid = false;
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
            return (IsCategoryValid(out temp) && IsComplexityValid(out temp) && IsDescriptionValid(out temp) && IsTagsNumberValid(out temp)
                && IsDurationValid(out temp) && IsRecipeIngredientsValid(out temp) && IsRecipeNameValid(out temp));
        }
        #endregion


        #region IDataErrorInfo Members
        public string this[string columnName] 
        {                                     
            get                                  
            {
                string result = string.Empty;
                switch (columnName)
                {
                    case "RecipeName":
                            if (!IsRecipeNameValid(out result))
                            {
                                return result;
                            }
                            break;
                    case "RecipeIngredients":
                            if (!IsRecipeIngredientsValid(out result))
                            {
                                return result;
                            }
                            break;
                    case "Duration":
                        if (!IsDurationValid(out result))
                        {
                            return result;
                        }
                        break;
                    case "Description":
                            if (!IsDescriptionValid(out result))
                            {
                                return result;
                            }
                        break;
                    case "Tags":
                        if (!IsTagsNumberValid(out result))
                        {
                            return result;
                        }
                        break;
                    case "TagName":
                        if (tagError != string.Empty)
                        {
                            return tagError;
                        }
                        break;
                    case "CurrentCategory":
                        if (!IsCategoryValid(out result))
                        {
                            return result;
                        }
                        break;
                    case "Complexity":
                        if (!IsComplexityValid(out result))
                        {
                            return result;
                        }
                        break;

                }
                return result;
            }
        }

        public string Error
        {
            get { return null; }
        }



        #endregion
        #region IPageViewModel Member
        public string name
        {
            get
            {
                return "Add Recipe";
            }
        }
        #endregion


    }
}