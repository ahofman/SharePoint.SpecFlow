using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace SharePoint.SpecFlow
{
    public static class ScenarioContextExtensions
    {
        public static Uri GetWebUri(this ScenarioContext sc)
        {
            return sc["CurrentWebUri"] as Uri;
        }

        public static void SetWebUri(this ScenarioContext sc, Uri webUri)
        {
            sc["CurrentWebUri"] = webUri;
        }
    }
}
