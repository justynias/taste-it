using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using taste_it.Additionals.ContentNavigationService;
using taste_it.Additionals.Messages;
using taste_it.Models;

namespace taste_it.ViewModels
{
    public class CurrentRecipeViewModel : ViewModelBase, IPageViewModel
    {
        private Recipe currentRecipe;

        public Recipe CurrentRecipe
        {
            get
            {
                return currentRecipe;
            }

            set
            {
                Set(ref currentRecipe, value);
            }
        }

        public string name
        {
            get
            {
                return "Current Recipe";
            }
        }

        public CurrentRecipeViewModel(Recipe CurrentRecipe)
        {
            this.CurrentRecipe = CurrentRecipe;
        }

    }
}
