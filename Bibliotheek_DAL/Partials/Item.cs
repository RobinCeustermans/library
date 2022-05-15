using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheek_DAL
{
    [AddINotifyPropertyChangedInterface]
    public partial class Item : Baseclass
    {
        public override string this[string columnName]
        {
            get
            {
                if (columnName == "Title" && string.IsNullOrWhiteSpace(Title))
                {
                    return "De titel is een verplicht veld!";
                }
                if (columnName == "PurchaseDate" && PurchaseDate == null)
                {
                    return "Aankoopdatum is een verplicht veld!";
                }
                if (columnName == "ReleaseDate" && ReleaseDate == null)
                {
                    return "Uitgavedatum is een verplicht veld";
                }
                if (columnName == "ISBN" && string.IsNullOrWhiteSpace(ISBN))
                {
                    return "ISBN is een verplicht veld";
                }
                if (columnName == "AgeCategoryID" && AgeCategoryID <= 0)
                {
                    return "Selecteer een leeftijdscategorie";
                }
                if (columnName == "Description" && string.IsNullOrWhiteSpace(Description))
                {
                    return "ISBN is een verplicht veld";
                }
                if (columnName == "SellPrice" && SellPrice < 0)
                {
                    return "De verkoopprijs moet groter zijn dan 0";
                }

                return "";
            }
        }

        public override bool Equals(object obj)
        {
            return obj is Item item &&
                   ItemID == item.ItemID;
        }

        public override int GetHashCode()
        {
            return -1218291565 + ItemID.GetHashCode();
        }
        public string ContributorsToString
        {
            get
            {
                string output = "";
                foreach (var item in this.ContributorItems)
                {
                    output += item.Contributor.Surname + " " + item.Contributor.Name;
                    if (item != this.ContributorItems.LastOrDefault()) output += Environment.NewLine;
                }
                return output;
            }
        }

        public string CategoriesToString
        {
            get
            {
                string output = "";
                foreach (var item in this.ItemCategories)
                {
                    output += item.Category.Name;
                    if (item != this.ItemCategories.LastOrDefault()) output += ", ";
                }
                return output;
            }
        }

        public string ReleaseDateToString
        {
            get { return ReleaseDate.ToShortDateString(); }
        }

        public bool IsWithinSaleRange
        {
            // veranderen naar 31?
            get { return (this.LifeSpan - DateTime.Now).TotalDays <= 31; }
        }

        public bool ItemIsReserved
        {
            get
            {
                foreach (var item in this.UserItems)
                {
                    if (item.ReservedUntil != null && DateTime.Now < item.ReservedUntil)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public bool ItemIsLentOut
        {
            get
            {
                foreach (var item in this.UserItems)
                {
                    if (item.BorrowedDate != null && item.ReturnedDate == null) return true;
                }
                return false;
            }
        }
    }
}
