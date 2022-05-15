using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheek_DAL
{
    public partial class ContributorItem
    {
        public int ContributorItemID { get; set; }
        [Required]
        public int ContributorID { get; set; }
        [Required]
        public int ItemID { get; set; }

        // navigation props
        public Item Item { get; set; }
        public Contributor Contributor { get; set; }
    }
}
