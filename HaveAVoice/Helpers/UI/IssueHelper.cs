using System;
using System.Text;
using System.Web.Mvc;
using HaveAVoice.Helpers.Enums;
using HaveAVoice.Helpers.UserInformation;
using HaveAVoice.Models;
using HaveAVoice.Models.View;

namespace HaveAVoice.Helpers.UI {
    public class IssueHelper {
        public static string UserIssueReply(IssueReplyModel anIssueReply) {
            string rowStyle = "text-align:left; background-color:#ade0ff; color:Black";

            return BuildIssueReplyTable(anIssueReply, rowStyle);
        }

        public static string OfficialIssueReply(IssueReplyModel anIssueReply) {
            string rowStyle = "text-align:left; background-color:#9999FF; color:Black";

            return BuildIssueReplyTable(anIssueReply, rowStyle);
        }

        private static string BuildIssueReplyTable(IssueReplyModel anIssueReply, string aRowStyle) {
            var tableTag = new TagBuilder("table");
            tableTag.MergeAttribute("border", "0");
            tableTag.MergeAttribute("cellspacing", "0");
            tableTag.MergeAttribute("cellpadding", "0");
            tableTag.MergeAttribute("width", "400px");

            var trTag = new TagBuilder("tr");
            trTag.MergeAttribute("style", aRowStyle);

            var tdTag = new TagBuilder("td");
            tdTag.MergeAttribute("width", "50%");
            if(anIssueReply.Anonymous) {
                tdTag.InnerHtml = "<strong>Anonymous</strong>";
            } else {
                tdTag.InnerHtml = "<strong>" + anIssueReply.User.Username + "</strong>";
            }
            tdTag.InnerHtml += " @ " + anIssueReply.DateTimeStamp;
            trTag.InnerHtml += tdTag.ToString();

            tdTag = new TagBuilder("td");
            tdTag.MergeAttribute("width", "50%");
            tdTag.MergeAttribute("style", "text-align:right;");
            tdTag.InnerHtml = "<strong>" + anIssueReply.User.City + ", " + anIssueReply.User.State + "</strong>";
            trTag.InnerHtml += tdTag.ToString();

            tableTag.InnerHtml = trTag.ToString();

            trTag = new TagBuilder("tr");
            trTag.MergeAttribute("style", aRowStyle);
            tdTag = new TagBuilder("td");
            tdTag.MergeAttribute("width", "1%");
            tdTag.MergeAttribute("colspan", "2");
            tdTag.InnerHtml = anIssueReply.Reply;
            trTag.InnerHtml += tdTag.ToString();

            tableTag.InnerHtml += trTag.ToString();

            trTag = new TagBuilder("tr");
            trTag.MergeAttribute("style", aRowStyle);
            tdTag = new TagBuilder("td");
            tdTag.MergeAttribute("width", "50%");
            tdTag.InnerHtml = String.Format("<a href=\"/IssueReply/View/{0}\">{1}</a>", 
                anIssueReply.Id, 
                "Comments ( " + anIssueReply.CommentCount + " )");
            trTag.InnerHtml += tdTag.ToString();
            tdTag = new TagBuilder("td");
            tdTag.MergeAttribute("width", "50%");
            tdTag.InnerHtml = ComplaintHelper.IssueReplyLink(anIssueReply.Id);
            trTag.InnerHtml += tdTag.ToString();

            tableTag.InnerHtml += trTag.ToString();

            return tableTag.ToString(TagRenderMode.Normal);
        }

        public static string Comment(IssueReplyComment aComment) {
            var tableTag = new TagBuilder("table");
            tableTag.MergeAttribute("border", "0");
            tableTag.MergeAttribute("cellspacing", "0");
            tableTag.MergeAttribute("cellpadding", "0");
            tableTag.MergeAttribute("width", "400px");

            var trTag = new TagBuilder("tr");
            string rowStyle = "text-align:left; background-color:#6699CC; color:Black";
            trTag.MergeAttribute("style", rowStyle);

            var tdTag = new TagBuilder("td");
            tdTag.MergeAttribute("width", "1%");
            tdTag.InnerHtml = "<strong>" + aComment.User.Username + "</strong>";
            tdTag.InnerHtml += " @ " + aComment.DateTimeStamp;
            trTag.InnerHtml += tdTag.ToString();

            tableTag.InnerHtml = trTag.ToString();

            trTag = new TagBuilder("tr");
            trTag.MergeAttribute("style", rowStyle);
            tdTag = new TagBuilder("td");
            tdTag.MergeAttribute("width", "1%");
            tdTag.InnerHtml = aComment.Comment;
            trTag.InnerHtml += tdTag.ToString();

            tableTag.InnerHtml += trTag.ToString();

            trTag = new TagBuilder("tr");
            trTag.MergeAttribute("style", rowStyle);
            tdTag = new TagBuilder("td");
            tdTag.MergeAttribute("style", "text-align:right;");
            tdTag.MergeAttribute("width", "1%");
            tdTag.InnerHtml = ComplaintHelper.IssueReplyCommentLink(aComment.Id);
            trTag.InnerHtml += tdTag.ToString();

            tableTag.InnerHtml += trTag.ToString();

            return tableTag.ToString(TagRenderMode.Normal);
        }

        public static string IssueReply(IssueReplyDetailsModel anIssueReply) {
            var tableTag = new TagBuilder("table");
            tableTag.MergeAttribute("border", "0");
            tableTag.MergeAttribute("cellspacing", "0");
            tableTag.MergeAttribute("cellpadding", "0");
            tableTag.MergeAttribute("width", "400px");

            var trTag = new TagBuilder("tr");
            string rowStyle = "text-align:left; background-color:#CCCCFF; color:Black";
            trTag.MergeAttribute("style", rowStyle);

            var tdTag = new TagBuilder("td");
            tdTag.MergeAttribute("width", "1%");
            tdTag.InnerHtml = "<strong>Issue</strong>" + anIssueReply.IssueReply.Issue.Title;
            trTag.InnerHtml += tdTag.ToString();

            tableTag.InnerHtml = trTag.ToString();

            trTag = new TagBuilder("tr");
            trTag.MergeAttribute("style", rowStyle);
            tdTag = new TagBuilder("td");
            tdTag.MergeAttribute("width", "1%");
            tdTag.InnerHtml = anIssueReply.IssueReply.Issue.Description;
            trTag.InnerHtml += tdTag.ToString();

            tableTag.InnerHtml += trTag.ToString();

            rowStyle = "text-align:left; height:120px; background-color:#CCCCCC; color:Black";

            trTag = new TagBuilder("tr");
            trTag.MergeAttribute("style", rowStyle);
            tdTag = new TagBuilder("td");
            tdTag.MergeAttribute("width", "1%");
            tdTag.InnerHtml = anIssueReply.IssueReply.Reply;
            trTag.InnerHtml += tdTag.ToString();

            tableTag.InnerHtml += trTag.ToString();

            return tableTag.ToString(TagRenderMode.Normal);
        }

        public static string BuildIssueDisplay(Issue anIssue, bool anIsLike) {
            UserPrivacySetting myUserPrivacy = UserInformationHelper.GetPrivacySettings(anIssue.User);

            string myAvatarURL ="http://images.chron.com/photos/2008/05/19/graphic_defaultAvatar/graphic_defaultAvatar.jpg";
            string myUsername = "Anonymous";
            if (UserInformationHelper.IsPrivacyAllowing(anIssue.User, PrivacyAction.DisplayProfile)) {
                myAvatarURL ="http://wedonetwork.co.uk/wedotech/wp-content/uploads/2010/08/master-chief-badass.jpg";
                myUsername = anIssue.User.Username;
            } 

            var myOuterDiv = new TagBuilder("div");
            string myStyle = "grid_10 omega margin-32b font-12";
            if (anIsLike) {
                myStyle += " alpha";
            }
            myOuterDiv.MergeAttribute("class", myStyle);

            var myDivImageWrapper = new TagBuilder("div");
            myDivImageWrapper.MergeAttribute("class", "alpha grid_2 omega");
            myDivImageWrapper.InnerHtml = "<img src=\"" + myAvatarURL + "\" alt=\"Hitomi Tanaka\" width=\"50\" height=\"50\"  />";
            myOuterDiv.InnerHtml = myDivImageWrapper.ToString();

            var myContextDiv = new TagBuilder("div");
            myContextDiv.MergeAttribute("class", "alpha grid_8");
            var myContextSpan = new TagBuilder("span");
            myContextSpan.MergeAttribute("class", "teal2");
            myContextSpan.InnerHtml = myUsername + ": ";
            myContextDiv.InnerHtml = myContextSpan.ToString(); 
            myContextDiv.InnerHtml += anIssue.Title;
            var myParagraph = new TagBuilder("p");
            myParagraph.MergeAttribute("class", "grey padding-6v");
            myParagraph.InnerHtml = "On 12/21/2012 by Hitomi Tanaka - United States (Illinois)";
            myContextDiv.InnerHtml += myParagraph.ToString();
            var myLikeDiv = new TagBuilder("div");
            myLikeDiv.MergeAttribute("class", "alpha grid_3");
            string myLike = new StringBuilder().AppendFormat("<a href=\"/Issue/IssueDisposition?issueId={0}&disposition={1}\" class=\"like\"><img src=\"/Content/images/like-sm.png\" alt=\"like\" />(9999) Like</a>", anIssue.Id, Disposition.LIKE).ToString();
            myLikeDiv.InnerHtml = myLike;
            myContextDiv.InnerHtml += myLikeDiv.ToString();
            var myDislikeDiv = new TagBuilder("div");
            myDislikeDiv.MergeAttribute("class", "grid_3 omega");
            string myDislike = new StringBuilder().AppendFormat("<a href=\"/Issue/IssueDisposition?issueId={0}&disposition={1}\" class=\"dislike\"><img src=\"/Content/images/dislike-sm.png\" alt=\"dislike\" />(0) Dislike</a>", anIssue.Id, Disposition.DISLIKE).ToString();
            myDislikeDiv.InnerHtml = myDislike;
            myContextDiv.InnerHtml += myDislikeDiv.ToString();
            myOuterDiv.InnerHtml += myContextDiv.ToString();


            var myClearDiv = new TagBuilder("div");
            myClearDiv.MergeAttribute("class", "clear");

            return myOuterDiv.ToString() + myClearDiv.ToString();
        }
    }
}
