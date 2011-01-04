using System;
using System.Web.Mvc;
using HaveAVoice.Helpers;

namespace HaveAVoice.Helpers.UI {
    public class MessageHelper {
        public static string MessageList(int fromUserId, string fromUsername, int messageId, string subject, string body, DateTime dateTimeStamp, bool viewed) {
        	var wrprDiv = new TagBuilder("div");
        	if (!viewed) {
        		wrprDiv.MergeAttribute("class", "mail new");
        	} else {
        		wrprDiv.MergeAttribute("class", "mail");
        	}
        	
        	var chkDiv = new TagBuilder("div");
        	chkDiv.MergeAttribute("class", "col-1");
        	chkDiv.InnerHtml = CheckBoxPortion(messageId);
        	
        	wrprDiv.InnerHtml += chkDiv.ToString();
        	
        	var imgDiv = new TagBuilder("div");
        	imgDiv.MergeAttribute("class", "col-1");
        	imgDiv.InnerHtml = UserInformationPortion(fromUsername);

        	wrprDiv.InnerHtml += imgDiv.ToString();
        	
        	var dtwrprDiv = new TagBuilder("div");
        	dtwrprDiv.MergeAttribute("class", "col-4 m-lft m-rgt");
        	
        	var userLinkDiv = new TagBuilder("div");
        	userLinkDiv.MergeAttribute("class", "p-t10");
        	userLinkDiv.InnerHtml = String.Format("<a href=\"View/{0}\">{1}</a>", messageId, fromUsername);
        	
        	var clrDiv = new TagBuilder("div");
        	clrDiv.MergeAttribute("class", "clear");
        	clrDiv.InnerHtml = "&nbsp;";
        	
			var dtDiv = new TagBuilder("div");
			dtDiv.MergeAttribute("class", "p-b10");
			dtDiv.InnerHtml = DateHelper.ConvertToLocalTime(dateTimeStamp).ToString();
			
			dtwrprDiv.InnerHtml += userLinkDiv.ToString();
			dtwrprDiv.InnerHtml += clrDiv.ToString();
			dtwrprDiv.InnerHtml += dtDiv.ToString();
			
			wrprDiv.InnerHtml += dtwrprDiv.ToString();
			
        	var prvDiv = new TagBuilder("div");
            prvDiv.MergeAttribute("class", "col-13 m-lft m-rgt");
        	
        	var sbjDiv = new TagBuilder("div");
        	sbjDiv.MergeAttribute("class", "p-t10");
        	sbjDiv.InnerHtml = String.Format("<a href=\"View/{0}\">{1}</a>", messageId, subject);
        	
        	var bdyDiv = new TagBuilder("div");
        	bdyDiv.MergeAttribute("class", "p-b10");
        	bdyDiv.InnerHtml = body;
        	
        	prvDiv.InnerHtml += sbjDiv.ToString();
        	prvDiv.InnerHtml += clrDiv.ToString();
        	prvDiv.InnerHtml += bdyDiv.ToString();
        	
        	wrprDiv.InnerHtml += prvDiv.ToString();
        	
        	var dltDiv = new TagBuilder("div");
            dltDiv.MergeAttribute("class", "col-2 m-lft m-rgt p-v10 right");
        	dltDiv.InnerHtml = "&nbsp;";

			wrprDiv.InnerHtml += dltDiv.ToString();
			wrprDiv.InnerHtml += clrDiv.ToString();

            return wrprDiv.ToString(TagRenderMode.Normal);
        }

        private static string CheckBoxPortion(int fromUserId) {
            var checkboxTag = new TagBuilder("input");
            checkboxTag.MergeAttribute("type", "checkbox");
            checkboxTag.MergeAttribute("name", "selectedMessages");
            checkboxTag.MergeAttribute("value", fromUserId.ToString());

            return checkboxTag.ToString(TagRenderMode.Normal);
        }

        private static string UserInformationPortion(string fromUsername) {
            var imageTag = new TagBuilder("img");
            /* HENRYK UPDATE URL TO BE THE ACTUAL USER IMAGE */
            imageTag.MergeAttribute("src", "http://alyintelligent.com/images/avatars/henry_avatar.jpg");
            imageTag.MergeAttribute("class", "profile sm");

            return imageTag.ToString(TagRenderMode.Normal);
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
