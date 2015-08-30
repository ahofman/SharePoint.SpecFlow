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
        FileGivens _sut = new FileGivens(new Uri("http://rp2013-1108:113"));
 
        [TestMethod]
        public void FileCreationIsIdempotent()
        {
            // create a doc lib
            var listCreator = new ListGivens(new Uri("http://rp2013-1108:113"));
            listCreator.GivenThereIsListCalled(Microsoft.SharePoint.Client.ListTemplateType.DocumentLibrary, "testdoclib1");

            _sut.GivenThereIsAFileWithUrl("this is a test", "/testdoclib1/testfile1.txt");

            _sut.GivenThereIsAFileWithUrl("this is a 2nd test", "/testdoclib1/testfile1.txt");

            var fileThens = new FileThens(new Uri("http://rp2013-1108:113"));
            fileThens.TheFileContentsEqual("/testdoclib1/testfile1.txt", "this is a 2nd test");
        }
    }
}
