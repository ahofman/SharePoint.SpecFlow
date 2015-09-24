using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharePoint.SpecFlow.Tests
{
    [TestClass]
    public class FileGivenTests
    {
        Context _ctx = new Context { SiteUri = new Uri( TestsSettings.Default.SiteCollectionUrl ) };

        [TestInitialize]
        public void TestInitialize()
        {
            // create a doc lib
            var listCreator = new ListGivens(_ctx);
            listCreator.GivenThereIsListCalled(Microsoft.SharePoint.Client.ListTemplateType.DocumentLibrary, "testdoclib1");
        }

        [TestMethod]
        public void FileCreationIsIdempotent()
        {
            var sut = new FileGivens(_ctx);

            sut.GivenThereIsAFileWithUrl("this is a test", "/testdoclib1/testfile1.txt");

            sut.GivenThereIsAFileWithUrl("this is a 2nd test", "/testdoclib1/testfile1.txt");

            var fileThens = new FileThens(_ctx);
            fileThens.TheFileContentsEqual("/testdoclib1/testfile1.txt", "this is a 2nd test");
        }

        [TestMethod]
        public void FileCreationSetsContext()
        {
            var sut = new FileGivens(_ctx);

            sut.GivenThereIsAFileWithUrl("FileCreationSetsContext", "/testdoclib1/testfile2.txt");

            Assert.AreEqual("/testdoclib1/testfile2.txt", _ctx.LastFileServerRelativeUrl);
        }
    }
}
