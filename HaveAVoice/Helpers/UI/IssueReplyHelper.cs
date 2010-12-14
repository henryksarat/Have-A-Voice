using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;
using System.Web.Mvc;

namespace HaveAVoice.Helpers.UI {
    public class IssueReplyHelper {
        public static string IssueReplyDisplay(IssueReply anIssueReply) {
            var myTableTag = new TagBuilder("table");
            myTableTag.MergeAttribute("border", "0");
            myTableTag.MergeAttribute("cellspacing", "0");
            myTableTag.MergeAttribute("cellpadding", "0");
            myTableTag.MergeAttribute("width", "400px");

            var myTrTag = new TagBuilder("tr");
            var myTdTag = new TagBuilder("td");
            myTdTag.InnerHtml = anIssueReply.Reply;
            myTrTag.InnerHtml = myTdTag.ToString();

            myTableTag.InnerHtml = myTrTag.ToString();

            return myTableTag.ToString();
        }
    }
}