using Bibliotheek_DAL;
using Bibliotheek_DAL.UnitsOfWork;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using The_Boys_Project.models;

namespace The_Boys_Project.ViewModels
{
    public class AdminUserDetailsViewModel : BaseViewModel
    {
        IUnitOfWork unitOfWork = new UnitOfWork(new LibraryEntities());
        public MainViewModel MainViewModel { get; set; }
        private ObservableCollection<UserItem> _userItems;
        private ObservableCollection<UserBookFair> _userBookFairs;
        private ObservableCollection<UserItem> _reserveduserItems;
        private ObservableCollection<UserItem> _borroweduserItems;
        private ObservableCollection<Fine> _userFines;
        private User _user;
        private bool _firstTab;
        private bool _secondTab;
        private bool _isUser;

        public bool IsUser
        {
            get { return _isUser; }
            set
            {
                _isUser = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<Fine> UserFines
        {
            get { return _userFines; }
            set
            {
                _userFines = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<UserItem> BorroweduserItems
        {
            get { return _borroweduserItems; }
            set 
            {
                _borroweduserItems = value; 
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<UserItem> ReserveduserItems
        {
            get { return _reserveduserItems; }
            set 
            {
                _reserveduserItems = value; 
                NotifyPropertyChanged(); 
            }
        }

        public bool FirstTab
        {
            get { return _firstTab; }
            set { _firstTab = value; NotifyPropertyChanged(); }
        }


        public bool SecondTab
        {
            get { return _secondTab; }
            set { _secondTab = value; NotifyPropertyChanged(); }
        }

        public ObservableCollection<UserItem> UserItems
        {
            get { return _userItems; }
            set { _userItems = value; NotifyPropertyChanged(); }
        }

        public ObservableCollection<UserBookFair> UserBookFairs
        {
            get { return _userBookFairs; }
            set { _userBookFairs = value; NotifyPropertyChanged(); }
        }

        public User User
        {
            get { return _user; }
            set { _user = value; }
        }
        public AdminUserDetailsViewModel(User user, MainViewModel mainViewModel = null)
        {
            user.UserFines();
            FirstTab = true;
            SecondTab = false;
            this.User = user;
            this.MainViewModel = mainViewModel;
            SetData();
        }
        public void SetData()
        {
            IsUser = (this.User.MembershipTypeID == 1 || this.User.MembershipTypeID == 2) ? true : false;
            UserItems = new ObservableCollection<UserItem>(unitOfWork.UserItemRepo.GetEntities(
              x => x.Item.ContributorItems.Select(z => z.Contributor),
              x => x.Item.ItemCategories.Select(z => z.Category),
              x => x.Item).Where(x => x.UserID == User.UserID));
            ReserveduserItems = new ObservableCollection<UserItem>(UserItems.Where(x => x.BorrowedDate == null));
            BorroweduserItems = new ObservableCollection<UserItem>(UserItems.Where(x => x.BorrowedDate != null));
            UserBookFairs = new ObservableCollection<UserBookFair>(unitOfWork.UserBookFairRepo.GetEntities(x => x.BookFair));
            UserFines = new ObservableCollection<Fine>(unitOfWork.FineRepo.GetEntities(x => x.Item).Where(x => x.UserID == User.UserID));
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
            return true;
        }

        public override void Execute(object parameter)
        {
        }
    }
}

