using Bibliotheek_DAL;
using Bibliotheek_DAL.UnitsOfWork;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace The_Boys_Project.ViewModels
{
    class UitleenhistoriekViewModel : BaseViewModel
    {
        IUnitOfWork unitOfWork = new UnitOfWork(new LibraryEntities());

        private ObservableCollection<UserItem> _userItems;
        private Item _selectedItem;

        public Item SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<UserItem> UserItems
        {
            get { return _userItems; }
            set
            {
                _userItems = value;
                NotifyPropertyChanged();
            }
        }

        public UitleenhistoriekViewModel(Item item)
        {
            SelectedItem = item;
            UserItems = new ObservableCollection<UserItem>(unitOfWork.UserItemRepo.GetEntities(
                x => x.ItemID == item.ItemID,
                x => x.User));
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
