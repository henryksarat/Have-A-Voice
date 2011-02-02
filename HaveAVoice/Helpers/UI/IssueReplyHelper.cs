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

            int myCount = 0;

            foreach (IssueReply myIssueReply in anIssueReplies) {
                if (myCount >= 4) {
                    break;
                }
                var myLI = new TagBuilder("li");
                myLI.InnerHtml = myIssueReply.Reply;
                myList += myLI.ToString();

                myCount++;
            }


            return myList;
        }
    }
}