using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Workflow;
using System;
using System.Linq;
using TechTalk.SpecFlow;

#if SHAREPOINT_2013
using Microsoft.SharePoint.Client.DocumentSet;
#endif

namespace SharePoint.SpecFlow
{
    [Binding]
    public class ListGivens : BindingBase
    {
        public ListGivens(Context context)
            : base(context)
        {
        }

        [Given("there is a (.*?) list called \"([^\"]*)\" in site \"(http[^\"]*)\"")]
        public void GivenThereIsListCalled(ListTemplateType listTemplateType, string listTitle, string siteUri)
        {
            Context.SiteUri = new Uri(siteUri);
            GivenThereIsListCalled(listTemplateType, listTitle);
        }

        [Given("there is a (.*?) list called \"([^\"]*)\"")]
        public void GivenThereIsListCalled(ListTemplateType listTemplateType, string listTitle)
        {
            var listTemplateBaseType = listTemplateType.GetBaseType();

            using (var cc = Context.CreateClientContext())
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

                Context.LastListTitle = listTitle;
            }
        }

#if SHAREPOINT_2013
        [Given("the list has a Document Set called \"([^\"]*)\"")]
        public void GivenTheListHasADocumentSetCalled(string docSetTitle)
        {
            // Ensure the SharePoint Server Publishing Infrastructure feature is enabled
            var siteGivens = new SiteGivens(Context);
            siteGivens.GivenTheCurrentSiteHasTheFeatureEnabled("f6924d36-2fa8-4f0b-b16d-06b7250180fa");

            string documentSetContentType = "0x0120D520";

            using (var cc = Context.CreateClientContext())
            {
                var list = cc.Web.Lists.GetByTitle(Context.LastListTitle);
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
                    list.Update();
                    cc.ExecuteQuery();

                    var ct = list.ContentTypes.SingleOrDefault(x => x.Id.StringValue.StartsWith(documentSetContentType));
                    if (ct == null)
                    {
                        var docSetContentType = cc.Web.ContentTypes.GetById(documentSetContentType);
                        cc.Load(docSetContentType);
                        cc.ExecuteQuery();
                        ct = list.ContentTypes.AddExistingContentType(docSetContentType);
                        cc.Load(ct);
                        cc.ExecuteQuery();
                    }

                    var docSet = DocumentSet.Create(cc, list.RootFolder, docSetTitle, ct.Id);

                    cc.ExecuteQuery();
                }
            }
        }
#endif 

        [Given("the list has a workflow associated")]
        public void GivenTheListHasAWorkflowAssociated(
            /*Guid workflowId,
            string workflowAssociationName,
            string workflowHistoryListName,
            string workflowTasksListName,
            string associationData,
            bool autoStartChange,
            bool autoStartCreate*/
            Table table)
        {
            var workflowId = Guid.Parse( table.Rows[0]["WorkflowId"].ToString() );
            var workflowAssociationName = table.Rows[0]["WorkflowAssociationName"];
            var workflowHistoryListName = table.Rows[0]["WorkflowHistoryListName"];
            var workflowTasksListName = table.Rows[0]["WorkflowTasksListName"];
            var associationData = table.Rows[0]["AssociationData"];
            var autoStartChange = Boolean.Parse(table.Rows[0]["AutoStartChange"]);
            var autoStartCreate = Boolean.Parse(table.Rows[0]["AutoStartCreate"]);

            using (var cc = Context.CreateClientContext())
            {
                var list = cc.Web.Lists.GetByTitle(Context.LastListTitle);
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
    }
}
