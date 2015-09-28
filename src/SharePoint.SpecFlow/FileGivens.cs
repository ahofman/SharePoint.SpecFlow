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
    public class FileGivens : BindingBase
    {
        public FileGivens(Context ctx)
            : base(ctx)
        {
        }

        [Given("there is a file with contents \"([^\"]*)\" at server relative url \"([^\"]*)\" by user ([^ ]+)")]
        public void GivenThereIsAFileWithUrl(string contents, string url, string user)
        {
            Context.LastUserName = user;
            GivenThereIsAFileWithUrl(contents, url);
        }

        [Given("there is a file with contents \"([^\"]*)\" at server relative url \"([^\"]*)\"")]
        public void GivenThereIsAFileWithUrl(string contents, string url)
        {
            using (var cc = Context.CreateClientContext())
            {
                var rawContents = Encoding.Default.GetBytes(contents);

                File.SaveBinaryDirect(cc, url, new System.IO.MemoryStream(rawContents), true);
            }

            Context.LastFileServerRelativeUrl = url;
        }

        [Given("the file is checked out by user ([^ ]+)")]
        public void GivenTheFileIsCheckedOutByUser(string userName)
        {
            Context.LastUserName = userName;
        }

        [Given("the file is checked out")]
        public void GivenTheFileIsCheckedOut()
        {
            using (var cc = Context.CreateClientContext())
            {
                var f = cc.Web.GetFileByServerRelativeUrl(Context.LastFileServerRelativeUrl);

                cc.Load(f);
                cc.ExecuteQuery();

                if (f.CheckOutType == CheckOutType.None)
                {
                    f.CheckOut();
                }
                else if (f.CheckedOutByUser.LoginName != Context.LastUserName)
                {
                    throw new SharePointSpecFlowException("File already checked out by " + f.CheckedOutByUser.LoginName);
                }
            }
        }
    }
}
