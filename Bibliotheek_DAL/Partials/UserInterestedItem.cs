using Bibliotheek_DAL.UnitsOfWork;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheek_DAL
{
    [AddINotifyPropertyChangedInterface]
    public partial class UserInterestedItem
    {
        IUnitOfWork unitOfWork = new UnitOfWork(new LibraryEntities());
        //public UserInterestedItem(Item item, User user)
        //{
        //    this.UserID = user.UserID;
        //    this.User = user;
        //    this.Item = item;
        //    this.ItemID = item.ItemID;
        //}

        public UserInterestedItem()
        {

        }

        public override bool Equals(object obj)
        {
            if (obj is UserInterestedItem item)
            {
                if (this.ItemID == item.ItemID && this.UserID == item.UserID)
                {
                    return true;
                }
                else return false;
            }
            else return false;
        }

        public override int GetHashCode()
        {
            int hashCode = 978915809;
            hashCode = hashCode * -1521134295 + EqualityComparer<IUnitOfWork>.Default.GetHashCode(unitOfWork);
            hashCode = hashCode * -1521134295 + ItemID.GetHashCode();
            hashCode = hashCode * -1521134295 + UserID.GetHashCode();
            return hashCode;
        }
    }
}
