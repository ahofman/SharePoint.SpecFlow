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
    public class FileWhens : BindingBase
    {
        public FileWhens(Context ctx)
            : base(ctx)
        {
        }

        [When("the file is copied to \"([^\"]*)\" by user ([^ ]+)")]
        public void TheFileIsCopiedTo(string newLocation, string userName)
        {
            Context.LastUserName = userName;
            TheFileIsCopiedTo(newLocation);
        }

        [When("the file is copied to \"([^\"]*)\"")]
        public void TheFileIsCopiedTo(string newLocation)
        {
            using (var cc = Context.CreateClientContext())
            {
                var file = cc.Web.GetFileByServerRelativeUrl(Context.LastFileServerRelativeUrl);

                file.CopyTo(newLocation, true);

                cc.Load(file);
                cc.ExecuteQuery();

                Context.LastFileServerRelativeUrl = newLocation;
            }
        }

        [When("the file is moved to \"([^\"]*)\" by user ([^ ]+)")]
        public void TheFileIsMovedTo(string newLocation, string userName)
        {
            Context.LastUserName = userName;
            TheFileIsMovedTo(newLocation);
        }

        [When("the file is moved to \"([^\"]*)\"")]
        public void TheFileIsMovedTo(string newLocation)
        {
            using (var cc = Context.CreateClientContext())
            {
                var file = cc.Web.GetFileByServerRelativeUrl(Context.LastFileServerRelativeUrl);

                file.MoveTo(newLocation, MoveOperations.None);

                cc.Load(file);
                cc.ExecuteQuery();

                Context.LastFileServerRelativeUrl = newLocation;
            }
        }
    }
}
