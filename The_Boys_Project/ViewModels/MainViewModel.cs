using The_Boys_Project.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Bibliotheek_DAL;
using System.Windows;
using The_Boys_Project.models;

namespace The_Boys_Project.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private User _user;
        private bool _hasLoggedInUser;
        private bool _userIsAdmin;
        private bool _userIsMainAdmin;


        public User User
        {
            get { return _user; }
            set
            {
                _user = value;
                NotifyPropertyChanged();

                SetUserBools();
            }
        }


        public bool HasLoggedInUser
        {
            get { return _hasLoggedInUser; }
            set 
            { 
                _hasLoggedInUser = value;
                NotifyPropertyChanged();
            }
        }

        public bool UserIsAdmin
        {
            get { return _userIsAdmin; }
            set 
            { 
                _userIsAdmin = value;
                NotifyPropertyChanged();
            }
        }

        public bool UserIsMainAdmin
        {
            get { return _userIsMainAdmin; }
            set
            {
                _userIsMainAdmin = value;
                NotifyPropertyChanged();
            }
        }



        public Item SelectedItemToEdit { get;set; }
        public BookFair SelectedBookFairToEdit { get; set; }
        public User SelectedUserToCheckout { get; set; }
        public string FineSearchValue { get; set; } = "";

        #region Content Control

        // Properties required to keep track of which ViewModel to display
        private BaseViewModel _selectedViewModel;

        public BaseViewModel SelectedViewModel
        {
            get { return _selectedViewModel; }
            set
            {
                _selectedViewModel = value;
                // Important to let the system know we're switching views!
                NotifyPropertyChanged();
            }
        }

        // Located in Commands folder. Used to change SelectedViewModel
        public ICommand UpdateViewCommand { get; set; }

        #endregion

        public MainViewModel()
        {
            // Instantiate the UpdateViewCommand to enable setting SelectedViewModel through binding
            UpdateViewCommand = new UpdateViewCommand(this);

            

        }

        public override string this[string columnName]
        {
            get { return ""; }
        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
        }

        private void SetUserBools()
        {
            HasLoggedInUser = User != null;
            if (HasLoggedInUser)
            {
                UserIsAdmin = User.MembershipType.Description == "Admin" || User.MembershipType.Description == "Hoofdadmin";
                UserIsMainAdmin = User.MembershipType.Description == "Hoofdadmin";
            }
            else
            {
                UserIsAdmin = false;
                UserIsMainAdmin = false;
            }
        }
    }
}
