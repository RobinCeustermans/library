using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Boys_Project.ViewModels
{
    public class MailEditorWindowViewModel: BaseViewModel
    {
        private BaseViewModel _selectedViewModel;

        public BaseViewModel SelectedViewModel
        {
            get { return _selectedViewModel; }
            set
            {
                _selectedViewModel = value;
                NotifyPropertyChanged();
            }
        }

        public override string this[string columnName] => throw new NotImplementedException();

        public MailEditorWindowViewModel()
        {
            SelectedViewModel = new MailEditorViewModel();
        }

        public override bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public override void Execute(object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
