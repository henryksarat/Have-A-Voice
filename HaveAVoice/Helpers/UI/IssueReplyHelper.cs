using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;
using System.Web.Mvc;

namespace HaveAVoice.Helpers.UI {
    public class IssueReplyHelper {
        public static string IssueReplyDisplay(IEnumerable<IssueReply> anIssueReplies) {
            string myList = string.Empty;

            foreach (IssueReply myIssueReply in anIssueReplies) {
                var myLI = new TagBuilder("li");
                myLI.InnerHtml = myIssueReply.Reply;
                myList += myLI.ToString();
            }


            return myList;
        }
    }
}