using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using taste_it.Additionals.ContentNavigationService;

namespace taste_it.ViewModels
{
    public class SidebarViewModel : ViewModelBase
    {
        private string message; // Message shown to user with his/her name e.g Hello root
        private IPageViewModel currentPageViewModel;
        private List<IPageViewModel> pageViewModels; // Switching between filterview and TASTE.IT view
        public SidebarViewModel() // Here we have to pass user probably from NavigableContentViewModel
        {

        }
    }
}
