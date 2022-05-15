using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheek_DAL
{
    public partial class BookFair
    {
        public int BookFairID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required] [DefaultValue(true)]
        public bool RegistrationOpen { get; set; }
        public string Location { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }

        // navigation props
        public ICollection<UserBookFair> UserBookFairs { get; set; }
    }
}
