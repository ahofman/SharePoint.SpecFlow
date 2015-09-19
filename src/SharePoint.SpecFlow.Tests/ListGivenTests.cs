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

        /*
        [TestMethod]
        public void MyAwesomeTestSetup()
        {
            _sut.GivenThereIsListCalled(ListTemplateType.DocumentLibrary, "253_BugDocSetMove_1");
            _sut.GivenThereIsListCalled(ListTemplateType.DocumentLibrary, "253_BugDocSetMove_2");

            _sut.GivenTheListHasADocumentSetCalled("253_BugDocSetMove_1", "253_DogSetA");
            _sut.GivenTheListHasADocumentSetCalled("253_BugDocSetMove_1", "253_DogSetB");

            _sut.GivenTheListHasAWorkflowAssociated("253_BugDocSetMove_1",
                Guid.Parse("79a21da3-a5ad-4b7e-b7f6-a28b85fa31eb"),
                "RP Submit Stub",
                "Workflow History",
                "Tasks",
                @"<SubmitWorkflowAssociationData xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""><WorkflowType>SubmitStub</WorkflowType></SubmitWorkflowAssociationData>",
                true, true);

            _sut.GivenTheListHasAWorkflowAssociated("253_BugDocSetMove_2",
                Guid.Parse("79a21da3-a5ad-4b7e-b7f6-a28b85fa31eb"),
                "RP Submit Stub",
                "Workflow History",
                "Tasks",
                @"<SubmitWorkflowAssociationData xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""><WorkflowType>SubmitStub</WorkflowType></SubmitWorkflowAssociationData>",
                true, true);

            var fg = new FileGivens(new Uri("http://rp2013-3:113"));

            fg.GivenThereIsAFileWithUrl("253_blah", "/253_BugDocSetMove_1/253_DogSetA/253_blah.txt");
            fg.GivenThereIsAFileWithUrl("253_blah", "/253_BugDocSetMove_1/253_DogSetB/253_blah2.txt");
        }*/
    }
}
