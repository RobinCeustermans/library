using Bibliotheek_DAL.DomainModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheek_DAL
{
    public class LibraryEntities: DbContext
    {
        public LibraryEntities(): base ("LibraryDB")
        {

        }

        public DbSet<AgeCategory> AgeCategories { get; set; }
        public DbSet<BookFair> BookFairs { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Contributor> Contributors { get; set; }
        public DbSet<ContributorItem> ContributorItems { get; set; }
        public DbSet<ContributorType> ContributorTypes { get; set; }
        public DbSet<EmailText> EmailTexts { get; set; }
        public DbSet<Fine> Fines { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemCategory> ItemCategories { get; set; }
        public DbSet<ItemType> ItemTypes { get; set; }
        public DbSet<MembershipType> MembershipTypes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserBookFair> UserBookFairs { get; set; }
        public DbSet<UserInterestedItem> UserInterestedItems { get; set; }
        public DbSet<UserItem> UserItems { get; set; }
    }
}
