﻿using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace SharePoint.SpecFlow
{
    [Binding]
    public class FileGivens
    {
        public FileGivens()
            : this( ScenarioContext.Current.GetWebUri() )
        {

        }

        public FileGivens(Uri currentWebUri)
        {
            _currentWebUri = currentWebUri;
        }

        [Given("there is a file with contents \"(.*?)\" at server relative url \"(.*?)\"")]
        public void GivenThereIsAFileWithUrl(string contents, string url)
        {
            var cc = new ClientContext(_currentWebUri);

            var rawContents = Encoding.Default.GetBytes(contents);

            File.SaveBinaryDirect(cc, url, new System.IO.MemoryStream(rawContents), true);
        }

        private Uri _currentWebUri;
    }
}