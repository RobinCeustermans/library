using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheek_DAL
{
    public partial class UserItem
    {
        public int UserItemID { get; set; }
        [Required]
        public int UserID { get; set; }
        [Required]
        public int ItemID { get; set; }
        public DateTime? ReservedUntil { get; set; }
        public DateTime? BorrowedDate { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? ReturnedDate { get; set; }

        // navigation props
        public User User { get; set; }
        public Item Item { get; set; }
    }
}
