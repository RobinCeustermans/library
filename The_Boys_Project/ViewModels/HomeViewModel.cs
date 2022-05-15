using Bibliotheek_DAL;
using Bibliotheek_DAL.UnitsOfWork;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using The_Boys_Project.models;
using The_Boys_Project.Views;

namespace The_Boys_Project.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        public MainViewModel MainViewModel { get; set; }

        // Database Connection 

        IUnitOfWork unitOfWork = new UnitOfWork(new LibraryEntities());

        public string ErrorMessage { get; set; }

        private bool _buyable;

        public bool Buyable
        {
            get { return _buyable; }
            set { _buyable = value; NotifyPropertyChanged(); }
        }

        // private lists
        private ObservableCollection<Item> _items;
        private ObservableCollection<AgeCategory> _ages;
        private ObservableCollection<Category> _categories;

        // private selected things
        private AgeCategory _selectedAge;
        private Category _selectedCategory;
        private Item _selectedItem;

        // private miscellaneous
        private string _booktitle = "";
        private int _amountReservedByUser;
        private User _user;
        private string _errorMessageSearch;

        // public lists
        public ObservableCollection<AgeCategory> Ages
        {
            get { return _ages; }
            set
            {
                _ages = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<Category> Categories
        {
            get { return _categories; }
            set
            {
                _categories = value;
                NotifyPropertyChanged();
            }
        }

        // public selected things

        public AgeCategory SelectedAge
        {
            get { return _selectedAge; }
            set
            {
                _selectedAge = value;
                NotifyPropertyChanged();
            }
        }

        public Category SelectedCategory
        {
            get { return _selectedCategory; }
            set
            {
                _selectedCategory = value;
                NotifyPropertyChanged();
            }
        }

        // public miscellaneous

        public string ErrorMessageBookSearch
        {
            get { return _errorMessageSearch; }
            set
            {
                _errorMessageSearch = value;
                NotifyPropertyChanged();
            }
        }

        public string BookTitle
        {
            get { return _booktitle; }
            set
            {
                _booktitle = value;
                NotifyPropertyChanged();
            }
        }

        public int AmountReservedByUser
        {
            get { return _amountReservedByUser; }
            set
            {
                _amountReservedByUser = value;
                NotifyPropertyChanged();
            }
        }

        public bool ItemWithinSaleRangeAndNotAdmin
        {
            get { return SelectedItemWithinSaleRange && !UserIsAdmin; }
        }

        public bool SelectedItemWithinSaleRange
        {
            get
            {
                if (SelectedItem != null)
                {
                    return SelectedItem.IsWithinSaleRange;
                }
                return false;
            }
        }

        public Item SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("SelectedItemWithinSaleRange");
                NotifyPropertyChanged("ItemWithinSaleRangeAndNotAdmin");
                GetDetailsOfBook();
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

        public ObservableCollection<ContributorItem> ContributorsOfBook { get; set; }

        public ObservableCollection<Item> Items
        {
            get { return _items; }
            set
            {
                _items = value;
                NotifyPropertyChanged();
            }
        }

        public string Title
        {
            // dynamically set the homescreen title if a user logs in
            get
            {
                if (User != null)
                {
                    return $"Welkom, {User.Name}!";
                }
                else return "Welkom";
            }
        }

        private void RefreshContributors()
        {
            if (SelectedItem != null)
            {
                int i = SelectedItem.ItemID;
                ContributorsOfBook = new ObservableCollection<ContributorItem>(unitOfWork.ContributorItemRepo.GetEntities(x => x.ItemID == i));
            }
        }

        public HomeViewModel(MainViewModel mainViewModel = null, User user = null)
        {
            this.MainViewModel = mainViewModel;
            this.User = user;
            Items = new ObservableCollection<Item>(unitOfWork.ItemRepo.GetEntities(x => x.ContributorItems));

            // check for amount of items already reserved by user
            if (User != null)
            {
                foreach (UserInterestedItem userInterestedItem in unitOfWork.UserInterestedItemRepo.GetEntities(x => x.Item).Where(y => y.UserID == this.User.UserID))
                {
                    bool check = DateTime.TryParse(userInterestedItem.DrawnDate?.AddDays(14).ToString(), out DateTime dateTime);
                    if (DateTime.Now <= dateTime)
                    {
                        MessageBox.Show("Je bent uitgeloot om " + userInterestedItem.Item.Title + " te mogen aankopen. Je hebt tot " + dateTime.ToString("d") + " om het boek af te betalen en af te halen aan de balie");
                    }
                }
                AmountReservedByUser = unitOfWork.UserItemRepo.GetEntities(
                    x => x.UserID == this.User.UserID &&
                    x.ReservedUntil > DateTime.Now)
                    .Count();
            }
            FillDataGrid();
            FillComboBoxAges();
            FillComboBoxCategories();
            ErrorMessageBookSearch = "";
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
            if (parameter.ToString() == "Reserveren")
            {
                if (SelectedItem != null && User != null)
                    return ItemIsAvailable();
                else return false;
            }
            else if (parameter.ToString() == "Uitleenhistoriek")
            {
                if (SelectedItem == null)
                {
                    return false;
                }
                else return true;
            }
            else if (parameter.ToString() == "InterestedInBook")
            {
                if (this.User != null && SelectedItem != null)
                {
                    if (this.User.MembershipTypeID == 2)
                    {
                        return true;
                    }
                    return false;
                }
                return false;
            }
            else if (parameter.ToString() == "DeleteItem")
            {
                if (SelectedItem == null)
                {
                    return false;
                }
                return true;
            }
            else if (parameter.ToString() == "Add")
            {
                return true;
            }
            else if (parameter.ToString() == "Aanpassen")
            {
                return true;
            }
            else if (parameter.ToString() == "Reset")
            {
                return true;
            }
            else if (parameter.ToString() == "Search")
            {
                return true;
            }
            else if (parameter.ToString() == "Edit")
            {
                if (SelectedItem == null)
                {
                    return false;
                }
                else return true;
            }
            else if (parameter.ToString() == "RaffleBook")
            {
                if (SelectedItem != null && SelectedItem.IsWithinSaleRange == true)
                {
                    return true;
                }
                else return false;
            }
            return true;
        }

        private void GetDetailsOfBook()
        {
            ErrorMessage = "";
            if (IsValid())
            {
                RefreshContributors();
            }
        }

        private void Edit()
        {
            if (SelectedItem != null)
            {
                MainViewModel.SelectedItemToEdit = SelectedItem;
                MainViewModel.UpdateViewCommand.Execute(parameter: "Aanpassen");
            }
            else ErrorMessage = "Selecteer een boek om te bewerken";
        }

        private void Add()
        {
                MainViewModel.UpdateViewCommand.Execute(parameter: "Toevoegen");
        }
        
        public override void Execute(object parameter)
        {
            if (parameter.ToString() == "Edit")
            {
                Edit();
            }
            else if (parameter.ToString() == "Add")
            {
                Add();
            }
            else if (parameter.ToString() == "Reserveren")
            {
                Reserveren();
            }
            else if (parameter.ToString() == "Uitleenhistoriek")
            {
                Uitleenhistoriek();
            }
            else if (parameter.ToString() == "Search")
            {
                Search();
            }
            else if (parameter.ToString() == "Reset")
            {
                Reset();
            }
            else if (parameter.ToString() == "InterestedInBook")
            {
                InterestedInBook();
            }
            else if (parameter.ToString() == "DeleteItem")
            {
                DeleteItem(SelectedItem);
            }
            else if (parameter.ToString() == "RaffleBook")
            {
                RaffleBook();
            }
        }

        private void InterestedInBook()
        {
            UserInterestedItem userInterestedItem = new UserInterestedItem()
            {
                ItemID = this.SelectedItem.ItemID,
                UserID = this.User.UserID
            };
            ObservableCollection<UserInterestedItem> userInterestedItems = new ObservableCollection<UserInterestedItem>(unitOfWork.UserInterestedItemRepo.GetEntities(
                x => x.UserID == this.User.UserID, x => x.Item,
                x => x.User));
            if (!userInterestedItems.Contains(userInterestedItem))
            {
                unitOfWork.UserInterestedItemRepo.AddEntity(userInterestedItem);
                unitOfWork.Save();
                MessageBox.Show("Je interesse in dit boek is bevestigd. Indien jij uitgeloot word om dit boek te kopen brengen we je op de hoogte");
                SelectedItem = null;
            }
            else MessageBox.Show("Je interesse in dit boek is al reeds geregistreerd"); // change to confirm messagebox 
        }
        private void Uitleenhistoriek()
        {
            MainViewModel.SelectedItemToEdit = SelectedItem;
            MainViewModel.UpdateViewCommand.Execute(parameter: "Uitleenhistoriek");
        }

        private void RaffleBook()
        {
            ObservableCollection<UserInterestedItem> userInterestedItems =
              new ObservableCollection<UserInterestedItem>(unitOfWork.UserInterestedItemRepo.
              GetEntities(x => x.ItemID == SelectedItem.ItemID));
            bool check = true;
            DateTime LastDrawnDate = new DateTime();
            foreach (UserInterestedItem userInterestedItem in userInterestedItems)
            {
                if (userInterestedItem.DrawnDate != null)
                {
                    if (DateTime.Now < userInterestedItem.DrawnDate?.AddDays(14))
                    {
                        check = false;
                        DateTime.TryParse(userInterestedItem.DrawnDate?.AddDays(14).ToString(), out LastDrawnDate);
                    }
                    else
                    {
                        unitOfWork.UserInterestedItemRepo.DeleteEntity(userInterestedItem);
                        unitOfWork.Save();
                    }
                }
            }
            if (check == true)
            {
                if (userInterestedItems.Count > 0)
                {
                    Random random = new Random();
                    int chosenOne = random.Next(1, userInterestedItems.Count);
                    userInterestedItems[chosenOne].DrawnDate = DateTime.Today;
                    unitOfWork.UserInterestedItemRepo.EditEntity(userInterestedItems[chosenOne]);
                    unitOfWork.Save();
                    MessageBox.Show("Boek is verloot");
                }
                else MessageBox.Show("Jammer genoeg is er nog niemand geinteresseerd in dit boek. Het kan nog niet uitgeloot worden.");
            }
            else MessageBox.Show("Er is minder dan 14 dagen geleden al een lid uitgeloot. op " + LastDrawnDate.ToString("d") + " kan er een nieuw lid geloot worden");
        }

        public bool ItemIsAvailable()
        {
            return !SelectedItem.ItemIsReserved && !SelectedItem.ItemIsLentOut;
        }

        public void Reserveren()
        {
            if (User.MembershipType.MaxNumberItems > AmountReservedByUser)
            {
                // add reservation to DB
                UserItem reservedItem = new UserItem()
                {
                    ItemID = SelectedItem.ItemID,
                    UserID = this.User.UserID,
                    ReservedUntil = DateTime.Now + new TimeSpan(3, 0, 0)
                };

                unitOfWork.UserItemRepo.AddEntity(reservedItem);
                int ok = unitOfWork.Save();
                if (ok > 0)
                {
                    // recalculate amount reserved by user
                    AmountReservedByUser = unitOfWork.UserItemRepo.GetEntities(
                        x => x.UserID == this.User.UserID &&
                        x.ReservedUntil > DateTime.Now)
                        .Count();
                }
                else
                {
                    // TODO: Change to error message
                    MessageBox.Show("Er ging iets mis");
                }
            }
        }

        private void DeleteItem(Item item)
        {
            Item itemToDelete = unitOfWork.ItemRepo.GetEntities(
                x => x.ItemID == item.ItemID,
                x => x.Fines,
                x => x.UserItems,
                x => x.ItemCategories,
                x => x.UserInterestedItems,
                x => x.ContributorItems)
                .FirstOrDefault();

            if (itemToDelete.Fines.Any(x => !x.IsPaid))
            {
                // TODO: Change messagebox to label
                MessageBox.Show("Unpaid fines");
                return;
            }
            else if (itemToDelete.UserItems.Any(x => x.BorrowedDate != null && x.ReturnedDate == null))
            {
                // TODO: Change messagebox to label
                MessageBox.Show("Item is currently borrowed");
                return;
            }
            else
            {
                unitOfWork.ItemCategoryRepo.DeleteRange(itemToDelete.ItemCategories);
                unitOfWork.UserItemRepo.DeleteRange(itemToDelete.UserItems);
                unitOfWork.FineRepo.DeleteRange(itemToDelete.Fines);
                unitOfWork.UserInterestedItemRepo.DeleteRange(itemToDelete.UserInterestedItems);
                unitOfWork.ContributorItemRepo.DeleteRange(itemToDelete.ContributorItems);
                unitOfWork.ItemRepo.DeleteEntity(itemToDelete);

                unitOfWork.Save();

                SelectedItem = null;
                FillDataGrid();
            }
        }

        public void FillDataGrid()
        {
            Items = new ObservableCollection<Item>
                          (unitOfWork.ItemRepo.GetEntities(
                     x => x.ContributorItems.Select(z => z.Contributor),
                     x => x.ItemCategories.Select(z => z.Category),
                     x => x.AgeCategory,
                     x => x.UserItems));
        }

        public void FillComboBoxAges()
        {
            Ages = new ObservableCollection<AgeCategory>
                          (unitOfWork.AgeCategoryRepo.GetEntities(
                     x => x.Items));
        }

        public void FillComboBoxCategories()
        {
            Categories = new ObservableCollection<Category>
                          (unitOfWork.CategoryRepo.GetEntities(
                     x => x.ItemCategories));
        }
        public void Search()
        {
            // get all items
            Items = new ObservableCollection<Item>
                          (unitOfWork.ItemRepo.GetEntities(
                         x => x.Title.Contains(BookTitle),
                         x => x.ContributorItems.Select(z => z.Contributor),
                         x => x.ItemCategories.Select(z => z.Category),
                         x => x.AgeCategory,
                         x => x.UserItems));

            // filter by SelectedAge
            if (SelectedAge != null)
            {
                Items = new ObservableCollection<Item>(Items.Where(
                    x => x.AgeCategoryID == SelectedAge.AgeCategoryID));
            }

            // filter by SelectedCategory
            if (SelectedCategory != null)
            {
                Items = new ObservableCollection<Item>(Items.Where(
                    x => x.ItemCategories.Any(z => z.CategoryID == SelectedCategory.CategoryID)));
            }

            // filter by Buyable
            if (Buyable)
            {
                Items = new ObservableCollection<Item>(Items.Where(
                    x => x.IsWithinSaleRange));
            }
            ErrorMessageBookSearch = Items.Count <= 0 ? "Er zijn geen boeken gevonden" : "";
        }

        public void Reset()
        {
            Buyable = false;
            SelectedAge = null;
            SelectedCategory = null;
            SelectedItem = null;
            BookTitle = "";
            ErrorMessageBookSearch = "";
            FillDataGrid();
        }
    }
}
