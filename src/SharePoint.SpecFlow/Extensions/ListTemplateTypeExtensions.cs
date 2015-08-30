using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharePoint.SpecFlow
{
    public static class ListTemplateTypeExtensions
    {
        public static BaseType GetBaseType(this ListTemplateType ltt)
        {
            // From https://msdn.microsoft.com/en-us/library/office/microsoft.sharepoint.client.listtemplatetype.aspx
            switch (ltt)
            {
                case ListTemplateType.GenericList:
                case ListTemplateType.Links:
                case ListTemplateType.Announcements:
                case ListTemplateType.Contacts:
                case ListTemplateType.Events:
                case ListTemplateType.Tasks:
                case ListTemplateType.DiscussionBoard:
                case ListTemplateType.WorkflowProcess:
                case ListTemplateType.CustomGrid:
                case ListTemplateType.WorkflowHistory:
                case ListTemplateType.GanttTasks:
                    return BaseType.GenericList;
                case ListTemplateType.DocumentLibrary:
                case ListTemplateType.PictureLibrary:
                case ListTemplateType.DataSources:
                case ListTemplateType.XMLForm:
                case ListTemplateType.NoCodeWorkflows:
                case ListTemplateType.WebPageLibrary:
                    return BaseType.DocumentLibrary;
                case ListTemplateType.Survey:
                    return BaseType.Survey;
                case ListTemplateType.IssueTracking:
                    return BaseType.Issue;
                default:
                    throw new SharePointSpecFlowException(String.Format("Can't use ListTemplateType {0} as it can't be mapped to a BaseType.", ltt.ToString()));
            }
        }
    }
}
