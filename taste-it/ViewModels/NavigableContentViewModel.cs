using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using taste_it.Additionals.ContentNavigationService;
using taste_it.DataService;
using taste_it.Additionals.Messages;
using taste_it.Models;
using System.Diagnostics;

namespace taste_it.ViewModels
{
    public class NavigableContentViewModel : ViewModelBase
    {
        public ICommand ChangePageCommand { get; private set; }

        private IPageViewModel currentPageViewModel;
        private List<IPageViewModel> pageViewModels;

        public NavigableContentViewModel(IRecipeDataService recipeData, ITagDataService tagData, ICategoryDataService categoryData)
        {
            Debug.WriteLine("tworze vm");
            PageViewModels.Add(new AddRecipeViewModel(recipeData, tagData, categoryData));
            PageViewModels.Add(new AllRecipesViewModel(recipeData));
            PageViewModels.Add(new FavouriteRecipesViewModel());

            Messenger.Default.Register<NavigationWithCurrentRecipeMessage>(this, this.HandleNavigationMessage);


            CurrentPageViewModel = PageViewModels[1];

            ChangePageCommand = new RelayCommand<IPageViewModel>(p =>  ChangeViewModel(
                (IPageViewModel)p) , 
                (p) => p is IPageViewModel && CurrentPageViewModel.name != p.name );


        }

        private void HandleNavigationMessage(NavigationWithCurrentRecipeMessage msg)
        {
            CurrentPageViewModel = new CurrentRecipeViewModel(msg.CurrentRecipe);
        }

        private void ChangeViewModel(IPageViewModel viewModel)
        {
            if (!PageViewModels.Contains(viewModel))
                PageViewModels.Add(viewModel);

            CurrentPageViewModel = PageViewModels
                .FirstOrDefault(vm => vm == viewModel);
        }

      

        public List<IPageViewModel> PageViewModels
        {
            get
            {
                if (pageViewModels == null)
                    pageViewModels = new List<IPageViewModel>();

                return pageViewModels;
            }
        }

        public IPageViewModel CurrentPageViewModel
        {
            get
            {
                return currentPageViewModel;
            }
            set
            {
                if (currentPageViewModel != value)
                {
                    Set(ref currentPageViewModel, value);
                }
            }
        }

      
    }
}
