using Bibliotheek_DAL;
using Bibliotheek_DAL.UnitsOfWork;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using The_Boys_Project.models;

namespace The_Boys_Project.ViewModels
{
    public class ProfileViewModel : BaseViewModel
    {
        IUnitOfWork unitOfWork = new UnitOfWork(new LibraryEntities());
        private ObservableCollection<UserItem> _userItems;
        private ObservableCollection<UserBookFair> _userBookFairs;
        private ObservableCollection<string> _countries;
        public MainViewModel MainViewModel { get; set; }

        private bool _firstTab;
        private UserItem _selectedUserItem;
        private UserBookFair _selectedBookFair;
        private bool _hasFine;
        private bool _secondTab;
        private string _errorMessage;
        private User _user;
        private string _password;
        private string _confirmPassword;
        private string _currentPassword;
        private bool _notAdmin;

        public UserBookFair SelectedBookFair
        {
            get { return _selectedBookFair; }
            set
            {
                _selectedBookFair = value;
                NotifyPropertyChanged();
            }
        }

        public UserItem SelectedUserItem
        {
            get { return _selectedUserItem; }
            set
            {
                _selectedUserItem = value;
                NotifyPropertyChanged();
            }
        }

        public bool HasFine
        {
            get { return _hasFine; }
            set
            {
                _hasFine = value;
                NotifyPropertyChanged();
            }
        }

        public bool FirstTab
        {
            get { return _firstTab; }
            set
            {
                _firstTab = value;
                NotifyPropertyChanged();
            }
        }

        public bool SecondTab
        {
            get { return _secondTab; }
            set
            {
                _secondTab = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<UserItem> UserItems
        {
            get { return _userItems; }
            set
            {
                _userItems = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<UserBookFair> UserBookFairs
        {
            get { return _userBookFairs; }
            set 
            { 
                _userBookFairs = value;
                NotifyPropertyChanged();
            }
        }

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                NotifyPropertyChanged();
            }
        }

        public User User
        {
            get { return _user; }
            set { _user = value; }
        }

        public string ConfirmPassword
        {
            get { return _confirmPassword; }
            set
            {
                _confirmPassword = value;
                NotifyPropertyChanged();
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<string> Countries
        {
            get { return _countries; }
            set
            {
                _countries = value;
                NotifyPropertyChanged();
            }
        }

        public string CurrentPassword
        {
            get { return _currentPassword; }
            set { _currentPassword = value; }
        }

        public bool NotAdmin
        {
            get { return _notAdmin; }
            set
            {
                _notAdmin = value;
                NotifyPropertyChanged();
            }
        }

        public ProfileViewModel(User user, MainViewModel mainViewModel = null)
        {
            ErrorMessage = "";
            user.UserFines();
            HasFine = (user.TotalFine <= 0) ? false : true;
            FirstTab = true;
            SecondTab = false;
            this.User = user;
            NotAdmin = (this.User.MembershipTypeID == 3 || this.User.MembershipTypeID == 4) ? false : true;
            this.MainViewModel = mainViewModel;
            Countries = Tools.GetCountries();
            SetUserItems();
            SetUserBookFairs();
        }
        public void SetUserItems()
        {
            UserItems = new ObservableCollection<UserItem>(unitOfWork.UserItemRepo.GetEntities(
                  x => x.Item.ContributorItems.Select(z => z.Contributor),
                  x => x.Item.ItemCategories.Select(z => z.Category),
                  x => x.Item).Where(x => x.UserID == User.UserID));
        }
        public void SetUserBookFairs()
        {
            UserBookFairs = new ObservableCollection<UserBookFair>(unitOfWork.UserBookFairRepo.GetEntities(
                        x => x.BookFair).Where(x => x.UserID == User.UserID));
        }
        public override string this[string columnName]
        {
            get
            {
                return (columnName == nameof(ConfirmPassword) && ConfirmPassword != Password)
                    ? "Wachtwoord en bevestig wachtwoord moeten identiek zijn!" : "";
            }
        }

        public override bool CanExecute(object parameter)
        {
            if (parameter.ToString() == "DeleteAccount")
            {
                return (User.MembershipTypeID == 1) ? true : false;
            }
            else if (parameter.ToString() == "RemoveReservation")
            {
                return (SelectedUserItem == null) ? false : true;
            }
            else if (parameter.ToString() == "RemoveBookFairRegistration")
            {
                return (SelectedBookFair == null) ? false : true;
            }
            else return true;
        }

        public override void Execute(object parameter)
        {
            if (parameter.ToString() == "SaveChanges")
            {
                EditUser(this.User);
            }
            if (parameter.ToString() == "DeleteAccount")
            {
                DeleteUser(this.User);
            }
            if (parameter.ToString() == "RemoveReservation")
            {
                RemoveReservation();
            }
            if (parameter.ToString() == "RemoveBookFairRegistration")
            {
                RemoveBookFairRegistration();
            }

        }

        private void RemoveBookFairRegistration()
        {
            unitOfWork.UserBookFairRepo.DeleteEntity(SelectedBookFair);
            int ok = unitOfWork.Save();
            ErrorMessage = (ok >= 1) ? "Je registratie is verwijderd"
            : "Je registratie is niet verwijderd, probeer later nog eens";
            SelectedBookFair = null;
            SetUserBookFairs();
        }

        private void RemoveReservation()
        {
            unitOfWork.UserItemRepo.DeleteEntity(SelectedUserItem);
            int ok = unitOfWork.Save();
            ErrorMessage = (ok >= 1) ? "Je reservatie is verwijderd"
                : "Je reservatie is niet verwijderd, probeer later nog eens";
            SelectedUserItem = null;
        }
        public void DeleteUser(User user)
        {
            ObservableCollection<UserItem> BorrowedUserItems = new ObservableCollection<UserItem>(unitOfWork.UserItemRepo.GetEntities().Where(x => x.BorrowedDate != null).Where(x => x.ReturnedDate == null).Where(x => x.UserID == user.UserID));
            string foutmelding = "";
            foutmelding = (BorrowedUserItems.Count > 0) ? "Een gebruiker met uitgeleende items kan niet verwijderd worden\n" : "";
            foutmelding += (user.TotalFine > 0) ? "Een account kan niet verwijderd worden met een openstaande boete" : "";
            if (foutmelding == "")
            {
                ObservableCollection<Fine> UserFines = new ObservableCollection<Fine>(unitOfWork.FineRepo.GetEntities().Where(x => x.UserID == user.UserID));
                ObservableCollection<UserInterestedItem> UserInterestedItems = new ObservableCollection<UserInterestedItem>(unitOfWork.UserInterestedItemRepo.GetEntities().Where(x => x.UserID == user.UserID));              
                if (UserFines.Count > 0)
                {
                    foreach (Fine fine in UserFines)
                    {
                        // changing id to the placeholder account 
                        fine.UserID = 32;
                        unitOfWork.FineRepo.EditEntity(fine);
                    }
                }
                if (UserInterestedItems.Count > 0)
                {
                    foreach (UserInterestedItem userInterestedItem in UserInterestedItems)
                    {
                        // changing id to the placeholder account 
                        userInterestedItem.UserID = 32;
                        unitOfWork.UserInterestedItemRepo.EditEntity(userInterestedItem);
                    }
                }
                if (UserBookFairs.Count > 0)
                {

                    foreach (UserBookFair userBookFair in UserBookFairs)
                    {
                        // changing id to the placeholder account 
                        userBookFair.UserID = 32;
                        unitOfWork.UserBookFairRepo.EditEntity(userBookFair);
                    }
                }
                if (UserItems.Count > 0)
                {

                    foreach (UserItem userItem in UserItems)
                    {
                        // changing id to the placeholder account 
                        userItem.UserID = 32;
                        unitOfWork.UserItemRepo.EditEntity(userItem);
                    }
                }
                unitOfWork.UserRepo.DeleteEntity(user);
                int ok = unitOfWork.Save();
                Tools.SendMail(User, "AccountDeleted", "Account verwijderd.");
                MainViewModel.User = null;
                MainViewModel.UpdateViewCommand.Execute(parameter: "Login");
                if (ok < 1)
                {
                    ErrorMessage = "Je account is niet verwijderd, probeer later nog eens";
                }
            }
            else ErrorMessage = foutmelding;
        }
        public void EditUser(User user)
        {
            // check if passwords has been filled in if not validation for changing of password is not necessary
            string error = "";
            string initialPassword = Tools.Decrypt(user.Password);
            if (this.Password != null || this.CurrentPassword != null || this.ConfirmPassword != null)
            {
                error += (this.Password == null) ? "Het veld nieuw wachtwoord mag niet leeg zijn" : "";
                error += (this.CurrentPassword == null) ? "Het veld huidig wachtwoord mag niet leeg zijn" : "";
                if (CurrentPassword == Tools.Decrypt(user.Password))
                {
                    User.Password = this.Password;
                    error += (this.Error != "") ? this.Error : "";
                }
                else error = "Het veld huidig wachtwoord komt niet overeen met het huidig wachtwoord van de gebruiker";
            }
            if (user.IsGeldig() && error == "")
            {
                user.Password = Tools.Encrypt(user.Password);
                unitOfWork.UserRepo.EditEntity(user);
                int ok = unitOfWork.Save();
                ErrorMessage = (ok >= 1) ? "Je account is aangepast"
                    : "Je account is niet aangepast, probeer later nog eens";
                ResetPasswordBoxes();
            }
            else
            {
                ErrorMessage = user.Error + this.Error + "" + error;
                user.Password = Tools.Encrypt(initialPassword);
            }
        }

        private void ResetPasswordBoxes()
        {
            this.Password = null;
            this.ConfirmPassword = null;
            this.CurrentPassword = null;
        }
    }
}
