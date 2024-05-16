using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGSC
{
    public partial class Customer
    {
        public string FullName 
        {
            get
            {
                return $"{Name} {FirstSurname} {SecondSurname}";
            }    
        }
    }
}
