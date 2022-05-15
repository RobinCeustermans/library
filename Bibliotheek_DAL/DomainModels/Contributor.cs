using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheek_DAL
{
    public partial class Contributor
    {
        public int ContributorID { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int ContributorTypeID { get; set; }

        // navigation props
        public ICollection<ContributorItem> ContributorItems { get; set; }
        public ContributorType ContributorType { get; set; }
    }
}
