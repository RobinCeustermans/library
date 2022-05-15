using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheek_DAL
{
    [AddINotifyPropertyChangedInterface]
    public partial class ContributorItem
    {
        public override bool Equals(object obj)
        {
            return obj is ContributorItem item &&
                   ContributorID == item.ContributorID &&
                   ItemID == item.ItemID;
        }

        public override int GetHashCode()
        {
            int hashCode = 327952122;
            hashCode = hashCode * -1521134295 + ContributorID.GetHashCode();
            hashCode = hashCode * -1521134295 + ItemID.GetHashCode();
            return hashCode;
        }
    }
}
