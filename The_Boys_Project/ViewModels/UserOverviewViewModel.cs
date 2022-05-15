using Bibliotheek_DAL;
using Bibliotheek_DAL.UnitsOfWork;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Boys_Project.ViewModels
{
    public class UserOverviewViewModel : BaseViewModel
    {
        private IUnitOfWork unitOfWork = new UnitOfWork(new LibraryEntities());
        public MainViewModel MainViewModel { get; set; }

        private ObservableCollection<User> _users;
        private User _selectedUser;
        private string _searchValue = "";
       
        public ObservableCollection<User> Users
        {
            get { return _users; }
            set
            {
                _users = value;
                NotifyPropertyChanged();
            }
        }

        public User SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                _selectedUser = value;
                NotifyPropertyChanged();
            }
        }

        public string SearchValue
        {
            get { return _searchValue; }
            set
            {
                _searchValue = value;
                NotifyPropertyChanged();
            }
        }

        public UserOverviewViewModel(MainViewModel viewModel)
        {
            this.MainViewModel = viewModel;
            Users = new ObservableCollection<User>(unitOfWork.UserRepo.GetEntities(x => x.MembershipType));
            foreach (var user in Users)
            {
                user.CheckMembership();
            }
        }

        public override string this[string columnName]
        {
            get
            {
                return "";
            }
        }

        public override bool CanExecute(object parameter)
        {
            if (parameter.ToString() == "ApplyMembership" || parameter.ToString() == "CheckoutUser" 
                || parameter.ToString() == "UserDetails")
            {
                return SelectedUser != null;
            }
            return true;
        }

        public override void Execute(object parameter)
        {
            if (parameter.ToString() == "ApplyMembership")
            {
                ApplyMembership();
            }
            else if (parameter.ToString() == "CheckoutUser")
            {
                CheckoutUser();
            }
            else if (parameter.ToString() == "Search")
            {
                Search();
            }
            else if (parameter.ToString() == "Reset")
            {
                Reset();
            }
            else if (parameter.ToString() == "UserDetails")
            {
                UserDetails();
            }
        }

        private void UserDetails()
        {
            MainViewModel.SelectedUserToCheckout = SelectedUser;
            MainViewModel.UpdateViewCommand.Execute(parameter: "UserDetails");
        }

        private void ApplyMembership()
        {
            if (SelectedUser.MembershipTypeID == 1)
            {
                SelectedUser.MembershipTypeID = 2;
                unitOfWork.UserRepo.EditEntity(SelectedUser);
                unitOfWork.Save();
                Reset();
            }
        }

        private void CheckoutUser()
        {
            this.MainViewModel.SelectedUserToCheckout = this.SelectedUser;
            this.MainViewModel.UpdateViewCommand.Execute(parameter: "Checkout");
        }
        
        private void Search()
        {
            Users = new ObservableCollection<User>(unitOfWork.UserRepo.GetEntities(
                x => x.Name.Contains(SearchValue) || x.Surname.Contains(SearchValue),
                x => x.MembershipType));
        }

        private void Reset()
        {
            Users = new ObservableCollection<User>(unitOfWork.UserRepo.GetEntities(x => x.MembershipType));
            SearchValue = "";
            SelectedUser = null;
        }
    }
}
