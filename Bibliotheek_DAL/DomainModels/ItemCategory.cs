using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheek_DAL
{
    public partial class ItemCategory
    {
        public int ItemCategoryID { get; set; }
        [Required]
        public int ItemID { get; set; }
        [Required]
        public int CategoryID { get; set; }

        // navigation props
        public Item Item { get; set; }
        public Category Category { get; set; }
    }
}
