using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
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
        private bool isBusy;

        #endregion
        #region Constructor
        public MainWindowViewModel(IFrameNavigationService navigationService)
        {
            this.navigationService = navigationService;
            Messenger.Default.Register<GenericMessage<bool>>(this, SetLoading);
           
        }


        #endregion
        #region Properties
        public bool IsBusy { get => isBusy; set => Set(ref isBusy, value); }
        #endregion
        #region Private Methods
        private void SetLoading(GenericMessage<bool> message)
        {
            IsBusy = message.Content;
            Console.WriteLine(message.Content.ToString());
        }
        #endregion

    }
}
