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
        //private readonly ICategoryDataService _categoryDataService;  //get categories list 
        //private readonly ITagDataService _tagDataService;  // check if tag exists

        private string recipeName;
        private string recipeIngredients;
        private string description;
        private int duration; 
        private int complexity;
        private ObservableCollection<Tag> tags;
        private ObservableCollection<Category> categories;

        #endregion
        #region properties

        public ICommand AddRecipeCommand { get; private set; }
        public ICommand ResetRecipeCommand { get; private set; }


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

                Set(ref duration, value);

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
        public ObservableCollection<Category> Categories
        {
            get
            {
                return categories;
            }

            set
            {

                Set(ref categories, value);

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


        #endregion

        //ctr
        public AddRecipeViewModel(IRecipeDataService  recipeData, IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;
            _recipeDataService = recipeData;

            AddRecipeCommand = new RelayCommand(AddRecipe);
            ResetRecipeCommand = new RelayCommand(ResetRecipe);

        }

        #region methods
        private void AddRecipe()
        {
            
        }
        private void ResetRecipe()
        {
            RecipeName = string.Empty;
            RecipeIngredients = string.Empty;
            Description = string.Empty;
            //Complexity = default value;
            //Duration = default value;
            //Categories.Clear();
            //Tags.Clear();
            
        }

        #endregion


        #region IDataErrorInfo Members
        public string this[string columnName] //fields should nt be empty 
        {                                     //radio button and progress bar->default value
            get                               //tags could be empty     
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