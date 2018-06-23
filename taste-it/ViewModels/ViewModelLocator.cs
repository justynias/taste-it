<<<<<<< refs/remotes/origin/feature-addrecipe
/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:taste_it"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using System;
using taste_it.Additionals.NavigationService;
using taste_it.DataService;

namespace taste_it.ViewModels
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}
            var navigationService = new FrameNavigationService();

            navigationService.Configure("Main", new Uri("../MainWindowView.xaml", UriKind.Relative));
            navigationService.Configure("SignIn", new Uri("../Views/SignInView.xaml", UriKind.Relative));
            navigationService.Configure("SignUp", new Uri("../Views/SignUpView.xaml", UriKind.Relative));
            navigationService.Configure("AddRecipe", new Uri("../Views/AddRecipeView.xaml", UriKind.Relative));



            SimpleIoc.Default.Register<IFrameNavigationService>(() => navigationService);
       
            SimpleIoc.Default.Register<IUserDataService, UserDataService>(true);
            SimpleIoc.Default.Register<IRecipeDataService, RecipeDataService>(true);
            SimpleIoc.Default.Register<ITagDataService, TagDataService>(true);
            SimpleIoc.Default.Register<ICategoryDataService, CategoryDataService>(true);
            SimpleIoc.Default.Register<MainWindowViewModel>(true);
            SimpleIoc.Default.Register<SignInViewModel>(true);
            SimpleIoc.Default.Register<SignUpViewModel>(true);
            SimpleIoc.Default.Register<AddRecipeViewModel>(true);



        }

        public MainWindowViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainWindowViewModel>();
            }
        }
        public SignInViewModel SignIn
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SignInViewModel>();
            }
        }

        public SignUpViewModel SignUp
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SignUpViewModel>();
            }
        }

        public AddRecipeViewModel AddRecipe
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AddRecipeViewModel>();
            }
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
=======
/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:taste_it"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using System;
using taste_it.Additionals.NavigationService;
using taste_it.DataService;

namespace taste_it.ViewModels
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}
            var navigationService = new FrameNavigationService();

            navigationService.Configure("Main", new Uri("../MainWindowView.xaml", UriKind.Relative));
            navigationService.Configure("SignIn", new Uri("../Views/SignInView.xaml", UriKind.Relative));
            navigationService.Configure("SignUp", new Uri("../Views/SignUpView.xaml", UriKind.Relative));


            SimpleIoc.Default.Register<IFrameNavigationService>(() => navigationService);
       
            SimpleIoc.Default.Register<IUserDataService, UserDataService>(true);
            SimpleIoc.Default.Register<IRecipeDataService, RecipeDataService>(true);
            SimpleIoc.Default.Register<MainWindowViewModel>(true);
            SimpleIoc.Default.Register<SignInViewModel>(true);
            SimpleIoc.Default.Register<SignUpViewModel>(true);


        }

        public MainWindowViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainWindowViewModel>();
            }
        }
        public SignInViewModel SignIn
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SignInViewModel>();
            }
        }

        public SignUpViewModel SignUp
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SignUpViewModel>();
            }
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
>>>>>>> HEAD~11
}