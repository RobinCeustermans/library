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
    public class FinesViewModel: BaseViewModel
    {
        UnitOfWork unitOfWork = new UnitOfWork(new LibraryEntities());

        private User _selectedUser;
        private Fine _selectedFine;
        private ObservableCollection<User> _users;
        private ObservableCollection<Fine> _fines;
        private bool _perUserTabToggled;
        private bool _perFineTabToggled;
        private string _searchValue;

        public User SelectedUser 
        {
            get { return _selectedUser; }
            set
            {
                _selectedUser = value;
                NotifyPropertyChanged();
            }
        }

        public Fine SelectedFine
        {
            get { return _selectedFine; }
            set
            {
                _selectedFine = value;
                NotifyPropertyChanged();
            }
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

        public ObservableCollection<Fine> Fines
        {
            get { return _fines; }
            set
            {
                _fines = value;
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

        public bool PerUserTabToggled
        {
            get { return _perUserTabToggled; }
            set
            {
                _perUserTabToggled = value;
                NotifyPropertyChanged();
                Reset();
            }
        }
        public bool PerFineTabToggled
        {
            get { return _perFineTabToggled; }
            set
            {
                _perFineTabToggled = value;
                NotifyPropertyChanged();
                Reset();
            }
        }

        public FinesViewModel(MainViewModel viewModel, string searchValue = "")
        {
            PerUserTabToggled = true;
            SearchValue = searchValue;

            SetFines();
            if (SearchValue != "")
            {
                viewModel.FineSearchValue = "";
                Search();
            }
        }

        public override string this[string columnName]
        {
            get { return ""; }
        }

        public override bool CanExecute(object parameter)
        {
            if (parameter.ToString() == "ConfirmPaid")
            {
                return SelectedUser != null || SelectedFine != null;
            }
            return true;
        }

        public override void Execute(object parameter)
        {
            if (parameter.ToString() == "ConfirmPaid")
            {
                ConfirmPaid(SelectedUser);
            }
            else if (parameter.ToString() == "Search")
            {
                Search();
            }
            else if (parameter.ToString() == "Reset")
            {
                Reset();
            }
        }

        private void ConfirmPaid(User user)
        {
            if (PerUserTabToggled)
            {
                List<Fine> userFines = unitOfWork.FineRepo.GetEntities(x => x.UserID == user.UserID && !x.IsPaid).ToList();

                foreach (var fine in userFines)
                {
                    fine.IsPaid = true;
                    unitOfWork.FineRepo.EditEntity(fine);
                }
            }
            else if (PerFineTabToggled)
            {
                SelectedFine.IsPaid = true;
                unitOfWork.FineRepo.EditEntity(SelectedFine);
            }

            unitOfWork.Save();
            SetFines();
        }

        private void SetFines()
        {
            // calculate all fines
            List<User> allUsers = unitOfWork.UserRepo.GetEntities().ToList();
            allUsers.ForEach(x => x.UserFines());

            Users = new ObservableCollection<User>(unitOfWork.UserRepo.GetEntities(
                x => x.Fines.Where(y => !y.IsPaid).Count() > 0, 
                x => x.MembershipType));
            Fines = new ObservableCollection<Fine>(unitOfWork.FineRepo.GetEntities(
                x => !x.IsPaid, 
                x => x.Item, 
                x => x.User, 
                x => x.User.MembershipType));
        }

        private void Reset()
        {
            SelectedUser = null;
            SelectedFine = null;
            SearchValue = "";
            SetFines();
        }

        private void Search()
        {
            if (PerUserTabToggled)
            {
                // .Where because props from partials can't be used within GetEntities :(
                Users = new ObservableCollection<User>(unitOfWork.UserRepo.GetEntities(
                    x => x.Fines.Where(y => !y.IsPaid).Count() > 0, 
                    x => x.MembershipType)
                    .Where(x => x.FullName.ToLower().Contains(SearchValue.ToLower())));
            }
            else if (PerFineTabToggled)
            {
                Fines = new ObservableCollection<Fine>(unitOfWork.FineRepo.GetEntities(
                    x => !x.IsPaid, 
                    x => x.Item, 
                    x => x.User, 
                    x => x.User.MembershipType)
                    .Where(x => x.User.FullName.ToLower().Contains(SearchValue.ToLower())));
            }
        }
    }
}
