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
    public class SignUpViewModel: ViewModelBase
    {
        #region fields
        private string userName;
        private string userPassword;
        private readonly IUserDataService _userDataService;
        private IFrameNavigationService _navigationService;

        #endregion

        #region properties
        public User NewUser { get; set; }
        public ObservableCollection<User> UserCollection { get; set; }
        public ICommand SignUpCommand { get; private set; }
        public ICommand SignInCommand { get; private set; }


        // naviate to sign in (button return or sign up (sign up with command)
        // message with navigation -> usercollection
        // validation, no empty fields, check if user exist, password 
        // ^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$
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

        public SignUpViewModel(IUserDataService userData, IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;
            _userDataService = userData;

            //
            UserCollection = new ObservableCollection<User>();
           // LoadUsers();
            SignUpCommand = new RelayCommand(AddUser);
            SignInCommand = new RelayCommand(SignIn);

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
        private async void AddUser()
        {
            NewUser = new User();
            NewUser.name = UserName;
            NewUser.password = UserPassword;
            //Guid or autoincrement id?
            await _userDataService.AddUserAsync(NewUser);
        }
        public void SignIn()
        {
            //Messenger.Default.Send<CurrentQuizMessage>(new CurrentQuizMessage
            //{
            //    Quiz = CurrentQuiz

            //});

            _navigationService.NavigateTo("SignIn");
        }

    }
}
