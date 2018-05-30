using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using taste_it.Additionals.NavigationService;
using taste_it.DataService;
using taste_it.Models;

namespace taste_it.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {

        #region Fields
        IFrameNavigationService navigationService;

        #endregion
        #region Constructor
        public MainWindowViewModel(IFrameNavigationService navigationService)
        {
            this.navigationService = navigationService;
        }
        #endregion



    }
}
