using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TechTalk.SpecFlow;

namespace SharePoint.SpecFlow
{
    [Binding]
    public class ListWhens : BindingBase
    {
        public ListWhens(Context ctx)
            : base(ctx)
        {
        }

        [When("the list called \"([^\"]*)\" contains ([0123456789]+) items as user ([^ ]+)")]
        public void WhenTheListCalledContainsItems(string listTitle, int expectedCount, string userName)
        {
            Context.LastListTitle = listTitle;
            Context.LastUserName = userName;
            WhenTheListContainsItems(expectedCount);
        }

        [When("the list called \"([^\"]*)\" contains ([0123456789]+) items")]
        public void WhenTheListCalledContainsItems(string listTitle, int expectedCount)
        {
            Context.LastListTitle = listTitle;
            WhenTheListContainsItems(expectedCount);
        }

        [When("the list contains ([0123456789]+) items")]
        public void WhenTheListContainsItems(int expectedCount)
        {
            int totalSecondsWaited = 0;

            using (var cc = Context.CreateClientContext())
            {
                while (totalSecondsWaited < Context.TimeoutSeconds)
                {
                    var list = cc.Web.Lists.GetByTitle(Context.LastListTitle);
                    cc.Load(list, l => l.ItemCount);
                    cc.ExecuteQuery();

                    if (list.ItemCount == expectedCount)
                        return;

                    Thread.Sleep(TimeSpan.FromSeconds(1));
                    totalSecondsWaited++;
                }

                throw new SharePointSpecFlowException("Timeout waiting for list " + Context.LastListTitle + " to contain " + expectedCount + " items.");
            }
        }
    }
}
