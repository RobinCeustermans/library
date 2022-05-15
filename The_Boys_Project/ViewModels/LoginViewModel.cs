using Bibliotheek_DAL.UnitsOfWork;
using Bibliotheek_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using The_Boys_Project.models;

namespace The_Boys_Project.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        // Initialise Unit of work
        IUnitOfWork unitOfWork = new UnitOfWork(new LibraryEntities());
        MainViewModel MainViewModel { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        private string _errorMessage;

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                NotifyPropertyChanged();
            }
        }

        public LoginViewModel(MainViewModel mainViewModel)
        {
            this.MainViewModel = mainViewModel;
            ErrorMessage = "";
        }

        public void Dispose()
        {
            unitOfWork?.Dispose();
        }

        public override string this[string columnName]
        {
            get
            {
                if (columnName == "Email" && string.IsNullOrWhiteSpace(Email))
                {
                    return "Vul een e-mailadres in.";
                }
                else if (columnName == "Password" && string.IsNullOrWhiteSpace(Password))
                {
                    return "Vul een wachtwoord in.";
                }
                return "";
            }
        }

        public override bool CanExecute(object parameter)
        {
            if (parameter.ToString() == "Login")
            {
                return (IsValid()) ? true : false;
            }
            return true;
        }

        public override void Execute(object parameter)
        {
            // login button logic
            if (parameter.ToString() == "Login")
            {
                // try to get user by e-mail from DB, includes Membershiptype!
                User user = unitOfWork.UserRepo.GetEntities(x => x.Email == this.Email, x => x.MembershipType).FirstOrDefault();
                if (IsValidLogin(user))
                {
                    user.CheckMembership();

                    Login(user);
                }
            }
            else if (parameter.ToString() == "Registreren")
            {
                Registreren();
            }
            // cancel button logic
            else if (parameter.ToString() == "Cancel")
            {
                Cancel();
            }
        }

        private void Login(User userToLogIn)
        {
            // set logged in user in mainview 
            MainViewModel.User = userToLogIn;

            // dispose of UnitOfWork right before changing views
            unitOfWork.Dispose();

            // redirect to home view
            MainViewModel.UpdateViewCommand.Execute(parameter: "Home");
        }
        private void Registreren()
        {
            // dispose of UnitOfWork right before changing views
            unitOfWork.Dispose();

            // redirect to registreren view
            MainViewModel.UpdateViewCommand.Execute(parameter: "Registreren");
        }

        private void Cancel()
        {
            // return to home view without logging in
            MainViewModel.UpdateViewCommand.Execute(parameter: "Home");
        }

        private bool IsValidLogin(User userToValidate)
        {
            if (userToValidate != null)
            {
                // in case of valid email, check if password matches email
                if (Tools.Decrypt(userToValidate.Password) == this.Password)
                {
                    if (userToValidate.IsActive)
                    {
                        // on valid login set the mai
                        return true;
                    }
                    else
                    {
                        ErrorMessage = "Dit account is inactief, er kan niet ingelogd worden, contacteer de hoofdadmin.";
                        return false;
                    }                     
                }
                // invalid password
                ErrorMessage = "De combinatie van e-mail en wachtwoord die u heeft ingegeven is niet correct.";
                return false;
            }
            // no valid email address
            ErrorMessage = "De combinatie van e-mail en wachtwoord die u heeft ingegeven is niet correct.";
            return false;
        }

        private bool UserIsAdmin(User user)
        {
            return (user.MembershipType.Description == "Admin" ||
            user.MembershipType.Description == "Hoofdadmin") ? true : false;
        }
    }
}
