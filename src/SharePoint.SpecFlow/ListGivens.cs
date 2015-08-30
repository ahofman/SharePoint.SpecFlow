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
    public class ListGivens
    {
        public ListGivens()
            : this(ScenarioContext.Current.GetWebUri())
        {
        }

        public ListGivens(Uri currentWebUri)
        {
            _currentWebUri = currentWebUri;
        }

        [Given("there is a (.*?) list called \"(.*?)\"")]
        public void GivenThereIsListCalled(ListTemplateType listTemplateType, string listTitle)
        {
            var listTemplateBaseType = listTemplateType.GetBaseType();

            var cc = new ClientContext(_currentWebUri);

            var list = cc.Web.Lists.GetByTitle(listTitle);
            cc.Load(list);

            // I wonder if there's a better way of querying if a list exists without relying on exceptions?
            try
            {
                cc.ExecuteQuery();
            }
            catch (ServerException)
            {
                list = null;
            }

            if (list != null)
            {
                // The list already exists.
                // Ensure that it is the correct type.
                if (list.BaseType != listTemplateBaseType)
                {
                    throw new SharePointSpecFlowException(String.Format("List with title {0} already exists, but it isn't the expected type of {1}.", listTitle, listTemplateBaseType));
                }
            }
            else
            {
                var lci = new ListCreationInformation();
                lci.Title = listTitle;
                lci.TemplateType = (int)listTemplateType;
                list = cc.Web.Lists.Add(lci);
                cc.Load(list);
                cc.ExecuteQuery();
            }

            if (ScenarioContext.Current != null)
            {
                ScenarioContext.Current.SetList(list);
            }
        }

        private Uri _currentWebUri;
    }
}
