using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheek_DAL
{
    [AddINotifyPropertyChangedInterface]
    public partial class BookFair : Baseclass
    {

        public override string this[string columnName]
        {
            get
            {
                if (columnName == "Name" && string.IsNullOrWhiteSpace(Name))
                {
                    return "De benaming is een verplicht veld!";
                }
                if (columnName == "Description" && string.IsNullOrWhiteSpace(Description))
                {
                    return "De beschrijving is een verplicht veld!";
                }
                if (columnName == "StartDate" && StartDate < DateTime.Now)
                {
                    return "De startdatum is een verplicht veld en kan niet vroeger worden gestart dan de eerstvolgende dag!";
                }
                if (columnName == "EndDate" && EndDate < StartDate)
                {
                    return "De einddatum is een verplicht veld en kan niet vroeger zijn dan de startdatum!";
                }
                return "";
            }
        }

        public override bool Equals(object obj)
        {
            return obj is BookFair fair &&
                   BookFairID == fair.BookFairID;
        }

        public override int GetHashCode()
        {
            int hashCode = -162458559;
            hashCode = hashCode * -1521134295 + BookFairID.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            return hashCode;
        }

        public string BookFairOpen
        {
            get
            {
                if (this.RegistrationOpen == true)
                {
                    return "Open";
                }
                else
                {
                    return "Gesloten";
                }
            }
        }
    }
}
