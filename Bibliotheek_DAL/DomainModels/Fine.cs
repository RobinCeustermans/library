using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheek_DAL
{
    public partial class Fine
    {
        public int FineID { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public bool IsPaid { get; set; }
        [Required]
        public int UserID { get; set; }
        public int ItemID { get; set; }

        // navigation props
        public User User { get; set; }
        public Item Item { get; set; }
    }
}
