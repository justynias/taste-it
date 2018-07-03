using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using taste_it.Additionals.ContentNavigationService;
using taste_it.Additionals.Messages;
using taste_it.Additionals.NavigationService;
using taste_it.DataService;
using taste_it.Models;

namespace taste_it.ViewModels
{
    public class SidebarViewModel : ViewModelBase
    {

        #region fields
        private IPageViewModel currentPageViewModel;
        private List<IPageViewModel> pageViewModels; // Switching between filterview and TASTE.IT view
        private User _currentUser;
        private IFrameNavigationService _navigationService;
        #endregion

        #region properties
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
        public RelayCommand LogOutCommand { get; private set; }

        #endregion
        public SidebarViewModel(ICategoryDataService categoryData, IFrameNavigationService navigationService) 
        {
            this._navigationService = navigationService;
            PageViewModels.Add(new FilterViewModel( categoryData));
            PageViewModels.Add(new TasteItViewModel()); 

            CurrentPageViewModel = PageViewModels[0];
            Messenger.Default.Register<CurrentUserMessage>(this, this.HandleCurrentUserMessage);
            Messenger.Default.Register<SwitchSidebarMessage>(this, this.HandleSwitchSidebarMessage);
            LogOutCommand = new RelayCommand(LogOut);


        }
        #region methods
        private void HandleSwitchSidebarMessage(SwitchSidebarMessage obj)
        {
            Console.WriteLine(obj.PageName);
            if(obj.PageName == "Add Recipe" || obj.PageName == "Current Recipe")
            {
                CurrentPageViewModel = PageViewModels[1];
            }
            else if(obj.PageName == "All Recipes" || obj.PageName == "Favourite Recipes")
            {
                CurrentPageViewModel = PageViewModels[0];
            }
        }

        private void LogOut()
        {
            ViewModelLocator.Cleanup();

            _navigationService.NavigateTo("SignIn");
        }

        private void HandleCurrentUserMessage(CurrentUserMessage message)
        {
            this._currentUser = message.CurrentUser;

        }
        #endregion
    }
}
