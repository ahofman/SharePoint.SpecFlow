using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.DocumentSet;
using Microsoft.SharePoint.Client.Workflow;
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

            using (var cc = new ClientContext(_currentWebUri))
            {
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
        }

        [Given("the list \"(.*?)\" has a Document Set called \"(.*?)\"")]
        public void GivenTheListHasADocumentSetCalled(string listTitle, string docSetTitle)
        {
            // Ensure the SharePoint Server Publishing Infrastructure feature is enabled
            var siteGivens = new SiteGivens(_currentWebUri);
            siteGivens.GivenTheCurrentSiteHasTheFeatureEnabled("f6924d36-2fa8-4f0b-b16d-06b7250180fa");

            string documentSetContentType = "0x0120D52000D0EE7F289A8729498B34F1E26F5E988A";

            using (var cc = new ClientContext(_currentWebUri))
            {
                var list = cc.Web.Lists.GetByTitle(listTitle);
                cc.Load(list);
                cc.Load(list.RootFolder);
                cc.Load(list.RootFolder.Folders);
                cc.Load(list.ContentTypes);
                cc.Load(cc.Web);
                cc.Load(cc.Web.ContentTypes);
                cc.ExecuteQuery();

                if (list.RootFolder.Folders.Any(x => x.Name == docSetTitle))
                {
                    // Folder already exists with the given doc set title.
                    // Ensure that this folder is actually a docset...
                }
                else
                {
                    list.ContentTypesEnabled = true;

                    var ct = list.ContentTypes.SingleOrDefault(x => x.Id.StringValue == documentSetContentType);
                    if (ct == null)
                    {
                        ct = list.ContentTypes.AddExistingContentType(cc.Web.ContentTypes.GetById(documentSetContentType));
                        cc.ExecuteQuery();
                    }

                    var docSet = DocumentSet.Create(cc, list.RootFolder, docSetTitle, ct.Id);

                    cc.ExecuteQuery();
                }
            }
        }

        [Given("the list \"(.*?)\" has a workflow associated")]
        public void GivenTheListHasAWorkflowAssociated(string listTitle, 
            Guid workflowId, 
            string workflowAssociationName, 
            string workflowHistoryListName,
            string workflowTasksListName,
            string associationData,
            bool autoStartChange,
            bool autoStartCreate)
        {
            using (var cc = new ClientContext(_currentWebUri))
            {
                var list = cc.Web.Lists.GetByTitle(listTitle);
                var workflowHistoryList = cc.Web.Lists.GetByTitle(workflowHistoryListName);
                var workflowTasksList = cc.Web.Lists.GetByTitle(workflowTasksListName);
                cc.Load(list);
                cc.Load(list.WorkflowAssociations);
                cc.Load(workflowHistoryList);
                cc.Load(workflowTasksList);
                cc.Load(cc.Web.WorkflowTemplates);
                cc.ExecuteQuery();

                var assoc = list.WorkflowAssociations.SingleOrDefault(x => x.Name == workflowAssociationName);
                if (assoc == null)
                {

                    var waci = new WorkflowAssociationCreationInformation();
                    waci.Template = cc.Web.WorkflowTemplates.Single(x => x.Id == workflowId);
                    waci.Name = workflowAssociationName;
                    waci.TaskList = workflowTasksList;
                    waci.HistoryList = workflowHistoryList;

                    assoc = list.WorkflowAssociations.Add(waci);
                }

                assoc.AutoStartChange = autoStartChange;
                assoc.AutoStartCreate = autoStartCreate;
                assoc.AssociationData = associationData;

                assoc.Update();
                cc.ExecuteQuery();
            }
        }

        private Uri _currentWebUri;
    }
}
