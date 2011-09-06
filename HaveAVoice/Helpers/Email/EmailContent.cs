using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;
using HaveAVoice.Helpers.Configuration;
using HaveAVoice.Helpers.UI;
using System.Web.Mvc;

namespace HaveAVoice.Helpers.Email {
    public static class EmailContent {
        public static string NewReplySubject(Issue anIssue) {
            return "Have A Voice: Someone replied to your issue on " + anIssue.Title;
        }

        public static string NewReplyBody(Issue anIssue) {           
            var myLink = new TagBuilder("a");
            myLink.MergeAttribute("href", LinkHelper.IssueUrl(anIssue.Title));
            myLink.InnerHtml = "Click here to go to your issue";

            string myBody = "Hey!, <br /><br /> Someone posted a reply to your issue on:  " + anIssue.Title + ". "
                + myLink.ToString() + " or copy and paste this into your browser: " + LinkHelper.IssueUrl(anIssue.Title)
                + "<br /><br />-Have A Voice team";

            return myBody;
        }

        public static string NewBoardPostSubject() {
            return "Have A Voice: Posted to your profile board!";
        }

        public static string NewBoardPostBody(User aPostedByUser, User aSourceUserId) {
            var myLink = new TagBuilder("a");
            myLink.MergeAttribute("href", LinkHelper.Profile(aSourceUserId));
            myLink.InnerHtml = "Click here to go to your profile page";


            string myBody = "Hey!, <br /><br /> " + NameHelper.FullName(aPostedByUser) + " has posted to your board. "
                + myLink.ToString() + " or copy and paste this URL into your browser: " + LinkHelper.Profile(aSourceUserId)
                + "<br /><br />-Have A Voice team";

            return myBody;
        }

        public static string GroupBoardSubject(Group aGroup) {
            return "Have A Voice: New post to group " + aGroup.Name;
        }

        public static string GroupBoardBody(Group aGroup) {
            var myLink = new TagBuilder("a");
            myLink.MergeAttribute("href", LinkHelper.GroupUrl(aGroup));
            myLink.InnerHtml = "Click here to go to the group";

            string myBody = "Hey!, <br /><br /> There is a new post to the baord " + aGroup.Name + ". "
                + myLink.ToString() + " or copy and paste this URL into your browser: " + LinkHelper.GroupUrl(aGroup)
                + "<br /><br />-Have A Voice team";

            return myBody;
        }

        public static string NewGroupMemberSubject(Group aGroup) {
            return "Have A Voice: Someone wants to join the group " + aGroup.Name;
        }

        public static string NewGroupMemberBody(User aMemberJoining, Group aGroup) {
            var myLink = new TagBuilder("a");
            myLink.MergeAttribute("href", LinkHelper.GroupUrl(aGroup));
            myLink.InnerHtml = "Click here to go to the group";

            string myBody = "Hey!, <br /><br />  " + NameHelper.FullName(aMemberJoining)
                + " wants to join the group " + aGroup.Name + ". You are an admin so you should either approve or deny their membership. "
                + myLink.ToString() + " or copy and paste this URL into your browser: " + LinkHelper.GroupUrl(aGroup)
                + "<br /><br />-Have A Voice team";

            return myBody;
        }

        public static string AcceptedIntoGroupSubject(Group aGroup) {
            return "Have A Voice: You have been accepted into the group " + aGroup.Name;
        }

        public static string AcceptedIntoGroupBody(Group aGroup) {
            var myLink = new TagBuilder("a");
            myLink.MergeAttribute("href", LinkHelper.GroupUrl(aGroup));
            myLink.InnerHtml = "Click here to go to the group";

            string myBody = "Hey!, <br /><br />  You have been accepted into the group " + aGroup.Name
                + myLink.ToString() + " or copy and paste this URL into your browser: " + LinkHelper.GroupUrl(aGroup)
                + "<br /><br />-Have A Voice team";

            return myBody;
        }

        public static string FriendRequestSubject() {
            return "Have A Voice: You have a new friend request!";
        }

        public static string FriendRequestBody(User aUserAdding) {
            var myLink = new TagBuilder("a");
            myLink.MergeAttribute("href", "http://www.haveavoice.com");
            myLink.InnerHtml = "haveavoice.com";

            string myBody = "Hey!, <br /><br /> " + NameHelper.FullName(aUserAdding) + " has sent you a friend request. Login to "
                + myLink.ToString() + " to accept them. <br /><br />-Have A Voice team";

            return myBody;
        }
    }
}