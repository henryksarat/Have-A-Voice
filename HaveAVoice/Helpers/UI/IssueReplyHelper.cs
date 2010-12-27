using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;
using System.Web.Mvc;

namespace HaveAVoice.Helpers.UI {
    public class IssueReplyHelper {
        public static string IssueReplyDisplay(IssueReply anIssueReply) {
        	var myLI = new TagBuilder("li");
        	myLI.InnerHtml = anIssueReply.Reply;

            return myLI;
        }
    }
}