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
        Context _ctx = new Context { SiteUri = new Uri(TestsSettings.Default.SiteCollectionUrl) };

        [TestMethod]
        public void FeatureActivationIsIdemptotent()
        {
            var sut = new SiteGivens(_ctx);

            sut.GivenTheCurrentSiteHasTheFeatureEnabled("f6924d36-2fa8-4f0b-b16d-06b7250180fa");

            sut.GivenTheCurrentSiteHasTheFeatureEnabled("f6924d36-2fa8-4f0b-b16d-06b7250180fa");
        }
    }
}
