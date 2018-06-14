using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        private bool canSave;
        private readonly IUserDataService _userDataService;
        private IFrameNavigationService _navigationService;

        #endregion

        #region properties
        public User NewUser { get; set; }
        public ObservableCollection<User> UserCollection { get; set; }
        public ICommand SignUpCommand { get; private set; }
        public ICommand NavigateToSignInViewCommand { get; private set; }
        public ICommand LoadUsersCommand { get; private set; }


        // naviate to sign in (button return or sign up (sign up with command)
        // message with navigation -> usercollection
        // validation, no empty fields, check if user exist, password 
        // ^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$
        private bool CanSave
        {
            get { return canSave; }
            set
            {
                Set(ref canSave, value);
            }
        }
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
            SignUpCommand = new RelayCommand(SignUp); //unable button
            LoadUsersCommand = new RelayCommand(LoadUsers); //unable button
            NavigateToSignInViewCommand = new RelayCommand(NavigateToSignInView);


        }

       
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
        private async void SignUp()
        {
            NewUser = new User() { name = this.UserName, password = this.HashPassword(UserPassword)};
            UserCollection.Add(NewUser);
            await _userDataService.AddUserAsync(NewUser);
        }


        public void NavigateToSignInView()
        {

            _navigationService.NavigateTo("SignIn");
        }

        private bool IsRepeatdPasswordValid()
        {
            if(UserPassword!=null && UserPasswordRepeated!=null) Console.WriteLine(UserPassword, UserPasswordRepeated);
            return UserPasswordRepeated != UserPassword;
        }
        private bool UserExists()
        {
            //message with collection
             return UserCollection.Any(u => u.name == UserName);
            //return false;
        }

        public string this[string columnName]
        {
            get
            {
                string result = string.Empty;
                if (columnName == "UserPassword")
                {
                    if (!HasPasswordProperFormat(UserPassword, out result)) return result;
                }
                else if (columnName == "UserPasswordRepeated")
                {
                    if (IsRepeatdPasswordValid())
                    {
                        CanSave = false;
                        return "Passwords are not the same";

                    }
                }
                else if (columnName == "UserName")
                {
                    if (UserExists()) return "User name already exists";
                }
               
                return result;
            }
        }

        public string Error
        {
            get { return null; }
          // get { return this.canSave ? string.Empty : string.Empty; }
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
            var hasMiniMaxChars = new Regex(@".{8,15}");
            var hasLowerChar = new Regex(@"[a-z]+");
            var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

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
            else if (!hasMiniMaxChars.IsMatch(input))
            {
                ErrorMessage = "Password should not be less than or greater than 12 characters";
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
    }
}
