using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharePoint.SpecFlow
{
    public class Context
    {
        public Uri SiteUri { get; set; }

        public string LastListTitle { get; set; }

        public string LastFileServerRelativeUrl { get; set; }
    }
}
