using The_Boys_Project.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Bibliotheek_DAL;
using The_Boys_Project.Views;

namespace The_Boys_Project.Commands
{
  public class UpdateViewCommand : ICommand
  {
    private MainViewModel viewModel;

    public UpdateViewCommand(MainViewModel viewModel)
    {
      this.viewModel = viewModel;
    }

    public event EventHandler CanExecuteChanged
    {
      add { CommandManager.RequerySuggested += value; }
      remove { CommandManager.RequerySuggested -= value; }
    }

    public bool CanExecute(object parameter)
    {
      return true;
    }
        public void Execute(object parameter)
        {
            // for Views that can have a logged in user, pass them into the new ViewModel here by passing the MainView's user
            if (parameter.ToString() == "Home")
            {
                viewModel.SelectedViewModel = new HomeViewModel(this.viewModel, this.viewModel.User);
            }
            else if (parameter.ToString() == "Login")
            {
                viewModel.SelectedViewModel = new LoginViewModel(this.viewModel);
            }
            else if (parameter.ToString() == "Toevoegen")
            {
                viewModel.SelectedViewModel = new CUDItemViewModel(this.viewModel, null);
            }
            else if (parameter.ToString() == "Aanpassen")
            {
                viewModel.SelectedViewModel = new CUDItemViewModel(this.viewModel, viewModel.SelectedItemToEdit);
            }
            else if (parameter.ToString() == "Registreren")
            {
                viewModel.SelectedViewModel = new RegistrerenViewModel(this.viewModel, false);
            }
            else if (parameter.ToString() == "CreateAdmin")
            {
                viewModel.SelectedViewModel = new RegistrerenViewModel(this.viewModel, true);
            }
            else if (parameter.ToString() == "Logout")
            {
                this.viewModel.User = null;
                viewModel.SelectedViewModel = new HomeViewModel(this.viewModel);
            }
            else if (parameter.ToString() == "Uitleenhistoriek")
            {
                viewModel.SelectedViewModel = new UitleenhistoriekViewModel(this.viewModel.SelectedItemToEdit);
            }
            else if (parameter.ToString() == "BookFair")
            {
                viewModel.SelectedViewModel = new BookFairViewModel(this.viewModel, this.viewModel.User);
            }
            else if (parameter.ToString() == "AddBookFair")
            {
                viewModel.SelectedViewModel = new CUDBookFairViewModel(this.viewModel, null);
            }
            else if (parameter.ToString() == "EditBookFair")
            {
                viewModel.SelectedViewModel = new CUDBookFairViewModel(this.viewModel, viewModel.SelectedBookFairToEdit);
            }
            else if (parameter.ToString() == "Fines")
            {
                viewModel.SelectedViewModel = new FinesViewModel(this.viewModel, this.viewModel.FineSearchValue);
            }
            else if (parameter.ToString() == "Profile")
            {
                viewModel.SelectedViewModel = new ProfileViewModel(this.viewModel.User, this.viewModel);
            }
            else if (parameter.ToString() == "DetailBookFair")
            {
                viewModel.SelectedViewModel = new BookFairDetailViewModel(this.viewModel, viewModel.SelectedBookFairToEdit);
            }
            else if (parameter.ToString() == "AuteurCategorie")
            {
                viewModel.SelectedViewModel = new AuthorCategoryViewModel(this.viewModel);
            }
            else if (parameter.ToString() == "Checkout")
            {
                viewModel.SelectedViewModel = new UserItemControlViewModel(this.viewModel, this.viewModel.SelectedUserToCheckout);
            }
            else if (parameter.ToString() == "UserOverview")
            {
                viewModel.SelectedViewModel = new UserOverviewViewModel(this.viewModel);
            }
            else if (parameter.ToString() == "AdminOperations")
            {
                viewModel.SelectedViewModel = new AdminOperationsViewModel(this.viewModel);
            }
            else if (parameter.ToString() == "AdminUserDetails")
            {
              viewModel.SelectedViewModel = new AdminUserDetailsViewModel(this.viewModel.User, this.viewModel);
            }
            else if (parameter.ToString() == "UserDetails")
            {
                viewModel.SelectedViewModel = new AdminUserDetailsViewModel(this.viewModel.SelectedUserToCheckout, this.viewModel);
            }
            else if (parameter.ToString() == "MailEditor")
            {
                var vm = new MailEditorWindowView();
                vm.DataContext = new MailEditorWindowViewModel();
                vm.ShowDialog();
            }

        }
    } 
}
