using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UniversityOfMe.Models;
using System.Web.Mvc;

namespace UniversityOfMe.Helpers.Email {
    public class EmailContent {
        public static string ClassBoardReplySubject(Class aClass) {
            return "UofMe: New reply to your class question in " + aClass.ClassCode;
        }

        public static string ClassBoardReplyBody(Class aClass, ClassBoard aBoard) {
            var myClassUrl = new TagBuilder("a");
            myClassUrl.MergeAttribute("href", URLHelper.BuildClassBoardUrl(aBoard));
            myClassUrl.InnerHtml = aClass.ClassCode;

            var myClassBoardUrl = new TagBuilder("a");
            myClassUrl.MergeAttribute("href", URLHelper.BuildClassBoardUrl(aBoard));
            myClassUrl.InnerHtml = "Click here to go to your post";

            string myBody = "Hey!, <br /><br /> Someone posted a reply to your question in " + myClassUrl.ToString()
                + ". " + myClassBoardUrl.ToString() + " or copy and paste this into your browser: " + URLHelper.BuildClassBoardUrl(aBoard)
                + "<br /><br />-UniversityOf.Me";

            return myBody;
        }

        public static string FriendRequestSubject() {
            return "UofMe: New friend request!";
        }

        public static string FriendRequestBody(User aUserAdding) {
            var myLink = new TagBuilder("a");
            myLink.MergeAttribute("href", "http://www.universityof.me");
            myLink.InnerHtml = "universityof.me";

            string mySubject = EmailContent.FriendRequestSubject();

            string myBody = "Hey!, <br /><br /> " + NameHelper.FullName(aUserAdding) + " has sent you a friend request. Login to "
                + myLink.ToString() + " to accept them.";

            return myBody;
        }

        public static string NewBoardSubject() {
            return "UofMe: Someone posted to your profile board!";
        }

        public static string NewBoardBody(User aPostedByUser, User aSourceUser) {
            var myLink = new TagBuilder("a");
            myLink.MergeAttribute("href", URLHelper.ProfileUrl(aSourceUser));
            myLink.InnerHtml = "Click here to go to your profile page";

            string myBody = "Hey!, <br /><br /> " + NameHelper.FullName(aPostedByUser) + " has posted to your board. "
                + myLink.ToString() + ". Or copy and paste this URL into your browser: " + NameHelper.FullName(aPostedByUser);

            return myBody;
        }
    }
}