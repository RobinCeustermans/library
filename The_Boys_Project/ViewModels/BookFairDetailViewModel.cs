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
    public class BookFairDetailViewModel : BaseViewModel
    {
        IUnitOfWork unitOfWork = new UnitOfWork(new LibraryEntities());

        public MainViewModel MainViewModel { get; set; }

        private ObservableCollection<UserBookFair> _usersBookFair;

        private string _bookFairTitle = "";
        private string _totalRegistered = "";
        private BookFair _selectedBookFair { get; set; }

        public BookFair SelectedBookFair
        {
            get { return _selectedBookFair; }
            set
            {
                _selectedBookFair = value;
                NotifyPropertyChanged();
            }
        }

        public string TotalRegistered
        {
            get { return _totalRegistered; }
            set 
            {
                _totalRegistered = value;
                NotifyPropertyChanged();
            }
            
        }


        public ObservableCollection<UserBookFair> UsersBookFair
        {
            get { return _usersBookFair; }
            set
            {
                _usersBookFair = value;
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


        public override string this[string columnName]
        {
            get { return ""; }
        }

        public override bool CanExecute(object parameter)
        {
            if (parameter.ToString() == "StopRegister")
            {
                return SelectedBookFair.RegistrationOpen;
            }
            return true;
        }

        public override void Execute(object parameter)
        {
            if (parameter.ToString() == "StopRegister")
            {
                StopRegister(SelectedBookFair);
            }
        }

        private void StopRegister(BookFair bookFair)
        {
            SelectedBookFair.RegistrationOpen = false;
            unitOfWork.BookFairRepo.AddOrUpdate(SelectedBookFair);
            unitOfWork.Save();
        }

        public BookFairDetailViewModel(MainViewModel mainViewModel, BookFair bookFairToShow)
        {
            this.SelectedBookFair = bookFairToShow;
            this.MainViewModel = mainViewModel;
            GetUsersBookFair();
            BookFairTitle = "Boekenbeurs: " + bookFairToShow.Name;
            TotalRegistered = "Totaal aantal inschrijvingen: " + UsersBookFair.Count.ToString();
        }


        private void GetUsersBookFair()
        {
            UsersBookFair = new ObservableCollection<UserBookFair>(unitOfWork.UserBookFairRepo.GetEntities(x => x.BookFairID == SelectedBookFair.BookFairID, x => x.User));
        }


    }
}
