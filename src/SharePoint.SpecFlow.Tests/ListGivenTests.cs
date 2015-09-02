﻿using Microsoft.SharePoint.Client;
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
        ListGivens _sut = new ListGivens(new Uri("http://rp2013-3:113"));
 
        [TestMethod]
        public void ListCreationIsIdempotent()
        {
            _sut.GivenThereIsListCalled(ListTemplateType.DocumentLibrary, "TestDocumentLibrary");

            _sut.GivenThereIsListCalled(ListTemplateType.DocumentLibrary, "TestDocumentLibrary");
        }

        [TestMethod]
        public void ListCreationThrowsWhenListAlreadyExistsButTypeIsWrong()
        {
            _sut.GivenThereIsListCalled(ListTemplateType.GenericList, "TestGenericList");

            bool thrown = false;
            try
            {
                _sut.GivenThereIsListCalled(ListTemplateType.DocumentLibrary, "TestGenericList");
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
            _sut.GivenThereIsListCalled(ListTemplateType.DocumentLibrary, "BDLBS_2");
            _sut.GivenTheListHasADocumentSetCalled("BDLBS_2", "BDLBS_DSB");

            _sut.GivenTheListHasADocumentSetCalled("BDLBS_2", "BDLBS_DSB");
        }

    }
}
