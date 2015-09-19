using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace SharePoint.SpecFlow
{
    public abstract class BindingBase
    {
        public BindingBase(Context context)
        {
            _context = context;
        }

        protected Context Context
        {
            get { return _context; }
        }

        private Context _context;
    }
}
