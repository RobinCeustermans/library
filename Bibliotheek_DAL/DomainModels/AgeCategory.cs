using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheek_DAL
{
    public partial class AgeCategory
    {
        public int AgeCategoryID { get; set; }
        [Required]
        public string Description { get; set; }

        // navigation props
        public ICollection<Item> Items { get; set; }
    }
}
