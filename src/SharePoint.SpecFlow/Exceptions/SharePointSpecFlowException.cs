using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharePoint.SpecFlow
{
    public class SharePointSpecFlowException : Exception
    {
        public SharePointSpecFlowException(string message)
            : base(message)
        {
        }
    }
}
