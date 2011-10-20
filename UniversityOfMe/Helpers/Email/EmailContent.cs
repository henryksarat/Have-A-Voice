using System.Web.Mvc;
using UniversityOfMe.Models;

namespace UniversityOfMe.Helpers.Email {
    public class EmailContent {
        public static string ClassBoardReplySubject(Class aClass) {
            return "UofMe: New reply to your class question in " + aClass.Course;
        }

        public static string FriendRequestSubject() {
            return "UofMe: New friend request!";
        }

        public static string FriendRequestBody(User aUserAdding) {
            var myLink = new TagBuilder("a");
            myLink.MergeAttribute("href", "http://www.universityof.me");
            myLink.InnerHtml = "universityof.me";

            string mySubject = EmailContent.FriendRequestSubject();

            string myBody = "Hey! <br /><br /> " + NameHelper.FullName(aUserAdding) + " has sent you a friend request. Login to "
                + myLink.ToString() + " to accept them. <br /><br />-UniversityOf.Me team";

            return myBody;
        }

        public static string NewBoardSubject() {
            return "UofMe: Someone posted to your profile board!";
        }

        public static string NewBoardBody(User aPostedByUser, User aSourceUser) {
            var myLink = new TagBuilder("a");
            myLink.MergeAttribute("href", LinkIt(URLHelper.ProfileUrl(aSourceUser)));
            myLink.InnerHtml = "Click here to go to your profile page";

            string myBody = "Hey! <br /><br /> " + NameHelper.FullName(aPostedByUser) + " has posted to your board. "
                + myLink.ToString() + ". Or copy and paste this URL into your browser: " + LinkIt(URLHelper.ProfileUrl(aSourceUser))
                + "<br /><br />-UniversityOf.Me team";

            return myBody;
        }

        public static string NewMessageSubject() {
            return "UofMe: Someone sent you a private message";
        }

        public static string NewMessageBody(User aUserSending) {
            var myLink = new TagBuilder("a");
            myLink.MergeAttribute("href", "http://www.universityof.me");
            myLink.InnerHtml = "UniversityOf.Me";

            string myBody = "Hey! <br /><br /> " + NameHelper.FullName(aUserSending) + " has sent you a private message. Login to "
                + myLink.ToString() + " to read it. <br /><br />-UniversityOf.Me team";

            return myBody;
        }

        public static string AnonymousFlirtSubject() {
            return "UofMe: Someone sent you an anonymous flirt!";
        }

        public static string AnonymousFlirtBody(string aUniversityId, string anAdjective, string aSomethingDelicious, 
            string anAnimal, string aHairColor, string aGender, 
            string aLocation, string aMessage) {

            var myLink = new TagBuilder("a");
            myLink.MergeAttribute("href", "http://www.universityof.me");
            myLink.InnerHtml = "UniversityOf.Me";

            var myFlirtLinks = new TagBuilder("a");
            myFlirtLinks.MergeAttribute("href", LinkIt(URLHelper.AnonymousFlirtUrl(aUniversityId)));
            myFlirtLinks.InnerHtml = "Click here to see the other anonymous flirts sent within your school";

            string myFlirt = anAdjective + " " + aSomethingDelicious + " " + anAnimal + ", that "
                                    + (aHairColor.Equals("Dunno") ? string.Empty : aHairColor + " Haired ")
                                    + aGender
                                    + (string.IsNullOrEmpty(aLocation) ? string.Empty : " i saw at " + aLocation) 
                                    + " - " + aMessage;

            string myBody = "Hey!, <br /><br /> Someone sent you an anonymous flirt! Here is what the flirt says with your pet name: <br /><br /> "
                + myFlirt
                + "<br /><br />"
                + myFlirtLinks.ToString() + " or copy and paste this into your browser: " + LinkIt(URLHelper.AnonymousFlirtUrl(aUniversityId))
                + "<br /><br />-UniversityOf.Me team";

            return myBody;
        }

        private static string LinkIt(string aLink) {
            return "http://www.universityof.me" + aLink;
        }
    }
}