using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheek_DAL
{
    public partial class MembershipType
    {
        public int MembershipTypeID { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int MaxNumberItems { get; set; }
        
        // navigation props
        public ICollection<User> Users { get; set; }
    }
}
