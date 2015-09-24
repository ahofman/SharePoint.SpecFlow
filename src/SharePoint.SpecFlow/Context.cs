using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SharePoint.SpecFlow
{
    public class Context
    {
        public Uri SiteUri { get; set; }

        const string SPECFLOW_SHAREPOINT_USERNAME = "SPECFLOW_SHAREPOINT_USERNAME";

        const string SPECFLOW_SHAREPOINT_PASSWORD = "SPECFLOW_SHAREPOINT_PASSWORD";

        const string SPECFLOW_SHAREPOINT_DOMAIN = "SPECFLOW_SHAREPOINT_DOMAIN";

        public ClientContext CreateClientContext()
        {
            var userNameFromEnvironment = Environment.GetEnvironmentVariable(SPECFLOW_SHAREPOINT_USERNAME);
            var passwordFromEnvironment = Environment.GetEnvironmentVariable(SPECFLOW_SHAREPOINT_PASSWORD);
            var domainFromEnvironment = Environment.GetEnvironmentVariable(SPECFLOW_SHAREPOINT_DOMAIN);

            NetworkCredential credential = null;

            if (!string.IsNullOrEmpty(userNameFromEnvironment) && !string.IsNullOrEmpty(passwordFromEnvironment))
            {
                if (string.IsNullOrEmpty(domainFromEnvironment))
                {
                    credential = new NetworkCredential(userNameFromEnvironment, passwordFromEnvironment);
                }
                else
                {
                    credential = new NetworkCredential(userNameFromEnvironment, passwordFromEnvironment, domainFromEnvironment);
                }
            }

            var result = new ClientContext(SiteUri);

            if ( credential != null )
                result.Credentials = credential;

            return result;
        }

        public string LastListTitle { get; set; }

        public string LastFileServerRelativeUrl { get; set; }

        public int TimeoutSeconds { get { return 60; } }
    }
}
