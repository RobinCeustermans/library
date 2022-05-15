using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheek_DAL
{
    [AddINotifyPropertyChangedInterface]
    public partial class Category : Baseclass
    {
        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object obj)
        {
            return obj is Category category &&
                   (CategoryID == category.CategoryID ||
                   Name == category.Name);
        }

        public override int GetHashCode()
        {
            return -1237963196 + CategoryID.GetHashCode();
        }

        public override string this[string columnName]
        {
            get
            {
                if (columnName == "Name" && string.IsNullOrWhiteSpace(Name))
                {
                    return "De beschrijving is een verplicht veld!";
                }
                return "";
            }
        }

    }
}
