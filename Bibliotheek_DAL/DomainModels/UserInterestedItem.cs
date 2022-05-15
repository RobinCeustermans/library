using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheek_DAL
{
    public partial class UserInterestedItem
    {
        public int UserInterestedItemID { get; set; }
        public DateTime? DrawnDate { get; set; }
        [Required]
        public int ItemID { get; set; }
        [Required]
        public int UserID { get; set; }

        public User User { get; set; }
        public Item Item { get; set; }
    }
}
