using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheek_DAL
{
    public partial class Item
    {
        public int ItemID { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public string ISBN { get; set; }
        [Required]
        public decimal SellPrice { get; set; }
        [Required]
        public DateTime ReleaseDate { get; set; }
        [Required]
        public DateTime PurchaseDate { get; set; }
        [Required]
        public DateTime LifeSpan { get; set; }
        [Required]
        public int AgeCategoryID { get; set; }
        [Required]
        public int ItemTypeID { get; set; }

        // navigation props
        public ICollection<UserInterestedItem> UserInterestedItems { get; set; }
        public ICollection<UserItem> UserItems { get; set; }
        public ICollection<ContributorItem> ContributorItems { get; set; }
        public ICollection<ItemCategory> ItemCategories { get; set; }
        public ICollection<Fine> Fines { get; set; }
        public ItemType ItemType { get; set; }
        public AgeCategory AgeCategory { get; set; }
    }
}
