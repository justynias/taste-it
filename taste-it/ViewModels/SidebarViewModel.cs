using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
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
    public class SidebarViewModel : ViewModelBase
    {
        private string message; // Message shown to user with his/her name e.g Hello root
        private IPageViewModel currentPageViewModel;
        private List<IPageViewModel> pageViewModels; // Switching between filterview and TASTE.IT view
        private User _currentUser;


        public string Message
        {
            get
            {
                if(_currentUser != null)
                    return "Hello "+ _currentUser.name;
                return "Hello null user";
            }
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
        //temporary data services
        public SidebarViewModel(IRecipeDataService recipeData, ITagDataService tagData, ICategoryDataService categoryData) // Here we have to pass user probably from NavigableContentViewModel
        {
            PageViewModels.Add(new FilterViewModel(recipeData, tagData, categoryData));
            PageViewModels.Add(new TasteItViewModel()); 

            CurrentPageViewModel = PageViewModels[0];
            Messenger.Default.Register<CurrentUserMessage>(this, this.HandleCurrentUserMessage);

        }

        private void HandleCurrentUserMessage(CurrentUserMessage message)
        {
            this._currentUser = message.CurrentUser;
            Debug.WriteLine(_currentUser.name);

        }
    }
}
