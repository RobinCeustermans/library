using Bibliotheek_DAL;
using Bibliotheek_DAL.UnitsOfWork;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using The_Boys_Project.Views;

namespace The_Boys_Project.ViewModels
{
    public class CUDItemViewModel : BaseViewModel
    {
        IUnitOfWork unitOfWork = new UnitOfWork(new LibraryEntities());
        public ICommand UpdateViewCommand { get; set; }

        //for the title at the top of the CUDItemView depending if you are creating or deleting
        private string _viewtitle;
        private string _cbContributorTitle;
        private string _cbCategoryTitle;
        private string _errorMessage;
        private DateTime _releaseDate;
        private ObservableCollection<Contributor> _contibutors;
        private Contributor _selectedContributor;
        private ObservableCollection<Category> _categories;
        private ObservableCollection<AgeCategory> _ageCategories;
        private ObservableCollection<ContributorItem> _contributorsItems;
        private ContributorItem _selectedContributorItem;
        private ObservableCollection<ItemCategory> _itemCategories;
        private ItemCategory _selectedItemCategory;
        private Contributor _selectedNewContributor;
        private Category _selectedCategory;
        private ObservableCollection<Contributor> _contributorsNewItems;
        private ObservableCollection<Category> _categoriesNewItems;
        private Category _selectedNewCategory;
        private Item _itemToEdit;
        private DateTime _purchaseDate;
        private int _ageCategoryID;
        private int itemTypeID;
        private string _sellPrice;
        private string _isbn;
        private DateTime _lifeSpan;
        private string _successMessage;
        private string _title;
        private string _desctription;

        public CUDItemViewModel(MainViewModel viewModel, Item itemToEdit)
        {
            this.MainViewModel = viewModel;

            AgeCategories = new ObservableCollection<AgeCategory>(unitOfWork.AgeCategoryRepo.GetEntities(x => x.Items));
            ConfigureItemRecord(itemToEdit);
        }

        public CUDItemViewModel() { }

        public string ViewTitle
        {
            get { return _viewtitle; }
            set
            {
                _viewtitle = value;
                NotifyPropertyChanged();
            }
        }

        public string CbContributorTitle
        {
            get { return _cbContributorTitle; }
            set
            {
                _cbContributorTitle = value;
                NotifyPropertyChanged();
            }
        }

        public string CbCategoryTitle
        {
            get { return _cbCategoryTitle; }
            set
            {
                _cbCategoryTitle = value;
                NotifyPropertyChanged();
            }
        }

        public bool TitleIsToevoegen
        {
            get
            {
                if (ViewTitle == "Bewerk item")
                {
                    CbContributorTitle = "Voeg nog een auteur toe";
                    CbCategoryTitle = "Voeg nog een genre toe";
                    return true;
                }
                else
                {
                    CbContributorTitle = "Voeg een auteur toe bij het nieuwe item";
                    CbCategoryTitle = "Voeg een genre toe bij het nieuwe item";
                    return false;
                }
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

        public string SuccessMessage
        {
            get { return _successMessage; }
            set
            {
                _successMessage = value;
                NotifyPropertyChanged();
            }
        }

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                NotifyPropertyChanged();
            }
        }

        public string Description
        {
            get { return _desctription; }
            set
            {
                _desctription = value;
                NotifyPropertyChanged();
            }
        }

        public string SellPrice
        {
            get { return _sellPrice; }
            set
            {
                _sellPrice = value;
                NotifyPropertyChanged();
            }
        }

        public string ISBN
        {
            get { return _isbn; }
            set
            {
                _isbn = value;
                NotifyPropertyChanged();
            }
        }

        public DateTime ReleaseDate
        {
            get { return _releaseDate; }
            set
            {
                _releaseDate = value;
                NotifyPropertyChanged();
            }
        }

        public DateTime PurchaseDate
        {
            get { return _purchaseDate; }
            set
            {
                _purchaseDate = value;
                NotifyPropertyChanged();
            }
        }

        public DateTime LifeSpan
        {
            get { return _lifeSpan; }
            set
            {
                _lifeSpan = value;
                NotifyPropertyChanged();
            }
        }

        public int AgeCategoryID
        {
            get { return _ageCategoryID; }
            set
            {
                _ageCategoryID = value;
                NotifyPropertyChanged();
            }
        }

        public int ItemTypeID
        {
            get { return itemTypeID; }
            set
            {
                itemTypeID = value;
                NotifyPropertyChanged();
            }
        }

        //comboboxes
        public ObservableCollection<Contributor> Contributors
        {
            get { return _contibutors; }
            set
            {
                _contibutors = value;
                NotifyPropertyChanged();
            }
        }

        public Contributor SelectedContributor
        {
            get { return _selectedContributor; }
            set
            {
                _selectedContributor = value;
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

        public Category SelectedCategory
        {
            get { return _selectedCategory; }
            set
            {
                _selectedCategory = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<AgeCategory> AgeCategories
        {
            get { return _ageCategories; }
            set
            {
                _ageCategories = value;
                NotifyPropertyChanged();
            }
        }

        ////lists when editing exisitng item

        public ObservableCollection<ContributorItem> ContributorItems
        {
            get { return _contributorsItems; }
            set
            {
                _contributorsItems = value;
                NotifyPropertyChanged();
            }
        }

        public ContributorItem SelectedContributItem
        {
            get { return _selectedContributorItem; }
            set
            {
                _selectedContributorItem = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<ItemCategory> ItemCategories
        {
            get { return _itemCategories; }
            set
            {
                _itemCategories = value;
                NotifyPropertyChanged();
            }
        }

        public ItemCategory SelectedItemCategory
        {
            get { return _selectedItemCategory; }
            set
            {
                _selectedItemCategory = value;
                NotifyPropertyChanged();
            }
        }

        ////list when creating new item


        public ObservableCollection<Contributor> ContributorsNewItems
        {
            get { return _contributorsNewItems; }
            set
            {
                _contributorsNewItems = value;
                NotifyPropertyChanged();
            }
        }

        //selected contributor from the list with the contributors of the new item

        public Contributor SelectedNewContributor
        {
            get { return _selectedNewContributor; }
            set
            {
                _selectedNewContributor = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<Category> CategoriesNewItems
        {
            get { return _categoriesNewItems; }
            set
            {
                _categoriesNewItems = value;
                NotifyPropertyChanged();
            }
        }

        //selected category from the list with the contributors of the new item

        public Category SelectedNewCategory
        {
            get { return _selectedNewCategory; }
            set
            {
                _selectedNewCategory = value;
                NotifyPropertyChanged();
            }
        }

        public Item ItemToEdit
        {
            get { return _itemToEdit; }
            set
            {
                _itemToEdit = value;
                NotifyPropertyChanged();
            }
        }

        MainViewModel MainViewModel { get; set; }

        public string Validations()
        {
            string errors = "";
            string isbnError = ValidateISBN(ISBN);
            if (string.IsNullOrWhiteSpace(Title))
                errors += "Titel van het item moet ingevuld zijn" + Environment.NewLine;
            if (string.IsNullOrWhiteSpace(PurchaseDate.ToShortDateString()))
                errors += "Er moet een aankoopdatum geselecteerd zijn" + Environment.NewLine;
            if (string.IsNullOrWhiteSpace(ReleaseDate.ToShortDateString()))
                errors += "Er moet een uitgavedatum geselecteerd zijn" + Environment.NewLine;
            if (!string.IsNullOrWhiteSpace(isbnError))
                errors += isbnError + Environment.NewLine;
            if (AgeCategoryID <= 0)
                errors += "Er moet een leeftijdscategorie geselecteerd zijn " + Environment.NewLine;
            if (string.IsNullOrWhiteSpace(SellPrice) || SellPrice == null)
                errors += "De verkoopprijs van het boek moet ingevuld zijn" + Environment.NewLine;
            if (!string.IsNullOrWhiteSpace(SellPrice) && !decimal.TryParse(SellPrice, out decimal sPrice))
                errors += "De verkoopprijs moet een numerieke waarde hebben" + Environment.NewLine;
            if (decimal.TryParse(SellPrice, out sPrice) && sPrice < 0)
                errors += "De verkoopprijs moet groter zijn dan 0" + Environment.NewLine;
            if (string.IsNullOrWhiteSpace(Description))
                errors += "Beschrijving van het item moet ingevuld zijn" + Environment.NewLine;
            if (string.IsNullOrWhiteSpace(LifeSpan.ToShortDateString()))
                errors += "De vervaldatum moet ingevuld zijn" + Environment.NewLine;

            return errors;
        }

        public override string this[string columnName]
        {
            get
            {
                return "";
            }
        }

        private void ConfigureItemRecord(Item itemToEdit)
        {
            if (itemToEdit != null)
            {
                ViewTitle = "Bewerk item";

                this.ItemToEdit = unitOfWork.ItemRepo.GetEntities(x => x.ContributorItems
                    .Select(y => y.Contributor)
                    , y => y.ItemCategories
                    .Select(z => z.Category))
                    .Where(x => x.ItemID.Equals(itemToEdit.ItemID))
                    .FirstOrDefault();

                Title = ItemToEdit.Title;
                PurchaseDate = ItemToEdit.PurchaseDate;
                ReleaseDate = ItemToEdit.ReleaseDate;
                ISBN = ItemToEdit.ISBN;
                //cb Contributors not tied to item
                Contributors = new ObservableCollection<Contributor>(unitOfWork.ContributorRepo.GetEntities()
                    .Except(ItemToEdit.ContributorItems.Select(x => x.Contributor)));
                //list itemContributors
                ContributorItems = new ObservableCollection<ContributorItem>(unitOfWork.ContributorItemRepo.
                    GetEntities(x => x.ItemID == ItemToEdit.ItemID));
                //cb Categories not tied to item
                Categories = new ObservableCollection<Category>(unitOfWork.CategoryRepo.GetEntities()
                    .Except(ItemToEdit.ItemCategories.Select(x => x.Category)));
                //list ItemCategories
                ItemCategories = new ObservableCollection<ItemCategory>(unitOfWork.ItemCategoryRepo.GetEntities(x => x.ItemID == ItemToEdit.ItemID));
                AgeCategories = new ObservableCollection<AgeCategory>(unitOfWork.AgeCategoryRepo.GetEntities(x => x.Items));
                AgeCategoryID = ItemToEdit.AgeCategoryID;
                SellPrice = ItemToEdit.SellPrice.ToString();
                Description = ItemToEdit.Description;
                LifeSpan = ItemToEdit.LifeSpan;
            }
            else
            {
                Refresh();
            }
        }

        private void Refresh()
        {
            ViewTitle = "Item aanmaken";
            Title = "";
            ISBN = "";
            Description = "";
            ReleaseDate = DateTime.Now.Date.AddYears(-10);
            PurchaseDate = DateTime.Now.Date;
            SellPrice = 5.ToString();
            LifeSpan = DateTime.Now.Date.AddYears(10);
            Contributors = new ObservableCollection<Contributor>(unitOfWork.ContributorRepo.GetEntities());
            Categories = new ObservableCollection<Category>(unitOfWork.CategoryRepo.GetEntities());
            ContributorsNewItems = new ObservableCollection<Contributor>();
            CategoriesNewItems = new ObservableCollection<Category>();
            AgeCategories = new ObservableCollection<AgeCategory>(unitOfWork.AgeCategoryRepo.GetEntities(x => x.Items));
            AgeCategoryID = 0;
        }

        public void AddItem()
        {
            ErrorMessage = Validations();
            if (string.IsNullOrWhiteSpace(ErrorMessage))
            {
                Item newItem = new Item()
                {
                    Title = this.Title,
                    Description = this.Description,
                    ISBN = ReplaceTokens(this.ISBN),
                    SellPrice = decimal.Parse(this.SellPrice),
                    ReleaseDate = this.ReleaseDate.Date,
                    PurchaseDate = this.PurchaseDate,
                    LifeSpan = this.PurchaseDate.AddYears(10).Date,
                    AgeCategoryID = this.AgeCategoryID,
                    ItemTypeID = 1
                };
                if (newItem.IsValid())
                {
                    unitOfWork.ItemRepo.AddEntity(newItem);
                    int ok = unitOfWork.Save();
                    if (ok > 0)
                    {
                        if (ContributorsNewItems.Count > 0)
                        {
                            foreach (var contributor in ContributorsNewItems)
                            {
                                ContributorItem contributorItem = new ContributorItem
                                {
                                    ItemID = newItem.ItemID,
                                    ContributorID = contributor.ContributorID
                                };
                                unitOfWork.ContributorItemRepo.AddEntity(contributorItem);
                                int ciOk = unitOfWork.Save();
                                if (ciOk <= 0) ErrorMessage = "Auteur van Item is niet toegevoegd";
                            }
                        }
                        if (CategoriesNewItems.Count > 0)
                        {
                            foreach (var category in CategoriesNewItems)
                            {
                                ItemCategory itemCategory = new ItemCategory
                                {
                                    ItemID = newItem.ItemID,
                                    CategoryID = category.CategoryID
                                };
                                unitOfWork.ItemCategoryRepo.AddEntity(itemCategory);
                                int icOk = unitOfWork.Save();
                                if (icOk > 0)
                                {
                                    MainViewModel.UpdateViewCommand.Execute(parameter: "Home");
                                }
                                else
                                    ErrorMessage = "Categorie van Item is niet toegevoegd";
                            }
                        }
                        MainViewModel.UpdateViewCommand.Execute(parameter: "Home");
                    }
                    else
                    {
                        ErrorMessage = "Item is niet toegevoegd";
                    }
                }
            }
        }

        public void EditItem()
        {
            if (ItemToEdit != null)
            {
                ErrorMessage = Validations();
                if (string.IsNullOrWhiteSpace(ErrorMessage))
                {
                    ItemToEdit.Title = this.Title;
                    ItemToEdit.PurchaseDate = this.PurchaseDate;
                    ItemToEdit.ReleaseDate = this.ReleaseDate;
                    ItemToEdit.ISBN = ReplaceTokens(this.ISBN);
                    ItemToEdit.AgeCategoryID = this.AgeCategoryID;
                    ItemToEdit.SellPrice = decimal.Parse(this.SellPrice);
                    ItemToEdit.Description = this.Description;
                    ItemToEdit.LifeSpan = this.LifeSpan;

                    if (ItemToEdit.IsValid())
                    {
                        unitOfWork.ItemRepo.EditEntity(ItemToEdit);
                        int ok = unitOfWork.Save();
                        if (ok > 0)
                        {
                            SuccessMessage = "Item is met succes aangepast";
                            ConfigureItemRecord(ItemToEdit);
                        }
                        else
                        {
                            SuccessMessage = "";
                            ErrorMessage = "Item is niet aangepast";
                        }
                    }
                    else
                    {
                        ErrorMessage = "Item is niet aangepast";
                        SuccessMessage = "";
                    }                     
                }
                else SuccessMessage = "";
            }
        }

        public void RefreshContributors()
        {
            ContributorItems = new ObservableCollection<ContributorItem>(unitOfWork.ContributorItemRepo.
                    GetEntities(x => x.ItemID == ItemToEdit.ItemID));
            Contributors = new ObservableCollection<Contributor>(unitOfWork.ContributorRepo.GetEntities()
                   .Except(ItemToEdit.ContributorItems.Select(x => x.Contributor)));
            ErrorMessage = "";
        }

        public void RefreshCategories()
        {
            Categories = new ObservableCollection<Category>(unitOfWork.CategoryRepo.GetEntities()
                .Except(ItemToEdit.ItemCategories.Select(x => x.Category)));
            ItemCategories = new ObservableCollection<ItemCategory>(unitOfWork.ItemCategoryRepo.GetEntities(x => x.ItemID == ItemToEdit.ItemID));
            ErrorMessage = "";
        }

        public void AddContributorToList()
        {
            if (SelectedContributor != null)
            {
                if (ItemToEdit != null)
                {
                    ContributorItem contributorItem = new ContributorItem
                    {
                        ItemID = ItemToEdit.ItemID,
                        ContributorID = SelectedContributor.ContributorID
                    };

                    unitOfWork.ContributorItemRepo.AddEntity(contributorItem);
                    int ok = unitOfWork.Save();
                    if (ok > 0) RefreshContributors();
                    else ErrorMessage = "Auteur is niet toegevoegd bij het item";
                }
                else
                {
                    //for new item
                    if (!ContributorsNewItems.Contains(SelectedContributor))
                    {
                        ContributorsNewItems.Add(SelectedContributor);
                        Contributors = new ObservableCollection<Contributor>(unitOfWork.ContributorRepo.GetEntities()
                             .Except(ContributorsNewItems.Select(x => x)));
                    }
                }
            }
        }

        public void RemoveContributorFromItem()
        {
            if (ItemToEdit != null)
            {
                if (SelectedContributItem != null)
                {
                    unitOfWork.ContributorItemRepo.DeleteEntity(SelectedContributItem);
                    int ok = unitOfWork.Save();
                    if (ok > 0) RefreshContributors();
                    else ErrorMessage = "ContributorItem is niet verwijderd";
                }
            }
            else
            {
                //for new item
                if (SelectedNewContributor != null)
                {
                    ContributorsNewItems.Remove(SelectedNewContributor);
                    Contributors = new ObservableCollection<Contributor>(unitOfWork.ContributorRepo.GetEntities()
                         .Except(ContributorsNewItems.Select(x => x)));
                }
            }
        }

        public void AddCategoryToList()
        {
            if (SelectedCategory != null)
            {
                if (ItemToEdit != null)
                {
                    ItemCategory itemCategory = new ItemCategory
                    {
                        ItemID = ItemToEdit.ItemID,
                        CategoryID = SelectedCategory.CategoryID
                    };
                    unitOfWork.ItemCategoryRepo.AddEntity(itemCategory);
                    int ok = unitOfWork.Save();
                    if (ok > 0) RefreshCategories();
                    else ErrorMessage = "Categorie is niet toegevoegd bij het item";
                }
                else
                {
                    //for new item
                    if (!CategoriesNewItems.Contains(SelectedCategory))
                    {
                        CategoriesNewItems.Add(SelectedCategory);
                        Categories = new ObservableCollection<Category>(unitOfWork.CategoryRepo.GetEntities()
                            .Except(CategoriesNewItems.Select(x => x)));
                    }
                }
            }
        }

        public void RemoveCategoryFromItem()
        {
            if (ItemToEdit != null)
            {
                if (SelectedItemCategory != null)
                {
                    unitOfWork.ItemCategoryRepo.DeleteEntity(SelectedItemCategory);
                    int ok = unitOfWork.Save();
                    if (ok > 0) RefreshCategories();
                    else ErrorMessage = "Categorie is niet verwijderd";
                }
            }
            else
            {
                if (SelectedNewCategory != null)
                {
                    CategoriesNewItems.Remove(SelectedNewCategory);
                    Categories = new ObservableCollection<Category>(unitOfWork.CategoryRepo.GetEntities()
                           .Except(CategoriesNewItems.Select(x => x)));
                }
            }
        }

        public void AddAuthorOrCategory()
        {
            new AuthorCategoryWindowView().ShowDialog();
            MainViewModel.UpdateViewCommand.Execute(parameter: "Aanpassen");
        }

        public override bool CanExecute(object parameter)
        {
            if (parameter.ToString() == "Cancel")
            {
                return true;
            }
            else if (parameter.ToString() == "AddItem")
            {
                return true;
            }
            else if (parameter.ToString() == "AddContributorToList")
            {
                return SelectedContributor != null;
            }
            else if (parameter.ToString() == "RemoveContributorFromItem")
            {
                return SelectedContributItem != null || SelectedNewContributor != null;
            }
            else if (parameter.ToString() == "AddCategoryToList")
            {
                return SelectedCategory != null;
            }
            else if (parameter.ToString() == "RemoveCategoryFromItem")
            {
                return SelectedItemCategory != null || SelectedNewCategory != null;
            }
            else if (parameter.ToString() == "EditItem")
            {
                return true;
            }
            else if (parameter.ToString() == "AddAuthorOrCategory")
            {
                return true;
            }
            return true;
        }

        public override void Execute(object parameter)
        {
            if (parameter.ToString() == "AddItem")
            {
                AddItem();
            }
            else if (parameter.ToString() == "EditItem")
            {
                EditItem();
            }
            else if (parameter.ToString() == "Cancel")
            {
                Cancel();
            }
            else if (parameter.ToString() == "AddContributorToList")
            {
                AddContributorToList();
            }
            else if (parameter.ToString() == "RemoveContributorFromItem")
            {
                RemoveContributorFromItem();
            }
            else if (parameter.ToString() == "AddCategoryToList")
            {
                AddCategoryToList();
            }
            else if (parameter.ToString() == "RemoveCategoryFromItem")
            {
                RemoveCategoryFromItem();
            }
            else if (parameter.ToString() == "AddAuthorOrCategory")
            {
                AddAuthorOrCategory();
            }
            else if (parameter.ToString() == "MakeBuyable")
            {
                MakeBuyable();
            }
        }

        private void MakeBuyable()
        {
            if (SellPrice != null && decimal.TryParse(SellPrice, out Decimal sellprice))
            {
                if (sellprice > 0)
                {
                    this.ItemToEdit.LifeSpan = DateTime.Now.AddMonths(1);
                    this.ItemToEdit.SellPrice = sellprice;
                    unitOfWork.ItemRepo.EditEntity(this.ItemToEdit);
                    int ok = unitOfWork.Save();
                    if (ok >= 1)
                    {
                        MessageBox.Show("Item is nu koopbaar");
                    }
                    else MessageBox.Show("Er ging iets fout");
                }
                else MessageBox.Show("Het veld verkoopprijs mag niet kleiner of gelijk aan 0 zijn!");
            }
            else MessageBox.Show("Het veld verkoopprijs moet een numerieke waarde bevatten");
        }

        public string ValidateISBN(string input)
        {
            ////invalid isbn
            //:978 - 1 - 86197 - 876 -
            string error = "";

            if (!string.IsNullOrWhiteSpace(input))
            {
                input = ReplaceTokens(input);
                if (input.Length == 10)
                    error = ValidateISBN10(input);
                else if (input.Length == 13)
                    error = ValidateISBN13(input);
                else error = "Een ISBN moet 10 of 13 karakters bevatten";
            }
            else error = "ISBN van het item moet ingevuld zijn";

            return error;
        }

        private string ValidateISBN10(string isbn10)
        {
            ////valid isbn 10: 
            //0-19-852663-6
            //-0 * 19 - 852$663 - 6
            // 0 * 19-- */ -*//  *** **-*-* 85    2$6   63-  -)6*/**-  
            // 043942089X

            ////invalid isbn 10:
            //043942089z
            //019852663

            string error = "";
            if (!string.IsNullOrWhiteSpace(isbn10))
            {
                int sum = 0;

                if (long.TryParse(isbn10.Substring(0, isbn10.Length - 1), out long j))
                {
                    string lastLetter = isbn10.Substring(isbn10.Length - 1);

                    // Using the alternative way of calculation

                    for (int i = 0; i < 9; i++)
                    {
                        sum += int.Parse(isbn10[i].ToString()) * (i + 1);
                    }

                    // Getting the remainder or the checkdigit
                    int remainder = sum % 11;

                    if (lastLetter.ToLower() == "x")
                    {
                        if (remainder != 10)
                            error = "Ongeligde ISBN";
                    }
                    // Otherwise check if the lastChar is numeric
                    else if (int.TryParse(lastLetter.ToString(), out sum))
                    {
                        // lastChar is numeric, so let's compare it to remainder
                        if (remainder != int.Parse(lastLetter.ToString()))
                            error = "Ongeldige ISBN";
                    }
                    else error = "Het laatste karakter van een ISBN-10 mag enkel een x of een cijfer zijn";
                }
                else error = "Een ISBN-10 mag geen letters betvatten, behalve een x op het einde";
            }
            return error;
        }

        private string ValidateISBN13(string isbn13)
        {
            ////valid isbn 13
            //:978-1-86197-876-9
            //97/*--81 ) 861  *  97,;:ùµ$2    71éè&§"è&é2

            ////invalid:
            //hgh - g - ggggg - ggg - g
            //978 - 1 - 86197 - 876 - 2
            //97!è§8 - 1 - 861('"é&97-8"é&²76-2&é

            string error = "";
            int sum = 0;
            // if it contains any non numeric chars, then error
            if (!string.IsNullOrWhiteSpace(isbn13))
            {
                if (long.TryParse(isbn13, out long j))
                {
                    //for the first 12 digits this method multiplies from left to right each digit by 1 or 3, beginning with 1
                    for (int i = 0; i < 12; i++)
                    {
                        //those product are summed up
                        sum += int.Parse(isbn13[i].ToString()) * (i % 2 == 1 ? 3 : 1);
                    }

                    //and then modulo 10 (ex. 118 modulo 10 = 110 remainder 8)
                    int remainder = sum % 10;
                    //10 minus the remainder
                    int checkDigit = 10 - remainder;
                    //if the remainder is 0, then the check digit also becomes 0
                    if (checkDigit == 10)
                        checkDigit = 0;
                    //check if the checkdigit is equal to the last number of your isbn, it is a valid isbn if they are equal.
                    if (checkDigit != int.Parse(isbn13[12].ToString()))
                        error = "Ongelige ISBN-13";
                }
                else error = "Een ISBN-13 mag geen letters bevatten";
            }
            return error;
        }

        private string ReplaceTokens(string str)
        {
            //char[] separators = new char[] { ' ', '-', ':', '*', '_', ';', ',', '\r', '\t', '\n' };
            //string[] temp = str.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            //return str = String.Join("", temp);
            str = Regex.Replace(str, "[^0-9a-zA-Z]", "");
            return str;
        }

        public void Dispose()
        {
            unitOfWork?.Dispose();
        }

        private void Cancel()
        {
            MainViewModel.UpdateViewCommand.Execute(parameter: "Home");
        }
    }
}