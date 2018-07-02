using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using taste_it.Additionals.ContentNavigationService;
using taste_it.Additionals.Messages;
using taste_it.DataService;
using taste_it.Models;

namespace taste_it.ViewModels
{
    class FavouriteRecipesViewModel : ViewModelBase, IPageViewModel
    {
        private ObservableCollection<Recipe> recipesCollection;
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

        public FavouriteRecipesViewModel()
        {
            RecipesCollection = new ObservableCollection<Recipe>();

            Messenger.Default.Register<RecipesCollectionMessage>(this, this.HandleRecipesCollectionMessage);
        }

 
        private void HandleRecipesCollectionMessage(RecipesCollectionMessage message)
        {
            RecipesCollection.Clear();

            var recipes = message.RecipesCollection;
            foreach (var item in recipes)
            {
                if(item.isFavourite)
                {
                    RecipesCollection.Add(item);
                    Debug.WriteLine(item.id_r);
                }
            }
           
        }

       
    }
}
