using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;
using System.Web.Mvc;
using HaveAVoice.Services.Helpers;

namespace HaveAVoice.Helpers.UI {
    public class IssueReplyCommentHelper {
        public static string BuildComment(IssueReplyComment aComment) {
            var clrDiv = new TagBuilder("div");
            clrDiv.MergeAttribute("class", "clear");
            clrDiv.InnerHtml += "&nbsp;";

            var wrprDiv = new TagBuilder("div");
            wrprDiv.MergeAttribute("class", "m-btm5 alt");

            var profileDiv = new TagBuilder("div");
            profileDiv.MergeAttribute("class", "push-6 col-3 center");

            var profileImg = new TagBuilder("img");
            profileImg.MergeAttribute("class", "profile");
            profileImg.MergeAttribute("alt", NameHelper.FullName(aComment.User));
            profileImg.MergeAttribute("src", PhotoHelper.ProfilePicture(aComment.User));

            profileDiv.InnerHtml += profileImg.ToString();
            wrprDiv.InnerHtml += profileDiv.ToString();

            var commentDiv = new TagBuilder("div");
            commentDiv.MergeAttribute("class", "push-6 m-lft col-12 m-rgt comment");

            var spanDirSpeak = new TagBuilder("span");
            spanDirSpeak.MergeAttribute("class", "speak-lft");
            spanDirSpeak.InnerHtml = "&nbsp;";

            commentDiv.InnerHtml += spanDirSpeak.ToString();

            var paddingDiv = new TagBuilder("div");
            paddingDiv.MergeAttribute("class", "p-a10");

            var href = new TagBuilder("a");
            href.MergeAttribute("class", "name");
            href.MergeAttribute("href", "#");
            href.InnerHtml += NameHelper.FullName(aComment.User);

            paddingDiv.InnerHtml += href.ToString();
            paddingDiv.InnerHtml += "&nbsp;";
            paddingDiv.InnerHtml += aComment.Comment;

            var divOptions = new TagBuilder("div");
            divOptions.MergeAttribute("class", "options p-v10");

            var divReport = new TagBuilder("div");
            divReport.MergeAttribute("class", "col-1 push-10");

            divReport.InnerHtml += ComplaintHelper.IssueReplyCommentLink(aComment.Id);
            divReport.InnerHtml += clrDiv.ToString();

            divOptions.InnerHtml += divReport.ToString();

            paddingDiv.InnerHtml += divOptions.ToString();

            commentDiv.InnerHtml += paddingDiv.ToString();
            wrprDiv.InnerHtml += commentDiv.ToString();

            var divTime = new TagBuilder("div");
            divTime.MergeAttribute("class", "push-6 col-3 date-tile");

            var divTimePad = new TagBuilder("div");
            divTimePad.MergeAttribute("class", "p-a10");

            var spanTime = new TagBuilder("span");
            spanTime.InnerHtml = aComment.DateTimeStamp.ToString("MMM").ToUpper();

            divTimePad.InnerHtml += spanTime.ToString();
            divTimePad.InnerHtml += "&nbsp;";
            divTimePad.InnerHtml += aComment.DateTimeStamp.ToString("dd");

            divTime.InnerHtml += divTimePad.ToString();

            wrprDiv.InnerHtml += divTime.ToString();

            wrprDiv.InnerHtml += clrDiv.ToString();

            return wrprDiv.ToString(TagRenderMode.Normal);
        }
    }
}