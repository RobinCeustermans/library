using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Bibliotheek_DAL
{
    [AddINotifyPropertyChangedInterface]
    public partial class Fine
    {
        public string IsPaidToString
        {
            get
            {
                if (IsPaid == true)
                {
                    return "Betaald";
                }
                else return "Niet Betaald";
            }
        }


    }
}
