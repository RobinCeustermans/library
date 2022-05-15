using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Bibliotheek_DAL.UnitsOfWork;
using Bibliotheek_DAL;
using Newtonsoft.Json;

namespace The_Boys_Project.ViewModels
{
    public class MailEditorViewModel : BaseViewModel
    {
        private IUnitOfWork unitOfWork = new UnitOfWork(new LibraryEntities());
        private string _mailText;
        private string _selectedMailToEdit;

        public string Path { get; set; }
        public string MailText
        {
            get { return _mailText; }
            set
            {
                _mailText = value;
                NotifyPropertyChanged();
                ChangeMailText();
            }
        }
        public List<string> Mails { get; set; }
        public string SelectedMailToEdit
        {
            get { return _selectedMailToEdit; }
            set
            {
                _selectedMailToEdit = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("KeyWords");
            }
        }
        public string KeyWords
        {
            get
            {
                string output = "#user# - Voornaam gebruiker";
                if (SelectedMailToEdit == "Registration")
                {
                    return output;
                }
                else if (SelectedMailToEdit == "BookfairRegistration")
                {
                    output += $"{Environment.NewLine}" +
                        $"#bookfair# - Naam boekenbeurs{Environment.NewLine}" +
                        $"#startdate# - Begindatum boekenbeurs{Environment.NewLine}" +
                        $"#enddate# - Einddatum boekenbeurs{Environment.NewLine}" +
                        $"#description# - Beschrijving boekenbeurs{Environment.NewLine}";
                    return output;
                }
                else if (SelectedMailToEdit == "AccountDeleted")
                {
                    return output;
                }
                else if (SelectedMailToEdit == "BookfairLocation")
                {
                    output += $"{Environment.NewLine}" +
                        $"#bookfair# - Naam boekenbeurs{Environment.NewLine}" +
                        $"#startdate# - Begindatum boekenbeurs{Environment.NewLine}" +
                        $"#enddate# - Einddatum boekenbeurs{Environment.NewLine}" +
                        $"#description# - Beschrijving boekenbeurs{Environment.NewLine}" + 
                        $"#location# - Locatie boekenbeurs{Environment.NewLine}";
                    return output;
                }
                return "";
            }
        }

        public MailEditorViewModel()
        {
            Path = System.IO.Path.Combine(Environment.CurrentDirectory, "MailEditor.html");
            Mails = new List<string>();
            unitOfWork.EmailTextRepo.GetEntities().ToList().ForEach(x => Mails.Add(x.Description));
        }

        public override string this[string columnName] => throw new NotImplementedException();

        public override bool CanExecute(object parameter)
        {
            if (parameter.ToString() == "ChangeMailText" || parameter.ToString() == "ShowMailText")
            {
                return SelectedMailToEdit != null;
            }
            return true;
        }

        public override void Execute(object parameter)
        {

        }

        private void ChangeMailText()
        {
            var mailToEdit = unitOfWork.EmailTextRepo.GetEntities(x => x.Description == SelectedMailToEdit).FirstOrDefault();
            mailToEdit.HTMLString = MailText;
            unitOfWork.EmailTextRepo.EditEntity(mailToEdit);
            unitOfWork.Save();
        }
    }
}
