using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGSC
{
    public partial class CreditRequest
    {
        public enum CreditStatus
        {
            Requested,
            Approved,
            Rejected,
            Finished
        }
    }
}
