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
    public class UserItemControlViewModel : BaseViewModel
    {
        IUnitOfWork unitOfWork = new UnitOfWork(new LibraryEntities());
        private User _user;
        public bool UserHasFines;
        //variable is not used can we remove it? private bool _userHasFines;
        private ObservableCollection<UserItem> _borrowedItems;
        private UserItem _selectedBorrowedItem;
        private UserItem _selectedReservedItem;
        private ObservableCollection<UserItem> _reservedItems;
        public string FineText { get; set; }
        public MainViewModel MainViewModel { get; set; }

        public string Title
        {
            get
            {
                return $"Boeken in- en uitchecken voor {User}";
            }
        }

        public User User
        {
            get { return _user; }
            set
            {
                _user = value;
                NotifyPropertyChanged();
            }
        }     

        public ObservableCollection<UserItem> BorrowedItems
        {
            get { return _borrowedItems; }
            set
            {
                _borrowedItems = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<UserItem> ReservedItems
        {
            get { return _reservedItems; }
            set
            {
                _reservedItems = value;
                NotifyPropertyChanged();
            }
        }

        public UserItem SelectedBorrowedItem
        {
            get { return _selectedBorrowedItem; }
            set
            {
                _selectedBorrowedItem = value;
                NotifyPropertyChanged();
            }
        }

        public UserItem SelectedReservedItem
        {
            get { return _selectedReservedItem; }
            set
            {
                _selectedReservedItem = value;
                NotifyPropertyChanged();
            }
        }

        public UserItemControlViewModel(MainViewModel mainView, User user)
        {
            this.MainViewModel = mainView;
            this.User = user;

            User.UserFines();
            SetData(this.User);
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
            if (parameter.ToString() == "ConfirmReturned")
            {
                return SelectedBorrowedItem != null;
            }
            else if (parameter.ToString() == "ConfirmBorrowed")
            {
                return SelectedReservedItem != null && !UserHasFines;
            }
            return true;
        }

        public override void Execute(object parameter)
        {
            if (parameter.ToString() == "ConfirmReturned")
            {
                ConfirmReturned(this.SelectedBorrowedItem);
            }
            else if (parameter.ToString() == "ConfirmBorrowed")
            {
                ConfirmBorrowed(this.SelectedReservedItem);
            }
            else if (parameter.ToString() == "Cancel")
            {
                Cancel();
            }
            else if (parameter.ToString() == "ManageFine")
            {
                ManageFine();
            }
        }

        private void ConfirmReturned(UserItem itemToReturn)
        {
            itemToReturn.ReturnedDate = DateTime.Now;

            unitOfWork.UserItemRepo.EditEntity(itemToReturn);
            unitOfWork.Save();

            SetData(this.User);
        }

        private void ConfirmBorrowed(UserItem itemToBorrow)
        {
            itemToBorrow.BorrowedDate = DateTime.Now;
            itemToBorrow.DueDate = DateTime.Now + new TimeSpan(14, 0, 0, 0);

            unitOfWork.UserItemRepo.EditEntity(itemToBorrow);
            unitOfWork.Save();

            SetData(this.User);
        }

        private void SetData(User user)
        {
            if (User.TotalFine != 0)
            {
                FineText = $"Er staat nog een boete van {user.TotalFine} euro open.";
                UserHasFines = true;
            }

            this.BorrowedItems = new ObservableCollection<UserItem>(unitOfWork.UserItemRepo.GetEntities(
                x => x.UserID == user.UserID &&
                x.BorrowedDate != null &&
                x.ReturnedDate == null,
                x => x.Item));

            this.ReservedItems = new ObservableCollection<UserItem>(unitOfWork.UserItemRepo.GetEntities(
                x => x.UserID == user.UserID &&
                x.BorrowedDate == null &&
                x.ReservedUntil > DateTime.Now,
                x => x.Item));
        }

        private void Cancel()
        {
            this.MainViewModel.UpdateViewCommand.Execute(parameter: "UserOverview");
        }

        private void ManageFine()
        {
            this.MainViewModel.FineSearchValue = User.FullName;
            this.MainViewModel.UpdateViewCommand.Execute(parameter: "Fines");
        }
    }
}
