using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheek_DAL
{
    [AddINotifyPropertyChangedInterface]
    public partial class Contributor : Baseclass
    {
        public override bool Equals(object obj)
        {
            return obj is Contributor contributor &&
                   ContributorID == contributor.ContributorID;
        }

        public override int GetHashCode()
        {
            return -678409313 + ContributorID.GetHashCode();
        }

        public override string ToString()
        {
            return Surname + " " + Name;
        }

        public override string this[string columnName]
        {
            get
            {
                if (columnName == "Name" && string.IsNullOrWhiteSpace(Name))
                {
                    return "Achternaam is een verplicht veld!";
                }
                if (columnName == "Surname" && string.IsNullOrWhiteSpace(Surname))
                {
                    return "Voornaam is een verplicht veld!";
                }
                if (columnName == "" && ContributorTypeID <= 0)
                {
                    return "Het Contributor type ID moet groter zijn dan 0";
                }

                return "";
            }
        }
    }
}
