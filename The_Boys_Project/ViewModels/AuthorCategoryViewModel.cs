using Bibliotheek_DAL;
using Bibliotheek_DAL.UnitsOfWork;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace The_Boys_Project.ViewModels
{
    public class AuthorCategoryViewModel : BaseViewModel
    {
        IUnitOfWork unitOfWork = new UnitOfWork(new LibraryEntities());
        MainViewModel MainViewModel { get; set; }

        //atributes contributor
        private ObservableCollection<Contributor> _contributors;
        private string _searchTermContributor;
        private string _errorSearchContributorMessage;
        private Contributor _contributorToEdit;
        private string _contributorErrorMessage;
        private string _contributorSuccessMessage;
        private bool _contributorErrorMessageVisible;
        private bool _contributorSuccessMessageVisible;

        private string _name;
        private string _surname;

        private ObservableCollection<ContributorItem> _contributorItemsToDelete;

        //attributes for category
        private ObservableCollection<Category> _categories;
        private string _searchTermCategory;
        private string _errorSearchCategoryMessage;
        private Category _categoryToEdit;
        private string _categoryErrorMessage;
        private string _categorySuccessMessage;
        private bool _categoryErrorMessageVisible;
        private bool _categorySuccessMessageVisible;

        private string _categoryName;

        private ObservableCollection<ItemCategory> _itemCategoriesToDelete;

        public AuthorCategoryViewModel(MainViewModel mainViewModel)
        {
            this.MainViewModel = mainViewModel;
            Contributors = new ObservableCollection<Contributor>(unitOfWork.ContributorRepo.GetEntities());
            Categories = new ObservableCollection<Category>(unitOfWork.CategoryRepo.GetEntities());

            ConfigureContributorRecord();
            ClearContributorMessages();
            SearchTermContributor = "";
            ErrorSearchContributorMessage = "";

            ConfigureCategoryRecord();
            ClearCategoryMessage();
            SearchTermCategory = "";
            ErrorSearchCategoryMessage = "";
        }

        //messages for contributor       

        public string ContributorErrorMessage
        {
            get { return _contributorErrorMessage; }
            set
            {
                _contributorErrorMessage = value;
                ToggleVisibilityContributorLabels(false);
                NotifyPropertyChanged();
            }
        }

        public string ContributorSuccessMessage
        {
            get { return _contributorSuccessMessage; }
            set
            {                
                _contributorSuccessMessage = value;
                ToggleVisibilityContributorLabels(true);
                NotifyPropertyChanged();         
            }
        }

        public bool ContributorErrorMessageVisible
        {
            get { return _contributorErrorMessageVisible; }
            set 
            {
                _contributorErrorMessageVisible = value;
                NotifyPropertyChanged();
            }
        }

        public bool ContributorSuccessMessageVisible
        {
            get { return _contributorSuccessMessageVisible; }
            set
            {
                _contributorSuccessMessageVisible = value;
                NotifyPropertyChanged();
            }
        }

        //messages for category

        public string CategoryErrorMessage
        {
            get { return _categoryErrorMessage; }
            set
            {
                _categoryErrorMessage = value;
                ToggleVisibilityCategoryLabels(false);
                NotifyPropertyChanged();
            }
        }
      
        public string CategorySuccessMessage
        {
            get { return _categorySuccessMessage; }
            set 
            { 
                _categorySuccessMessage = value;
                ToggleVisibilityCategoryLabels(true);
                NotifyPropertyChanged();
            }
        }

        public bool CategoryErrorMessageVisible
        {
            get { return _categoryErrorMessageVisible; }
            set 
            {
                _categoryErrorMessageVisible = value;
                NotifyPropertyChanged();
            }
        }

        public bool CategorySuccessMessageVisible
        {
            get { return _categorySuccessMessageVisible; }
            set
            { 
                _categorySuccessMessageVisible = value;
                NotifyPropertyChanged();
            }
        }
   
        //properties contributor

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                NotifyPropertyChanged();
            }
        }
        public string Surname
        {
            get { return _surname; }
            set
            {
                _surname = value;
                NotifyPropertyChanged();
            }
        }

        //propeties category

        public string CategoryName
        {
            get { return _categoryName; }
            set 
            {
                _categoryName = value;
                NotifyPropertyChanged();
            }
        }

        public void Dispose()
        {
            unitOfWork?.Dispose();
        }

        //datagrid of contributors        

        public ObservableCollection<Contributor> Contributors
        {
            get { return _contributors; }
            set
            {
                _contributors = value;
                NotifyPropertyChanged();
            }
        }
        public string SearchTermContributor
        {
            get { return _searchTermContributor; }
            set
            {
                _searchTermContributor = value;
                NotifyPropertyChanged();
            }
        }

        public string ErrorSearchContributorMessage
        {
            get { return _errorSearchContributorMessage; }
            set
            {
                _errorSearchContributorMessage = value;
                NotifyPropertyChanged();
            }
        }

        public void SearchContributor()
        {
            ClearContributorMessages();
            ContributorToEdit = null;
            Contributors = new ObservableCollection<Contributor>
                (unitOfWork.ContributorRepo.GetEntities(x => x.Surname.Contains(SearchTermContributor) || x.Name.Contains(SearchTermContributor)));
            ErrorSearchContributorMessage = Contributors.Count() == 0 ? "Er zijn geen auteurs gevonden die overeenkomen met de zoekopdracht" : "";
        }

        //datagrid of categories

        public ObservableCollection<Category> Categories
        {
            get { return _categories; }
            set 
            {
                _categories = value;
                NotifyPropertyChanged();
            }
        }

        public string SearchTermCategory
        {
            get { return _searchTermCategory; }
            set 
            {
                _searchTermCategory = value;
                NotifyPropertyChanged();
            }
        }
      
        public string ErrorSearchCategoryMessage
        {
            get { return _errorSearchCategoryMessage; }
            set
            {
                _errorSearchCategoryMessage = value;
                NotifyPropertyChanged();
            }
        }

        public void SearchCategory()
        {
            ClearCategoryMessage();
            CategoryToEdit = null;
            Categories = new ObservableCollection<Category>(unitOfWork.CategoryRepo.GetEntities(x => x.Name.Contains(SearchTermCategory)));
            ErrorSearchCategoryMessage = Categories.Count() == 0 ? "Er zijn geen categorieën gevonden die overeenkomen met de zoekopdracht" : "";
        }

        //Contributor CUD
        
        public Contributor ContributorToEdit
        {
            get { return _contributorToEdit; }
            set
            {
                _contributorToEdit = value;
                ConfigureContributorRecord();
                NotifyPropertyChanged();
                if (value != null)
                {
                    ClearContributorMessages();
                }            
            }
        }
   
        private void ConfigureContributorRecord()
        {
            if (ContributorToEdit != null)
            {
                Name = ContributorToEdit.Name;
                Surname = ContributorToEdit.Surname;
            }
        }

        public string ContributorValidations()
        {
            List<string> errorList = new List<string>();        
            string errors = "";
            
            if (string.IsNullOrWhiteSpace(Name))
            {
                errorList.Add("Achternaam van de auteur moet ingevuld zijn.");
            }
            if (string.IsNullOrWhiteSpace(Surname))
            {
                errorList.Add("Voornaam van de auteur moet ingevuld zijn.");              
            }

            foreach (var item in errorList)
            {
                errors += item != errorList.LastOrDefault() ? item + Environment.NewLine : item;
            }
            return errors;
        }

        private void AddContributor()
        {
            ContributorErrorMessage = ContributorValidations();
            if (string.IsNullOrWhiteSpace(ContributorErrorMessage))
            {
                if (ContributorToEdit == null)
                {
                    Contributor contributor = new Contributor
                    {
                        Name = this.Name,
                        Surname = this.Surname,
                        ContributorTypeID = 1
                    };
                    if (contributor.IsValid())
                    {
                        unitOfWork.ContributorRepo.AddEntity(contributor);
                        int ok = unitOfWork.Save();
                        if (ok > 0)
                        {
                            Contributors = new ObservableCollection<Contributor>(unitOfWork.ContributorRepo.GetEntities());
                            ToggleVisibilityContributorLabels(true);
                            ContributorSuccessMessage = "Auteur is met succes toegevoegd";
                            ResetContributor(false);
                        }
                        else 
                        {
                            ToggleVisibilityContributorLabels(false);
                            ContributorErrorMessage = "Auteur is niet toegevoegd";                         
                        }
                    }
                }
                else
                {
                    ToggleVisibilityContributorLabels(false);
                    ContributorErrorMessage = "Deselecteer de huidige auteur om een nieuwe toe te voegen door te refeshen";
                }
            }
        }

        private void EditContributor()
        {
            if (ContributorToEdit != null)
            {
                ContributorErrorMessage = ContributorValidations();
                if (string.IsNullOrWhiteSpace(ContributorErrorMessage))
                {
                    ContributorToEdit.Name = this.Name;
                    ContributorToEdit.Surname = this.Surname;
                    if (ContributorToEdit.IsValid())
                    {
                        unitOfWork.ContributorRepo.EditEntity(ContributorToEdit);
                        int ok = unitOfWork.Save();
                        if (ok > 0)
                        {
                            ToggleVisibilityContributorLabels(true);
                            ContributorSuccessMessage = "Auteur is aangepast";
                            ResetContributor(false);
                        }
                        else
                        {
                            ToggleVisibilityContributorLabels(false);
                            ContributorErrorMessage = "Auteur is niet aangepast";
                        }
                    }
                }
            }
            else 
            {
                ToggleVisibilityContributorLabels(false);
                ContributorErrorMessage = "Selecteer een bestaande auteur om te bewerken";
            }
        }

        public ObservableCollection<ContributorItem> ContributorItemsToDelete
        {
            get { return _contributorItemsToDelete; }
            set
            {
                _contributorItemsToDelete = value;
                NotifyPropertyChanged();
            }
        }

        private void DeleteContributor()
        {
            if (ContributorToEdit != null)
            {
                ContributorItemsToDelete = new ObservableCollection<ContributorItem>
                                               (unitOfWork.ContributorItemRepo.GetEntities
                                               (x => x.ContributorID == ContributorToEdit.ContributorID));
                if (ContributorItemsToDelete.Count > 0)
                {
                    DialogResult dialogResult = System.Windows.Forms.MessageBox.Show(
                        "Weet u zeker dat u deze auteur wilt verwijderen, deze auteur heeft namelijk 1 of meer boeken onder zijn naam",
                        "Bevestiging verwijderen", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        foreach (var item in ContributorItemsToDelete)
                        {
                            unitOfWork.ContributorItemRepo.DeleteEntity(item);
                        }
                        int ok = unitOfWork.Save();
                        DeleteAuthorWithoutItem();
                    }
                }
                else if (ContributorItemsToDelete.Count == 0)
                {
                    DeleteAuthorWithoutItem();
                }
            }
            else 
            {
                ToggleVisibilityContributorLabels(false);
                ContributorErrorMessage = "Selecteer een auteur om te verwijderen";
            } 
        }

        private void DeleteAuthorWithoutItem()
        {
            unitOfWork.ContributorRepo.DeleteEntity(ContributorToEdit);
            int okContributor = unitOfWork.Save();
            if (okContributor > 0)
            {
                ResetContributor(false);
                Contributors = new ObservableCollection<Contributor>(unitOfWork.ContributorRepo.GetEntities());
                ToggleVisibilityContributorLabels(true);
                ContributorSuccessMessage = "Auteur is verwijderd";
            }
            else
            {
                ToggleVisibilityContributorLabels(false);
                ContributorErrorMessage = "Auteur is niet verwijderd";
            } 
        }

        //Category CUD

        public Category CategoryToEdit
        {
            get { return _categoryToEdit; }
            set
            {
                _categoryToEdit = value;
                ConfigureCategoryRecord();
                NotifyPropertyChanged();
                if (value != null)
                {
                    ClearCategoryMessage();
                }              
            }
        }

        private void ConfigureCategoryRecord()
        {
            if (CategoryToEdit != null) CategoryName = CategoryToEdit.Name;
        }

        public string CategoryValidations()
        {
            string errors = "";
            return errors = (string.IsNullOrWhiteSpace(CategoryName)) ? "De beschrijving van de categorie moet ingevuld te zijn": "";
        }

        public void AddCategory()
        {
            CategoryErrorMessage = CategoryValidations();
            if (string.IsNullOrWhiteSpace(CategoryErrorMessage))
            {
                if (CategoryToEdit == null)
                {
                    Category category = new Category { Name = this.CategoryName };
                    if (!Categories.Contains(category))
                    {
                        if (category.IsValid())
                        {
                            unitOfWork.CategoryRepo.AddEntity(category);
                            int ok = unitOfWork.Save();
                            if (ok > 0)
                            {
                                Categories = new ObservableCollection<Category>(unitOfWork.CategoryRepo.GetEntities());
                                ToggleVisibilityCategoryLabels(true);
                                CategorySuccessMessage = "Categorie is met succes toegevoegd";
                                ResetCategory(false);
                            }
                            else
                            {
                                ToggleVisibilityCategoryLabels(false);
                                CategoryErrorMessage = "Categorie is niet toegevoegd";
                            }
                        }
                    }                           
                }
                else
                {
                    ToggleVisibilityCategoryLabels(false);
                    CategoryErrorMessage = "Deselecteer de huidige categorie om een nieuwe toe te voegen door te refeshen";
                }
            }
        }

        public void EditCategory()
        {
            if (CategoryToEdit != null)
            {
                CategoryErrorMessage = CategoryValidations();
                if (string.IsNullOrWhiteSpace(CategoryErrorMessage))
                {
                    CategoryToEdit.Name = this.CategoryName;
                    if (CategoryToEdit.IsValid())
                    {
                        unitOfWork.CategoryRepo.EditEntity(CategoryToEdit);
                        int ok = unitOfWork.Save();
                        if (ok > 0) 
                        {
                            ToggleVisibilityCategoryLabels(true);
                            CategorySuccessMessage = "Categorie is aangepast";
                            ResetCategory(false);
                        } 
                        else
                        {
                            ToggleVisibilityCategoryLabels(false);
                            CategoryErrorMessage = "Deze categorie bestaal";
                        }
                    }
                }
            }
            else
            {
                ToggleVisibilityCategoryLabels(false);
                CategoryErrorMessage = "Selecteer een categorie om te bewerken";
            }         
        }

        public ObservableCollection<ItemCategory> ItemCategoriesToDelete
        {
            get { return _itemCategoriesToDelete; }
            set
            {
                _itemCategoriesToDelete = value;
                NotifyPropertyChanged();
            }
        }

        public void DeleteCategory()
        {
            if (CategoryToEdit != null)
            {
                ItemCategoriesToDelete = new ObservableCollection<ItemCategory>
                                             (unitOfWork.ItemCategoryRepo.GetEntities
                                             (x => x.CategoryID == CategoryToEdit.CategoryID));
                if (ItemCategoriesToDelete.Count() > 0)
                {
                    DialogResult dialogResult = System.Windows.Forms.MessageBox.Show(
                        "Weet u zeker dat u deze categorie wilt verwijderen, deze categorie heeft namelijk 1 of meer boeken met deze categorie",
                        "Bevestiging verwijderen", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        foreach (var item in ItemCategoriesToDelete)
                        {
                            unitOfWork.ItemCategoryRepo.DeleteEntity(item);
                        }
                        int ok = unitOfWork.Save();
                        DeleteCategoryWithoutItem();
                    }
                }
                else if (ItemCategoriesToDelete.Count() == 0)
                {
                    DeleteCategoryWithoutItem();
                }
            }
            else 
            {
                ToggleVisibilityCategoryLabels(false);
                CategoryErrorMessage = "Selecteer een categorie om te verwijderen";
            }          
        }

        public void DeleteCategoryWithoutItem()
        {
            unitOfWork.CategoryRepo.DeleteEntity(CategoryToEdit);
            int okCategory = unitOfWork.Save();
            if (okCategory > 0)
            {
                ResetCategory(false);
                Categories = new ObservableCollection<Category>(unitOfWork.CategoryRepo.GetEntities());
                ToggleVisibilityCategoryLabels(true);
                CategorySuccessMessage = "Categorie is verwijderd";
            }
            else 
            {
                ToggleVisibilityCategoryLabels(false);
                CategoryErrorMessage = "Categorie is niet verwijderd";
            }
        }

        ////helper methods

        //contributor

        public void ResetContributor(bool clearMessagefield)
        {
            if (this.IsValid())
            {             
                Surname = null;
                Name = null;
                if (ContributorToEdit != null)
                {
                    ContributorToEdit = null;
                }
                if (clearMessagefield)
                {
                    ClearContributorMessages();
                }                         
            }
        }

        public void ToggleVisibilityContributorLabels(bool success)
        {
            if (success)
            {
                ContributorSuccessMessageVisible = true;
                ContributorErrorMessageVisible = false;
            }
            else
            {
                ContributorSuccessMessageVisible = false;
                ContributorErrorMessageVisible = true;
            }
        }

        public void ClearContributorMessages()
        {
            ContributorErrorMessage = "";
            ContributorSuccessMessage = "";
            ToggleVisibilityContributorLabels(true);
        }

        //category

        public void ResetCategory(bool clearMessagefield)
        {
            if (this.IsValid())
            {
                CategoryName = null;
                if (CategoryToEdit != null)
                {
                    CategoryToEdit = null;
                }
                if (clearMessagefield)
                {
                    ClearCategoryMessage();
                }    
            }
        }
        public void ToggleVisibilityCategoryLabels(bool success)
        {
            if (success)
            {
                CategorySuccessMessageVisible = true;
                CategoryErrorMessageVisible = false;
            }
            else
            {
                CategorySuccessMessageVisible = false;
                CategoryErrorMessageVisible = true;
            }
        }

        public void ClearCategoryMessage()
        {
            CategoryErrorMessage = "";
            CategorySuccessMessage = "";
            ToggleVisibilityCategoryLabels(true);
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
            // check if searchterms are empty?
            return true;
        }

        public override void Execute(object parameter)
        {
            if (parameter.ToString() == "SearchContributor")
            {
                SearchContributor();
            }
            else if (parameter.ToString() == "AddContributor")
            {
                AddContributor();
            }
            else if (parameter.ToString() == "ResetContributor")
            {
                ResetContributor(true);
            }
            else if (parameter.ToString() == "EditContributor")
            {
                EditContributor();
            }
            else if (parameter.ToString() == "DeleteContributor")
            {
                DeleteContributor();
            }
            else if (parameter.ToString() == "SearchCategory")
            {
                SearchCategory();
            }
            else if (parameter.ToString() == "ResetCategory")
            {
                ResetCategory(true);
            }
            else if (parameter.ToString() == "AddCategory")
            {
                AddCategory();
            }
            else if (parameter.ToString() == "EditCategory")
            {
                EditCategory();
            }
            else if (parameter.ToString() == "DeleteCategory")
            {
                DeleteCategory();
            }
        }
    }
}
