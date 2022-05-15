using Bibliotheek_DAL.DomainModels;
using Bibliotheek_DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheek_DAL.UnitsOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<AgeCategory> AgeCategoryRepo { get; }
        IRepository<BookFair> BookFairRepo { get; }
        IRepository<Category> CategoryRepo { get; }
        IRepository<Contributor> ContributorRepo { get; }
        IRepository<ContributorItem> ContributorItemRepo { get; }
        IRepository<ContributorType> ContributorTypeRepo { get; }
        IRepository<EmailText> EmailTextRepo { get; }
        IRepository<Fine> FineRepo { get; }
        IRepository<Item> ItemRepo { get; }
        IRepository<ItemCategory> ItemCategoryRepo { get; }
        IRepository<ItemType> ItemTypeRepo { get; }
        IRepository<MembershipType> MembershipTypeRepo { get; }
        IRepository<User> UserRepo { get; }
        IRepository<UserBookFair> UserBookFairRepo { get; }
        IRepository<UserInterestedItem> UserInterestedItemRepo { get; }
        IRepository<UserItem> UserItemRepo { get; }
        int Save();
    }
}
