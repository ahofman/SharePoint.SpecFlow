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
    public class DocumentLibraryGivens
    {
        public DocumentLibraryGivens()
            : this(ScenarioContext.Current.GetWebUri())
        {
        }

        public DocumentLibraryGivens(Uri currentWebUri)
        {
            _currentWebUri = currentWebUri;
        }

        [Given("there is a document library called \"(.*?)\"")]
        public void GivenThereIsADocumentLibraryCalled(string docLibName)
        {
            var cc = new ClientContext(_currentWebUri);

            List existingList = cc.Web.Lists.GetByTitle(docLibName);
            cc.Load(existingList);

            // I wonder if there's a better way of querying if a list exists without relying on exceptions?
            try
            {
                cc.ExecuteQuery();
            }
            catch (ServerException)
            {
                existingList = null;
            }

            if (existingList != null)
            {
                // The list already exists.
                // TODO: Make sure it's a document library...
            }
            else
            {
                var lci = new ListCreationInformation();
                lci.Title = docLibName;
                lci.TemplateType = (int)ListTemplateType.DocumentLibrary;
                var addedList = cc.Web.Lists.Add(lci);
                cc.Load(addedList);
                cc.ExecuteQuery();
            }
        }

        private Uri _currentWebUri;
    }
}
