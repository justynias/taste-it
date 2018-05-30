using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using taste_it.Additionals.NavigationService;
using taste_it.ViewModels;

namespace taste_it
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            IFrameNavigationService navigationService = new FrameNavigationService();
            MainWindow app = new MainWindow();
            MainWindowViewModel context = new MainWindowViewModel(navigationService);
            app.DataContext = context;
            app.Show();
        }
    }
}
