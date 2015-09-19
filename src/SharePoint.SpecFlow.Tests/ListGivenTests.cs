using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace SharePoint.SpecFlow.Tests
{
    [TestClass]
    public class ListGivenTests
    {
        Context _ctx = new Context { SiteUri = new Uri("http://rp2013-3:113") };
 
        [TestMethod]
        public void ListCreationIsIdempotent()
        {
            var sut = new ListGivens(_ctx);

            sut.GivenThereIsListCalled(ListTemplateType.DocumentLibrary, "TestDocumentLibrary");

            sut.GivenThereIsListCalled(ListTemplateType.DocumentLibrary, "TestDocumentLibrary");
        }

        [TestMethod]
        public void ListCreationSetsContext()
        {
            var sut = new ListGivens(_ctx);

            sut.GivenThereIsListCalled(ListTemplateType.DocumentLibrary, "TestDocumentLibrary");

            Assert.AreEqual("TestDocumentLibrary", _ctx.LastListTitle);
        }

        [TestMethod]
        public void ListCreationThrowsWhenListAlreadyExistsButTypeIsWrong()
        {
            var sut = new ListGivens(_ctx);

            sut.GivenThereIsListCalled(ListTemplateType.GenericList, "TestGenericList");

            bool thrown = false;
            try
            {
                sut.GivenThereIsListCalled(ListTemplateType.DocumentLibrary, "TestGenericList");
            }
            catch
            {
                thrown = true;
            }

            Assert.IsTrue(thrown);
        }

        [TestMethod]
        public void DocumentSetCreationIsIdempotent()
        {
            var sut = new ListGivens(_ctx);

            sut.GivenThereIsListCalled(ListTemplateType.DocumentLibrary, "BDLBS_2");
            sut.GivenTheListHasADocumentSetCalled("BDLBS_DSB");

            sut.GivenTheListHasADocumentSetCalled("BDLBS_DSB");
        }
    }
}
