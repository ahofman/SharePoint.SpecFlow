using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace SharePoint.SpecFlow
{
    [Binding]
    public class FileThens : BindingBase
    {
        public FileThens(Context ctx)
            : base(ctx)
        {
        }

        [Then("the file at server relative url \"([^\"]*)\" should have the contents \"([^\"]*)\"")]
        public void TheFileContentsEqual(string url, string expectedContents)
        {
            var cc = new ClientContext(Context.SiteUri);
            var fi = File.OpenBinaryDirect(cc, url);
            var ms = new System.IO.MemoryStream();

            byte[] temp = new byte[64 * 1024];
            int chunkSizeRead = 0;
            do
            {
                chunkSizeRead = fi.Stream.Read(temp, 0, 64 * 1024);

                ms.Write(temp, 0, chunkSizeRead);
            }
            while (chunkSizeRead == 64 * 1024);

            var actualString = Encoding.Default.GetString( ms.GetBuffer(), 0, (int)ms.Length );

            Assert.AreEqual(expectedContents, actualString);
        }

        
    }
}
