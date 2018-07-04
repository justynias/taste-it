

using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Diagnostics;
using taste_it.Additionals.NavigationService;
using taste_it.DataService;

namespace taste_it.ViewModels
{
   
    public class ViewModelLocator
    {
      
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            var navigationService = new FrameNavigationService();

            navigationService.Configure("Main", new Uri("../MainWindowView.xaml", UriKind.Relative));
            navigationService.Configure("SignIn", new Uri("../Views/SignInView.xaml", UriKind.Relative));
            navigationService.Configure("SignUp", new Uri("../Views/SignUpView.xaml", UriKind.Relative));
            navigationService.Configure("NavigableContent", new Uri("../Views/NavigableContentView.xaml", UriKind.Relative));
            navigationService.Configure("Sidebar", new Uri("../Views/SidebarView.xaml", UriKind.Relative));
            navigationService.Configure("CurrentRecipe", new Uri("../Views/CurrentRecipeView.xaml", UriKind.Relative));



            SimpleIoc.Default.Register<IFrameNavigationService>(() => navigationService);

            SimpleIoc.Default.Register<IUserDataService, UserDataService>(true);
            SimpleIoc.Default.Register<IRecipeDataService, RecipeDataService>(true);
            SimpleIoc.Default.Register<ITagDataService, TagDataService>(true);
            SimpleIoc.Default.Register<ICategoryDataService, CategoryDataService>(true);
            SimpleIoc.Default.Register<MainWindowViewModel>(true);
            SimpleIoc.Default.Register<SignInViewModel>(true);
            SimpleIoc.Default.Register<SignUpViewModel>(true);
            SimpleIoc.Default.Register<NavigableContentViewModel>(true);
            SimpleIoc.Default.Register<SidebarViewModel>(true);



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
        public NavigableContentViewModel NavigableContent
        {
            get
            {
                return ServiceLocator.Current.GetInstance<NavigableContentViewModel>();
            }
        }
        public SidebarViewModel Sidebar
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SidebarViewModel>();
            }
        }
        public CurrentRecipeViewModel CurrentRecipe
        {
            get
            {
                return ServiceLocator.Current.GetInstance<CurrentRecipeViewModel>();
            }
        }
        public static void Cleanup()
        {

            SimpleIoc.Default.Unregister<FrameNavigationService>();
            SimpleIoc.Default.Unregister<UserDataService>();
            SimpleIoc.Default.Unregister<RecipeDataService>();
            SimpleIoc.Default.Unregister<TagDataService>();
            SimpleIoc.Default.Unregister<CategoryDataService>();
            SimpleIoc.Default.Unregister<MainWindowViewModel>();
            SimpleIoc.Default.Unregister<SignInViewModel>();
            SimpleIoc.Default.Unregister<SignUpViewModel>();
            SimpleIoc.Default.Unregister<NavigableContentViewModel>();
            SimpleIoc.Default.Unregister<SidebarViewModel>();



            SimpleIoc.Default.Register<IUserDataService, UserDataService>(true);
            SimpleIoc.Default.Register<IRecipeDataService, RecipeDataService>(true);
            SimpleIoc.Default.Register<ITagDataService, TagDataService>(true);
            SimpleIoc.Default.Register<ICategoryDataService, CategoryDataService>(true);
            SimpleIoc.Default.Register<MainWindowViewModel>(true);
            SimpleIoc.Default.Register<SignInViewModel>(true);
            SimpleIoc.Default.Register<SignUpViewModel>(true);
            SimpleIoc.Default.Register<NavigableContentViewModel>(true);
            SimpleIoc.Default.Register<SidebarViewModel>(true);

        }
    }
}