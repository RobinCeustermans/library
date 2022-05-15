using Bibliotheek_DAL.DomainModels;
using Bibliotheek_DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheek_DAL.UnitsOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private IRepository<AgeCategory> _ageCategoryRepo;
        private IRepository<BookFair> _bookFairRepo;
        private IRepository<Category> _categoryRepo;
        private IRepository<Contributor> _contributorRepo;
        private IRepository<ContributorItem> _contributorItemRepo;
        private IRepository<ContributorType> _contributorTypeRepo;
        private IRepository<EmailText> _emailTextRepo;
        private IRepository<Fine> _fineRepo;
        private IRepository<Item> _itemRepo;
        private IRepository<ItemCategory> _itemCategoryRepo;
        private IRepository<ItemType> _itemTypeRepo;
        private IRepository<MembershipType> _membershipTypeRepo;
        private IRepository<User> _userRepo;
        private IRepository<UserBookFair> _userBookFairRepo;
        private IRepository<UserInterestedItem> _userInterestedItemRepo;
        private IRepository<UserItem> _userItemRepo;

        private LibraryEntities LibraryEntities { get; }

        public UnitOfWork(LibraryEntities libraryEntities)
        {
            this.LibraryEntities = libraryEntities;
        }

        public IRepository<EmailText> EmailTextRepo
        {
            get
            {
                if (_emailTextRepo == null)
                    _emailTextRepo = new Repository<EmailText>(this.LibraryEntities);
                return _emailTextRepo;
            }
            set { _emailTextRepo = value; }
        }

        public IRepository<UserItem> UserItemRepo
        {
            get
            {
                if (_userItemRepo == null) 
                    _userItemRepo = new Repository<UserItem>(this.LibraryEntities);
                return _userItemRepo;
            }
            set { _userItemRepo = value; }
        }


        public IRepository<UserInterestedItem> UserInterestedItemRepo
        {
            get
            {
                if (_userInterestedItemRepo == null)
                    _userInterestedItemRepo = new Repository<UserInterestedItem>(this.LibraryEntities);
                return _userInterestedItemRepo;
            }
            set { _userInterestedItemRepo = value; }
        }


        public IRepository<UserBookFair> UserBookFairRepo
        {
            get
            {
                if (_userBookFairRepo == null)
                    _userBookFairRepo = new Repository<UserBookFair>(this.LibraryEntities);
                return _userBookFairRepo;
            }
            set { _userBookFairRepo = value; }
        }


        public IRepository<User> UserRepo
        {
            get
            {
                if (_userRepo == null)
                    _userRepo = new Repository<User>(this.LibraryEntities);
                return _userRepo;
            }
            set { _userRepo = value; }
        }


        public IRepository<MembershipType> MembershipTypeRepo
        {
            get
            {
                if (_membershipTypeRepo == null)
                    _membershipTypeRepo = new Repository<MembershipType>(this.LibraryEntities);
                return _membershipTypeRepo;
            }
            set { _membershipTypeRepo = value; }
        }


        public IRepository<ItemType> ItemTypeRepo
        {
            get
            {
                if (_itemTypeRepo == null)
                    _itemTypeRepo = new Repository<ItemType>(this.LibraryEntities);
                return _itemTypeRepo;
            }
            set { _itemTypeRepo = value; }
        }


        public IRepository<ItemCategory> ItemCategoryRepo
        {
            get
            {
                if (_itemCategoryRepo == null)
                    _itemCategoryRepo = new Repository<ItemCategory>(this.LibraryEntities);
                return _itemCategoryRepo;
            }
            set { _itemCategoryRepo = value; }
        }


        public IRepository<Item> ItemRepo
        {
            get
            {
                if (_itemRepo == null)
                    _itemRepo = new Repository<Item>(this.LibraryEntities);
                return _itemRepo;
            }
            set { _itemRepo = value; }
        }


        public IRepository<Fine> FineRepo
        {
            get
            {
                if (_fineRepo == null)
                    _fineRepo = new Repository<Fine>(this.LibraryEntities);
                return _fineRepo;
            }
            set { _fineRepo = value; }
        }


        public IRepository<ContributorType> ContributorTypeRepo
        {
            get
            {
                if (_contributorTypeRepo == null)
                    _contributorTypeRepo = new Repository<ContributorType>(this.LibraryEntities);
                return _contributorTypeRepo;
            }
            set { _contributorTypeRepo = value; }
        }


        public IRepository<ContributorItem> ContributorItemRepo
        {
            get
            {
                if (_contributorItemRepo == null)
                    _contributorItemRepo = new Repository<ContributorItem>(this.LibraryEntities);
                return _contributorItemRepo;
            }
            set { _contributorItemRepo = value; }
        }


        public IRepository<Contributor> ContributorRepo
        {
            get
            {
                if (_contributorRepo == null)
                    _contributorRepo = new Repository<Contributor>(this.LibraryEntities);
                return _contributorRepo;
            }
            set { _contributorRepo = value; }
        }


        public IRepository<Category> CategoryRepo
        {
            get
            {
                if (_categoryRepo == null)
                    _categoryRepo = new Repository<Category>(this.LibraryEntities);
                return _categoryRepo;
            }
            set { _categoryRepo = value; }
        }


        public IRepository<BookFair> BookFairRepo
        {
            get
            {
                if (_bookFairRepo == null)
                    _bookFairRepo = new Repository<BookFair>(this.LibraryEntities);
                return _bookFairRepo;
            }
            set { _bookFairRepo = value; }
        }


        public IRepository<AgeCategory> AgeCategoryRepo
        {
            get
            {
                if (_ageCategoryRepo == null)
                    _ageCategoryRepo = new Repository<AgeCategory>(this.LibraryEntities);
                return _ageCategoryRepo;
            }
            set { _ageCategoryRepo = value; }
        }

        public void Dispose()
        {
            LibraryEntities.Dispose();
        }

        public int Save()
        {
            return LibraryEntities.SaveChanges();
        }
    }
}
