using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharePoint.SpecFlow.Tests
{
    [TestClass]
    public class SiteGivenTests
    {
        SiteGivens _sut = new SiteGivens(new Uri("http://rp2013-3:113"));
 
        [TestMethod]
        public void FeatureActivationIsIdemptotent()
        {
            _sut.GivenTheCurrentSiteHasTheFeatureEnabled("f6924d36-2fa8-4f0b-b16d-06b7250180fa");

            _sut.GivenTheCurrentSiteHasTheFeatureEnabled("f6924d36-2fa8-4f0b-b16d-06b7250180fa");
        }
    }
}
