using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using taste_it.Additionals.NavigationService;
using taste_it.DataService;
using taste_it.Models;

namespace taste_it.ViewModels
{
    public class SignInViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        #region fields

        //private User currentUser;
        private string userName;
        private string userPassword;
        private readonly IUserDataService _userDataService;
        private IFrameNavigationService _navigationService;

        #endregion

        #region properties
        public User CurrentUser {get;set;}
        public ObservableCollection<User> UserCollection { get; set; }
        public ICommand SignInCommand { get; private set; }
        public string UserName
        {
            get
            {
                return userName;
            }

            set
            {
                
                Set(ref userName, value);
            }
        }
        public string UserPassword
        {
            get
            {
                return userPassword;
            }

            set
            {

                Set(ref userPassword, value);
            }
        }

        #endregion 

        
        
        public SignInViewModel(IUserDataService userData, IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;
            _userDataService = userData;
            UserCollection = new ObservableCollection<User>();
            LoadUsers();

            SignInCommand = new RelayCommand(SignIn);
        }


        #region methods
        private async void LoadUsers()
        {
            var users = await _userDataService.GetUsersAsync();
            UserCollection.Clear();
            foreach (var item in users)
           {
                UserCollection.Add(item);
            }
            RaisePropertyChanged(() => UserCollection);
        }

        public void SignIn()   // Method execute after button clicked
        {

            CheckCredentials();
            //if(ChechCreditionals -> NavigateToNextView  // after logged in display another view
        }
        private bool CheckCredentials()
        {
            HasErrors = !Verification();
            if (HasErrors)
            {
                ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs("UserName"));
                ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs("UserPassword"));
            }
            else
            {
                return true;
            }
            return false;
        }

        private bool Verification()   
        {           
            foreach (var u in UserCollection)
            {
                if (UserName == u.name && UserPassword == u.password)
                {
                    CurrentUser = u;
                    Console.WriteLine("logged in");
                    return true;
                }
            }
            UserName = null;
            UserPassword = null;
            return false;
        }

        #endregion

        //Implementation of INotifyDataErrorInfo, to verify data after button is clicked
        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName) || (!HasErrors))
                return null;
            return new List<string>() { "Invalid credentials" };
        }
        public bool HasErrors { get; set; } = false;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

    }
}

