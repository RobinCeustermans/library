using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheek_DAL.DomainModels
{
    public class EmailText
    {
        public int EmailTextID { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string HTMLString { get; set; }
    }
}
