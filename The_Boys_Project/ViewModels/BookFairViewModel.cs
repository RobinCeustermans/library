using Bibliotheek_DAL;
using Bibliotheek_DAL.UnitsOfWork;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using The_Boys_Project.models;

namespace The_Boys_Project.ViewModels
{
    public class BookFairViewModel : BaseViewModel
    {

        IUnitOfWork unitOfWork = new UnitOfWork(new LibraryEntities());

        public MainViewModel MainViewModel { get; set; }


        private ObservableCollection<BookFair> _bookfairs;
        private ObservableCollection<UserBookFair> _userBookFairs;


        private BookFair _selectedBookFair;
        private User _user;
        private string _bookFairTitle = "";
        private string _errorMessage = "";

        public DialogResult MessageBoxResult { get; set; }

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<BookFair> BookFairs
        {
            get { return _bookfairs; }
            set
            {
                _bookfairs = value;
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

        public BookFair SelectedBookFair
        {
            get { return _selectedBookFair; }
            set
            {
                _selectedBookFair = value;
                NotifyPropertyChanged();
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

        public string BookFairTitle
        {
            get { return _bookFairTitle; }
            set
            {
                _bookFairTitle = value;
                NotifyPropertyChanged();
            }
        }

        public bool UserIsAdmin
        {
            get
            {
                if (User != null)
                {
                    if (User.MembershipType.Description == "Admin" || User.MembershipType.Description == "Hoofdadmin")
                    {
                        return true;
                    }
                    else
                    { return false; }
                }
                else return false;
            }
        }

        public override string this[string columnName]
        {
            get { return ""; }
        }

        public override bool CanExecute(object parameter)
        {
            if (parameter.ToString() == "Edit")
            {
                return SelectedBookFair != null;
            }
            if (parameter.ToString() == "Delete")
            {
                return SelectedBookFair != null;
            }
            if (parameter.ToString() == "Register")
            {
                if (SelectedBookFair != null && User != null && !UserBookFairs.Any(x => x.BookFairID == SelectedBookFair.BookFairID))
                {
                    return SelectedBookFair.RegistrationOpen;
                }
                else
                {
                    return false;
                }
            }
            if (parameter.ToString() == "Detail")
            {
                return SelectedBookFair != null;
            }
            return true;

        }

        public override void Execute(object parameter)
        {
            if (parameter.ToString() == "Reset")
            {
                Reset();
            }
            else if (parameter.ToString() == "Search")
            {
                Search();
            }
            else if (parameter.ToString() == "Add")
            {
                Add();
            }
            else if (parameter.ToString() == "Edit")
            {
                Edit();
            }
            else if (parameter.ToString() == "Delete")
            {
                Delete(SelectedBookFair);
            }
            else if (parameter.ToString() == "Register")
            {
                Register();
            }
            else if (parameter.ToString() == "Detail")
            {
                Detail();
            }
        }

        public BookFairViewModel() { }

        public BookFairViewModel(MainViewModel mainViewModel, User user)
        {
            this.MainViewModel = mainViewModel;
            this.User = user;
            FillDataGrid();

            if (user != null)
            {
                GetUserBookFairs();
            }
        }

        public void Search()
        {
            BookFairs = new ObservableCollection<BookFair>(unitOfWork.BookFairRepo.GetEntities(
                x => x.Name.Contains(BookFairTitle),
                x => x.UserBookFairs.Select(z => z.User)));
            if (BookFairs.Count == 0)
            {
                ErrorMessage = "Er zijn geen overeenkomstige boekenbeurzen gevonden.";
            }
            else
            {
                ErrorMessage = "";
            }

        }

        private void Register()
        {

            UserBookFair registeredBookFair = new UserBookFair()
            {
                BookFairID = SelectedBookFair.BookFairID,
                UserID = this.User.UserID
            };

            if (!UserBookFairs.Any(x => x.BookFairID == registeredBookFair.BookFairID))
            {
                unitOfWork.UserBookFairRepo.AddEntity(registeredBookFair);
                int ok = unitOfWork.Save();
                if (ok > 0)
                {
                    Tools.SendMail(User, "BookfairRegistration", "Registratie voor boekenbeurs geslaagd!", SelectedBookFair);
                    Reset();
                }
                else
                {
                    ErrorMessage = "Er ging iets mis";
                }
            }
            else
            {
                ErrorMessage = "U bent al ingeschreven";
            }
        }


        public void Delete(BookFair bookFair)
        {
            BookFair bookFairToDelete = unitOfWork.BookFairRepo.GetEntities(
                x => x.BookFairID == bookFair.BookFairID,
                x => x.UserBookFairs).FirstOrDefault();

            MessageBoxResult = MessageBox.Show($"Bent u zeker dat u {SelectedBookFair.Name} wilt verwijderen?", "Waarschuwing", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (MessageBoxResult == System.Windows.Forms.DialogResult.Yes)
            {

                unitOfWork.UserBookFairRepo.DeleteRange(bookFairToDelete.UserBookFairs);
                unitOfWork.BookFairRepo.DeleteEntity(bookFairToDelete);

                unitOfWork.Save();

                SelectedBookFair = null;
                FillDataGrid();
                ErrorMessage = "Boekenbeurs is verwijderd.";
            }
        }

        public void Add()
        {
            MainViewModel.UpdateViewCommand.Execute(parameter: "AddBookFair");
        }

        public void Edit()
        {
            MainViewModel.SelectedBookFairToEdit = SelectedBookFair;
            MainViewModel.UpdateViewCommand.Execute(parameter: "EditBookFair");
        }

        public void Detail()
        {
            MainViewModel.SelectedBookFairToEdit = SelectedBookFair;
            MainViewModel.UpdateViewCommand.Execute(parameter: "DetailBookFair");
        }

        public void Reset()
        {
            SelectedBookFair = null;
            ErrorMessage = "";
            BookFairTitle = "";
            FillDataGrid();
            if (User != null) GetUserBookFairs();
        }

        private void FillDataGrid()
        {
            BookFairs = new ObservableCollection<BookFair>(unitOfWork.BookFairRepo.GetEntities(x => x.UserBookFairs.Select(z => z.User)));
        }

        private void GetUserBookFairs()
        {
            UserBookFairs = new ObservableCollection<UserBookFair>(unitOfWork.UserBookFairRepo.GetEntities(x => x.UserID == User.UserID));
        }
    }
}
