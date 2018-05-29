using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using taste_it.DataService;
using taste_it.Models;

namespace taste_it.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        
        public ObservableCollection<User> UserCollection { get; set; }
        private readonly IUserDataService _userDataService;
        public ICommand LoadCommand { get; private set; }

        public MainWindowViewModel(IUserDataService userData)
        {
            _userDataService = userData;
            UserCollection = new ObservableCollection<User>();
            
            LoadCommand = new RelayCommand(LoadUsers);
        }

        public async void LoadUsers()
        {
            var users  = await _userDataService.GetUsersAsync();
            UserCollection.Clear();
            foreach (var item in users)
            {
                UserCollection.Add(item);
            }
            RaisePropertyChanged(() => UserCollection);
        }



    }
}
