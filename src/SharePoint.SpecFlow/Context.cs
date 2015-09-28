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

        const string SHAREPOINT_SPECFLOW_CREDENTIAL_PREFIX = "SHAREPOINT_SPECFLOW_CREDENTIAL_";

        public ClientContext CreateClientContext()
        {
            NetworkCredential credential = null;

            if (!string.IsNullOrEmpty(LastUserName))
            {
                var environmentVariableName = SHAREPOINT_SPECFLOW_CREDENTIAL_PREFIX + LastUserName;
                var passwordFromEnvironment = Environment.GetEnvironmentVariable(environmentVariableName);
                if (string.IsNullOrEmpty(passwordFromEnvironment))
                {
                    throw new SharePointSpecFlowException("No password supplied for user " + LastUserName + ". Please supply the password for this user in an environment variable called " + environmentVariableName);
                }

                var domain = string.Empty;

                // determine the domain from the passed in username
                var splitString = LastUserName.Split('/');
                if (splitString.Length > 1)
                {
                    domain = splitString.First();
                    LastUserName = splitString.Last();
                }

                if (string.IsNullOrEmpty(domain))
                {
                    credential = new NetworkCredential(LastUserName, passwordFromEnvironment);
                }
                else
                {
                    credential = new NetworkCredential(LastUserName, passwordFromEnvironment, domain);
                }
            }

            var result = new ClientContext(SiteUri);

            if ( credential != null )
                result.Credentials = credential;

            return result;
        }

        public string LastListTitle { get; set; }

        public string LastFileServerRelativeUrl { get; set; }

        public string LastUserName { get; set; }

        public int TimeoutSeconds { get { return 60; } }
    }
}
