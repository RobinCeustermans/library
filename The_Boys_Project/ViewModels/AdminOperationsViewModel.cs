using Bibliotheek_DAL;
using Bibliotheek_DAL.UnitsOfWork;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace The_Boys_Project.ViewModels
{
    public class AdminOperationsViewModel : BaseViewModel
    {
        IUnitOfWork unitOfWork = new UnitOfWork(new LibraryEntities());
        public MainViewModel MainViewModel { get; set; }
        private ObservableCollection<User> _users;
        private User _userToEdit;    
        private string _searchTermUser;
        private string _errorMessageSearch;
        private string _errorMessage;
        private string _successMessage;
        private bool _errorMessageLabelIsVisible;
        private bool _successMessageLabelIsVisible;
        public AdminOperationsViewModel(MainViewModel mainViewModel)
        {
            this.MainViewModel = mainViewModel;
            ToggleVisibilityLabels(true);
            ResetSearch();         
        }

        public ObservableCollection<User> Users
        {
            get { return _users; }
            set
            {
                _users = value;
                NotifyPropertyChanged();
            }
        }
        public string SearchTermUser
        {
            get { return _searchTermUser; }
            set
            {
                _searchTermUser = value;
                NotifyPropertyChanged();
            }
        }

        public string ErrorMessageSearch
        {
            get { return _errorMessageSearch; }
            set
            {
                _errorMessageSearch = value;
                NotifyPropertyChanged();
            }
        }

        public void SearchUser()
        {
            Users = new ObservableCollection<User>
                (unitOfWork.UserRepo.GetEntities
                (x => (x.Name.Contains(SearchTermUser) || x.Surname.Contains(SearchTermUser)) && x.MembershipType.Description == "Admin"));
            if (Users.Count() == 0)
                ErrorMessageSearch = "Er zijn geen gebruikers gevonden die overeenkomen met de zoekopdracht";
            else
            {
                UserToEdit = null;
                ClearSearchFields();
            }
        }

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                ToggleVisibilityLabels(false);
                NotifyPropertyChanged();
            }
        }

        public string SuccessMessage
        {
            get { return _successMessage; }
            set
            {
                _successMessage = value;
                ToggleVisibilityLabels(true);
                NotifyPropertyChanged();
            }
        }

        public bool ErrorMessageLabelIsVisible
        {
            get { return _errorMessageLabelIsVisible; }
            set
            {
                _errorMessageLabelIsVisible = value;
                NotifyPropertyChanged();
            }
        }

        public bool SuccessMessageLabelIsVisible
        {
            get { return _successMessageLabelIsVisible; }
            set
            {
                _successMessageLabelIsVisible = value;
                NotifyPropertyChanged();
            }
        }

        public User UserToEdit
        {
            get { return _userToEdit; }
            set
            {
                _userToEdit = value;
                NotifyPropertyChanged();
                ClearFields();              
            }
        }

       public void DeactivateAdmin()
        {
            if (UserToEdit != null)
            {
                UserToEdit.IsActive = false;
                unitOfWork.UserRepo.EditEntity(UserToEdit);
                int ok = unitOfWork.Save();
                if (ok > 0)
                {
                    SuccessMessage = "De admin is nu inactief";
                    UserToEdit = null;
                }
                else ErrorMessage = "Het is niet gelukt om de admin op inactief te zetten";
            }
        }

        public void ActivateAdmin()
        {
            if (UserToEdit != null)
            {
                UserToEdit.IsActive = true;
                unitOfWork.UserRepo.EditEntity(UserToEdit);
                int ok = unitOfWork.Save();
                if (ok > 0)
                {
                    SuccessMessage = "De admin is nu actief";
                    UserToEdit = null;
                }
                else ErrorMessage = "Het is niet gelukt om de admin te activeren";
            }
        }

        public void DeleteAdmin()
        {
            if (UserToEdit != null)
            {
                DialogResult dialogResult = System.Windows.Forms.MessageBox.Show(
                        "Weet u zeker dat u deze admin wilt verwijderen? Deze kan niet meer hersteld worden",
                        "Bevestiging verwijderen", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    unitOfWork.UserRepo.DeleteEntity(UserToEdit);
                    int ok = unitOfWork.Save();
                    if (ok > 0)
                    {
                        Users = new ObservableCollection<User>(unitOfWork.UserRepo.GetEntities(x => x.MembershipType).Where(y => y.MembershipType.Description == "Admin"));
                        SuccessMessage = "De Admin is verwijderd";
                    }
                    else ErrorMessage = "De Admin is niet verwijderd";
                }
            }
        }

        public void ResetSearch()
        {
            UserToEdit = null;
            SuccessMessage = "";
            Users = new ObservableCollection<User>(unitOfWork.UserRepo.GetEntities(x => x.MembershipType).Where(y => y.MembershipType.Description == "Admin"));
            ClearSearchFields();
        }

        public void ClearFields()
        {
            if (UserToEdit != null)
            {
                ClearSearchFields();
                ErrorMessage = "";
                ToggleVisibilityLabels(true);
                SuccessMessage = "";                        
            }
        }

        public void ClearSearchFields()
        {
            ErrorMessageSearch = "";
            SearchTermUser = "";
        }

        public void ToggleVisibilityLabels(bool success)
        {
            if (success)
            {
                SuccessMessageLabelIsVisible = true;               
                ErrorMessageLabelIsVisible = false;
            }
            else
            {
                SuccessMessageLabelIsVisible = false;
                ErrorMessageLabelIsVisible = true;
            }
        }

        public override string this[string columnName]
        {
            get
            {
                return "";
            }
        }

        private void AddAdmin()
        {
            MainViewModel.UpdateViewCommand.Execute(parameter: "CreateAdmin");
        }
        public override bool CanExecute(object parameter)
        {
            if (parameter.ToString() == "DeactivateAdmin")
            {        
                return (UserToEdit != null) ? UserToEdit.IsActive ? true : false : false;
            }
            else if (parameter.ToString() == "ActivateAdmin")
            {
                return (UserToEdit != null) ? UserToEdit.IsActive ? false : true : false;
            }
            else if (parameter.ToString() == "DeleteAdmin")
            {
                return UserToEdit != null ? true : false;
            }
            return true;
        }

        public override void Execute(object parameter)
        {
            if (parameter.ToString() == "SearchUser")
            {
                SearchUser();
            }
            else if (parameter.ToString() == "ResetSearch")
            {
                ResetSearch();
            }
            else if (parameter.ToString() == "DeactivateAdmin")
            {
                DeactivateAdmin();
            }
            else if (parameter.ToString() == "ActivateAdmin")
            {
                ActivateAdmin();
            }
            else if (parameter.ToString() == "DeleteAdmin")
            {
                DeleteAdmin();
            }
            else if (parameter.ToString() == "AddAdmin")
            {
                AddAdmin();
            }

        }
    }
}
