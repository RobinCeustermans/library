using Bibliotheek_DAL;
using Bibliotheek_DAL.Repositories;
using Bibliotheek_DAL.UnitsOfWork;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using The_Boys_Project.models;

namespace The_Boys_Project.ViewModels
{
    public class RegistrerenViewModel : BaseViewModel
    {
        IUnitOfWork unitOfWork = new UnitOfWork(new LibraryEntities());
        MainViewModel MainViewModel { get; set; }

        private User _user;
        private string _confirmPassword;
        private string _password;
        private bool _Accepted;
        private ObservableCollection<string> _countries;
        private string _viewTitle;
        private string _registerErrorMessage;

        public RegistrerenViewModel(MainViewModel mainViewModel = null, bool addAdmin = false)
        {
            this.MainViewModel = mainViewModel;
            User = new User();
            Countries = new ObservableCollection<string>(Tools.GetCountries());
            ViewTitle = addAdmin ? "Admin toevoegen" : "Registreren";
            RegisterErrorMessage = "";
        }

        public string RegisterErrorMessage
        {
            get { return _registerErrorMessage; }
            set
            {
                _registerErrorMessage = value;
                NotifyPropertyChanged();
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

        public bool TitleIsAdminToevoegen
        {
            get
            {
                return ViewTitle == "Admin toevoegen" ? false : true;
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
            set { _password = value; }
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

        public bool Accepted
        {
            get { return _Accepted; }
            set { _Accepted = value; }
        }
        private List<User> _users;

        public List<User> Users
        {
            get { return _users; }
            set { _users = value; }
        }

        public RegistrerenViewModel(MainViewModel mainViewModel)
        {
            this.MainViewModel = mainViewModel;
            User = new User();
            Countries = new ObservableCollection<string>(Tools.GetCountries());
        }

        public override string this[string columnName]
        {
            get
            {
                if (ViewTitle != "Admin toevoegen" & columnName == nameof(Accepted) && Accepted != true)
                    return "Je moet de algemene voorwaarden accepteren om je te kunnen registreren!";

                if (columnName == nameof(ConfirmPassword) && ConfirmPassword != User.Password)
                    return "Wachtwoord en bevestig wachtwoord moeten identiek zijn!";
                return "";
            }
        }
        public override bool CanExecute(object parameter)
        {
            if (parameter.ToString() == "Registreren")
            {
                return IsValid() ? true : false;
            }
            return true;
        }

        public override void Execute(object parameter)
        {
            if (parameter.ToString() == "Register")
                Registreren();
            else if (parameter.ToString() == "Cancel")
                Cancel();
        }

        private void Cancel()
        {
            if (ViewTitle == "Admin toevoegen")
            {
                MainViewModel.UpdateViewCommand.Execute(parameter: "AdminOperations");
            }
            else
                // return to home view without logging in
                MainViewModel.UpdateViewCommand.Execute(parameter: "Login");

        }
        private void Registreren()
        {
            User.Password = Password;
            if (this.Error == "" && User.IsGeldig())
            {
                User.Password = Tools.Encrypt(Password);
                IUnitOfWork unitOfWork = new UnitOfWork(new LibraryEntities());
                Users = unitOfWork.UserRepo.GetEntities().ToList();
                User.MembershipTypeID = ViewTitle == "Admin toevoegen" ? 3 : 1;
                User.IsActive = true;
                if (!Users.Contains(User))
                {
                    unitOfWork.UserRepo.AddEntity(User);
                    int ok = unitOfWork.Save();
                    if (ok >= 1)
                    {
                        if (User.MembershipTypeID != 3)
                        {
                            Tools.SendMail(User, "Registration", "Bevestiging van registratie!");
                            MainViewModel.UpdateViewCommand.Execute(parameter: "Login");
                        }
                        else
                          MainViewModel.UpdateViewCommand.Execute(parameter: "AdminOperations");
                    }
                    else RegisterErrorMessage = "Oeps er ging iets fout probeer later nog eens.";
                }
                else RegisterErrorMessage = "Een gebruiker met hetzelfde emailadres bestaat al!";
            }
            else RegisterErrorMessage = Error + User.Error;
        }
    }
}
