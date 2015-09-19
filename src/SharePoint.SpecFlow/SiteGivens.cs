using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace SharePoint.SpecFlow
{
    [Binding]
    public class SiteGivens : BindingBase
    {
        public SiteGivens(Context ctx)
            : base(ctx)
        {
        }

        [Given("the current site has the \"([^\"]*)\" feature enabled")]
        public void GivenTheCurrentSiteHasTheFeatureEnabled(string featureGuid)
        {
            using (var cc = new ClientContext(Context.SiteUri))
            {
                var featureId = Guid.Parse(featureGuid);
     
                var feature = cc.Site.Features.GetById(featureId);
                cc.Load(feature);
                cc.ExecuteQuery();

                if (feature.ServerObjectIsNull.HasValue && feature.ServerObjectIsNull.Value)
                {
                    feature = cc.Site.Features.Add(featureId, true, FeatureDefinitionScope.Site);
                    cc.Load(feature);
                    cc.ExecuteQuery();
                }
            }
        }
    }
}
