using System;
using System.Web.Mvc;
using HaveAVoice.Helpers;

namespace HaveAVoice.Helpers.UI {
    public class MessageHelper {
        public static string MessageList(int fromUserId, string fromUsername, int messageId, string subject, string body, DateTime dateTimeStamp, bool viewed) {
            var tableTag = new TagBuilder("table");
            tableTag.MergeAttribute("border", "0");
            tableTag.MergeAttribute("cellspacing", "0");
            tableTag.MergeAttribute("cellpadding", "0");
            tableTag.MergeAttribute("width", "600px");

            string rowStyle = "text-align:left";

            if (!viewed) {
                rowStyle += "; background-color:#ade0ff; color:Black";
            }
            var trTag = new TagBuilder("tr");       
            trTag.MergeAttribute("style", rowStyle);
          

            var tdTag = new TagBuilder("td");
            tdTag.MergeAttribute("width", "1%");
            tdTag.InnerHtml = CheckBoxPortion(messageId);
            trTag.InnerHtml += tdTag.ToString();

            tdTag = new TagBuilder("td");
            tdTag.MergeAttribute("width", "1%");
            tdTag.InnerHtml = UserInformationPortion(fromUsername);
            trTag.InnerHtml += tdTag.ToString();

            tdTag = new TagBuilder("td");
            tdTag.MergeAttribute("style", "vertical-align:top; width:98%");
            tdTag.InnerHtml = MessagePreview(messageId, subject, body, dateTimeStamp);
            trTag.InnerHtml += tdTag.ToString();

            tableTag.InnerHtml = trTag.ToString();

            return tableTag.ToString(TagRenderMode.Normal);
        }

        private static string CheckBoxPortion(int fromUserId) {
            var tableTag = new TagBuilder("table");
            tableTag.MergeAttribute("border", "0");
            tableTag.MergeAttribute("cellspacing", "0");
            tableTag.MergeAttribute("cellpadding", "3");
            tableTag.MergeAttribute("width", "100%");

            var trTag = new TagBuilder("tr");

            var tdTag = new TagBuilder("td");
            var checkboxTag = new TagBuilder("input");
            checkboxTag.MergeAttribute("type", "checkbox");
            checkboxTag.MergeAttribute("name", "selectedMessages");
            checkboxTag.MergeAttribute("value", fromUserId.ToString());
            tdTag.InnerHtml = checkboxTag.ToString();
            trTag.InnerHtml = tdTag.ToString();

            tableTag.InnerHtml = trTag.ToString();

            return tableTag.ToString(TagRenderMode.Normal);
        }

        private static string UserInformationPortion(string fromUsername) {
            var tableTag = new TagBuilder("table");
            tableTag.MergeAttribute("border", "0");
            tableTag.MergeAttribute("cellspacing", "0");
            tableTag.MergeAttribute("cellpadding", "3");
            tableTag.MergeAttribute("width", "100%");

            var trTag = new TagBuilder("tr");
            var tdTag = new TagBuilder("td");
            tdTag.InnerHtml = fromUsername;
            trTag.InnerHtml = tdTag.ToString();
            tableTag.InnerHtml += trTag.ToString();

            var imageTag = new TagBuilder("img");
            imageTag.MergeAttribute("src", "http://alyintelligent.com/images/avatars/henry_avatar.jpg");
            tdTag.InnerHtml = imageTag.ToString();
            trTag.InnerHtml = tdTag.ToString();
            tableTag.InnerHtml += trTag.ToString();

            return tableTag.ToString(TagRenderMode.Normal);
        }

        private static string MessagePreview(int messageId, string subject, string body, DateTime dateTimeStamp) {
            var tableTag = new TagBuilder("table");
            tableTag.MergeAttribute("border", "0");
            tableTag.MergeAttribute("cellspacing", "0");
            tableTag.MergeAttribute("cellpadding", "3");
            tableTag.MergeAttribute("width", "100%");

            var trTag = new TagBuilder("tr");

            var tdTag = new TagBuilder("td");
            tdTag.InnerHtml = String.Format("<a href=\"View/{0}\">{1}</a>", messageId, subject);
            trTag.InnerHtml = tdTag.ToString();
            tableTag.InnerHtml += trTag.ToString();

            tdTag = new TagBuilder("td");
            tdTag.InnerHtml = DateHelper.ConvertToLocalTime(dateTimeStamp).ToString();
            trTag.InnerHtml = tdTag.ToString();
            tableTag.InnerHtml += trTag.ToString();

            tdTag = new TagBuilder("td");
            tdTag.InnerHtml = String.Format("<a href=\"View/{0}\">{1}</a>", messageId, body);
            trTag.InnerHtml = tdTag.ToString();
            tableTag.InnerHtml += trTag.ToString();


            return tableTag.ToString(TagRenderMode.Normal);
        }


        public static string MessageItem(string fromUsername,  string subject, string body, DateTime dateTimeStamp) {
            var tableTag = new TagBuilder("table");
            tableTag.MergeAttribute("border", "0");
            tableTag.MergeAttribute("cellspacing", "0");
            tableTag.MergeAttribute("cellpadding", "0");
            tableTag.MergeAttribute("width", "600px");

            var trTag = new TagBuilder("tr");
            var tdTag = new TagBuilder("td");
            tdTag.MergeAttribute("width", "1%");
            tdTag.InnerHtml = UserInformationPortion(fromUsername);
            trTag.InnerHtml += tdTag.ToString();

            tdTag = new TagBuilder("td");
            tdTag.MergeAttribute("style", "vertical-align:top; width:99%");
            tdTag.InnerHtml = FullMessage(subject, body, dateTimeStamp);
            trTag.InnerHtml += tdTag.ToString();

            tableTag.InnerHtml = trTag.ToString();

            return tableTag.ToString(TagRenderMode.Normal);
        }

        private static string FullMessage(string subject, string body, DateTime dateTimeStamp) {
            var tableTag = new TagBuilder("table");
            tableTag.MergeAttribute("border", "0");
            tableTag.MergeAttribute("cellspacing", "0");
            tableTag.MergeAttribute("cellpadding", "3");
            tableTag.MergeAttribute("width", "100%");

            var trTag = new TagBuilder("tr");

            var tdTag = new TagBuilder("td");
            tdTag.InnerHtml = subject;
            trTag.InnerHtml = tdTag.ToString();
            tableTag.InnerHtml += trTag.ToString();

            tdTag = new TagBuilder("td");
            tdTag.InnerHtml = DateHelper.ConvertToLocalTime(dateTimeStamp).ToString();
            trTag.InnerHtml = tdTag.ToString();
            tableTag.InnerHtml += trTag.ToString();

            tdTag = new TagBuilder("td");
            tdTag.InnerHtml = body;
            trTag.InnerHtml = tdTag.ToString();
            tableTag.InnerHtml += trTag.ToString();


            return tableTag.ToString(TagRenderMode.Normal);
        }
    }
}
