using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheek_DAL
{
    public partial class User
    {
        public int UserID { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string PostalCode { get; set; }
        [Required]
        public string StreetName { get; set; }
        [Required]
        public string HouseNumber { get; set; }
        [Required]
        public string Password { get; set; }
        [Required][DefaultValue(true)]
        public bool IsActive { get; set; }
        public DateTime? MembershipStartDate { get; set; }
        [Required]
        public int MembershipTypeID { get; set; }

        // navigation props
        public ICollection<Fine> Fines { get; set; }
        public ICollection<UserBookFair> UserBookFairs { get; set; }
        public ICollection<UserInterestedItem> UserInterestedItems { get; set; }
        public ICollection<UserItem> UserItems { get; set; }
        public MembershipType MembershipType { get; set; }

    }
}
