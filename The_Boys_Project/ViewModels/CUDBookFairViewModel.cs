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
    public class CUDBookFairViewModel : BaseViewModel
    {
        IUnitOfWork unitOfWork = new UnitOfWork(new LibraryEntities());

        private string _viewTitle;
        private BookFair _bookFairToEdit;
        private List<BookFair> _bookFairs;
        private string _errorMessage;

        public List<BookFair> BookFairs
        {
            get { return _bookFairs; }
            set { _bookFairs = value; }
        }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public string RegistrationOpen { get; set; }

        public bool BookFairHasNoLocation { get; set; }

        public BookFair BookFairToEdit
        {
            get { return _bookFairToEdit; }
            set
            {
                _bookFairToEdit = value;
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


        public bool ButtonCheck
        {
            get
            {
                if (ViewTitle == "Boekenbeurs bewerken")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }


        public string ViewTitle
        {
            get { return _viewTitle; }
            set
            {
                _viewTitle = value;
                NotifyPropertyChanged();
            }
        }

        MainViewModel MainViewModel { get; set; }

        public CUDBookFairViewModel(MainViewModel mainViewModel, BookFair bookFairToEdit)
        {
            this.MainViewModel = mainViewModel;
            this.BookFairToEdit = bookFairToEdit;
            ConfigureBookFairRecord(bookFairToEdit);
            if (BookFairToEdit != null) BookFairHasNoLocation = BookFairToEdit.Location == null;
            ErrorMessage = "";
            BookFairs = unitOfWork.BookFairRepo.GetEntities().ToList();
        }

        private void ConfigureBookFairRecord(BookFair bookFairToEdit)
        {
            if (bookFairToEdit != null)
            {
                ViewTitle = "Boekenbeurs bewerken";

                Name = bookFairToEdit.Name;
                Description = bookFairToEdit.Description;
                StartDate = bookFairToEdit.StartDate;
                EndDate = bookFairToEdit.EndDate;
                Location = bookFairToEdit.Location;
            }
            else
            {
                ViewTitle = "Boekenbeurs toevoegen";
                StartDate = DateTime.Now.Date;
                EndDate = DateTime.Now.Date;
            }
        }

        public override string this[string columnName]
        {
            get { return ""; }
        }

        public override bool CanExecute(object parameter)
        {
            if (parameter.ToString() == "EditBookFair")
            {
                return BookFairToEdit != null;
            }
            return true;
        }

        public override void Execute(object parameter)
        {
            if (parameter.ToString() == "AddBookFair")
            {
                AddBookFair();
            }
            else if (parameter.ToString() == "EditBookFair")
            {
                EditBookFair();
            }
            else if (parameter.ToString() == "Cancel")
            {
                Cancel();
            }
        }

        private void AddBookFair()
        {
            BookFair bookFair = new BookFair()
            {
                Name = this.Name,
                Description = this.Description,
                StartDate = this.StartDate,
                EndDate = this.EndDate,
                Location = this.Location,
                RegistrationOpen = true
            };


            if (!BookFairs.Contains(bookFair))
            {
                if (bookFair.IsValid())
                {
                    
                    unitOfWork.BookFairRepo.AddEntity(bookFair);
                    int ok = unitOfWork.Save();

                    if (ok > 0)
                    {
                        Cancel();
                    }
                }
                else
                {
                    ErrorMessage = bookFair.Error;
                }
            }

        }

        private void EditBookFair()
        {
            BookFairToEdit.Name = this.Name;
            BookFairToEdit.Description = this.Description;
            BookFairToEdit.StartDate = this.StartDate;
            BookFairToEdit.EndDate = this.EndDate;
            BookFairToEdit.Location = this.Location;

            if (BookFairToEdit.IsValid())
            {
                unitOfWork.BookFairRepo.AddOrUpdate(BookFairToEdit);
                int ok = unitOfWork.Save();
                if (ok > 0)
                {
                    BookFairHasNoLocation = BookFairToEdit.Location == null;
                    if (!BookFairHasNoLocation)
                    {
                        List<User> recipients = unitOfWork.UserRepo.GetEntities(
                            x => x.UserBookFairs.Any(
                                y => y.BookFairID == BookFairToEdit.BookFairID)).ToList();
                        Tools.SendMail(
                            recipients,
                            "BookfairLocation",
                            "Locatie vastgelegd voor een boekenbeurs waar jij voor ingeschreven bent",
                            BookFairToEdit);
                    }
                    Cancel();
                }
            }
            else
            {
                ErrorMessage = BookFairToEdit.Error;
            }
        }

        private void Cancel()
        {
            MainViewModel.UpdateViewCommand.Execute(parameter: "BookFair");
        }

    }
}
