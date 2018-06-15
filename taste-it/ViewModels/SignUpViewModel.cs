using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using taste_it.Additionals.NavigationService;
using taste_it.DataService;
using taste_it.Models;

namespace taste_it.ViewModels
{
    public class SignUpViewModel: ViewModelBase, IDataErrorInfo
    {
        #region fields
        private string userName;
        private string userPassword;
        private string userPasswordRepeated;
        private readonly IUserDataService _userDataService;
        private IFrameNavigationService _navigationService;


        #endregion

        #region properties
        public User NewUser { get; set; }
        public ObservableCollection<User> UserCollection { get; set; }
        public ICommand SignUpCommand { get; private set; }
        public ICommand NavigateToSignInViewCommand { get; private set; }
        public ICommand LoadUsersCommand { get; private set; }

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
        public string UserPasswordRepeated
        {
            get
            {
                return userPasswordRepeated;
            }

            set
            {

                Set(ref userPasswordRepeated, value);

            }

        }

        #endregion

        public SignUpViewModel(IUserDataService userData, IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;
            _userDataService = userData;

            UserCollection = new ObservableCollection<User>();
            SignUpCommand = new RelayCommand(SignUp, true); 
            LoadUsersCommand = new RelayCommand(LoadUsers); 
            NavigateToSignInViewCommand = new RelayCommand(NavigateToSignInView);


        }

        #region methods

        private string HashPassword(string password)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            return Convert.ToBase64String(hashBytes);


        }

        private bool CanSave()
        {
            string value;
            bool result = (HasPasswordProperFormat(UserPassword, out value) && HasUserNameProperFormat(UserName, out value) && IsRepeatdPasswordCorrect());
            return result;
        }
        private async void LoadUsers()
        {
           // System.Threading.Thread.Sleep(5000); //sleep for testing 
            var users =  await _userDataService.GetUsersAsync();
            UserCollection.Clear();
            foreach (var item in users)
            {
                UserCollection.Add(item);
            }
            RaisePropertyChanged(() => UserCollection);
        }
        //private async void SignUp()
        private async void SignUp()
        { 

            if (CanSave())
            {
                NewUser = new User() { name = this.UserName, password = this.HashPassword(UserPassword) };
                UserCollection.Add(NewUser);
                await _userDataService.AddUserAsync(NewUser);
                Debug.WriteLine("sign up");   //to test only
            }
            else
            {
                Debug.WriteLine("no sign up");   //to test only
            }
        }

        private void NavigateToSignInView()
        {

            _navigationService.NavigateTo("SignIn");
        }
        #endregion

        #region validation methods
        private bool IsRepeatdPasswordCorrect()
        {
            return UserPasswordRepeated == UserPassword;
        }
        private bool HasUserNameProperFormat(string name, out string ErrorMessage)
        {
            ErrorMessage = string.Empty;
            if ((string.IsNullOrWhiteSpace(name) || string.IsNullOrEmpty(name)))
            {
                Debug.WriteLine("empty");
                Debug.WriteLine(name);
                ErrorMessage = "User name should not be empty";
                return false;

            }
            else if (UserCollection.Any(u => u.name == UserName))
            {
                ErrorMessage = "User name already exists";
                return false;
            }
            else if (name.Length>20)
            {
                ErrorMessage = "User name should not be greater than 20 characters";
                return false;
            }
            else return true;

        }
        private bool HasPasswordProperFormat(string password, out string ErrorMessage)
        {
            var input = password;
            ErrorMessage = string.Empty;
            Console.WriteLine(UserPassword);

            if (string.IsNullOrWhiteSpace(input))
            {
                ErrorMessage = "Password should not be empty";
                return false;
            }
            
            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasLowerChar = new Regex(@"[a-z]+");
            var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

            if(!(password.Length>=8 && password.Length<=12))
            {
                ErrorMessage = "Password should not be less than 8 or greater than 12 characters";
                return false;
            }
            if (!hasLowerChar.IsMatch(input))
            {
                ErrorMessage = "Password should contain At least one lower case letter";
                return false;
            }
            else if (!hasUpperChar.IsMatch(input))
            {
                ErrorMessage = "Password should contain At least one upper case letter";
                return false;
            }

            else if (!hasNumber.IsMatch(input))
            {
                ErrorMessage = "Password should contain At least one numeric value";
                return false;
            }

            else if (!hasSymbols.IsMatch(input))
            {
                ErrorMessage = "Password should contain At least one special case characters";
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion

        #region IDataErrorInfo Members
        public string this[string columnName]
        {
            get
            {
                string result = string.Empty;
                if (columnName == "UserPassword")
                {
                    if (!HasPasswordProperFormat(UserPassword, out result)) { return result;  }
                    
                }
                else if (columnName == "UserPasswordRepeated")
                {
                    if (!IsRepeatdPasswordCorrect()) {  return "Passwords are not the same"; }
                   
                }
                else if (columnName == "UserName")
                {
                    if (!HasUserNameProperFormat(UserName, out result)) { return result; }
                    return result;
                }
                return result;
            }
        }

        public string Error
        {
            get { return null; }
        }


        #endregion

    }
}
