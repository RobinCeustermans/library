using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheek_DAL
{
    [AddINotifyPropertyChangedInterface]
    public partial class UserBookFair
    {
        public override bool Equals(object obj)
        {
            if (obj is UserBookFair fair)
            {
                if (UserID == fair.UserID && BookFairID == fair.BookFairID)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            int hashCode = 1364217602;
            hashCode = hashCode * -1521134295 + UserBookFairID.GetHashCode();
            hashCode = hashCode * -1521134295 + BookFairID.GetHashCode();
            hashCode = hashCode * -1521134295 + UserID.GetHashCode();
            return hashCode;
        }
    }
}
