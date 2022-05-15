using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheek_DAL
{
    public partial class Category
    {
        public int CategoryID { get; set; }
        [Required]
        public string Name { get; set; }

        // navigation props
        public ICollection<ItemCategory> ItemCategories { get; set; }
    }
}
