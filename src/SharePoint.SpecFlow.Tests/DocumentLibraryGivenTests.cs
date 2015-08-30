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
    public class DocumentLibraryGivenTests
    {
        [TestMethod]
        public void DocumentLibraryCreationIsIdempotent()
        {
            var sut = new DocumentLibraryGivens(new Uri("http://rp2013-1108:113"));

            sut.GivenThereIsADocumentLibraryCalled("blah");

            sut.GivenThereIsADocumentLibraryCalled("blah");

        }
    }
}
