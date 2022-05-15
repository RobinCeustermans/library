using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheek_DAL
{
    public partial class UserBookFair
    {
        public int UserBookFairID { get; set; }
        [Required]
        public int BookFairID { get; set; }
        [Required]
        public int UserID { get; set; }

        // navigation props
        public User User { get; set; }
        public BookFair BookFair { get; set; }
    }
}
