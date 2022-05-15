using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheek_DAL
{
    public partial class ContributorType
    {
        public int ContributorTypeID { get; set; }
        [Required]
        public string Description { get; set; }

        // navigation props
        public ICollection<Contributor> Contributors { get; set; }
    }
}
