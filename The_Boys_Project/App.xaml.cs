using The_Boys_Project.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace The_Boys_Project
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            MainViewModel vm = new MainViewModel();
            // Assign view to start with through SelectedViewModel
            vm.SelectedViewModel = new HomeViewModel();
            // vm.SelectedViewModel = new MailEditorViewModel();
            Views.MainView view = new Views.MainView();
            view.DataContext = vm;
            view.Show();
        }
    }
}
