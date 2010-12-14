using System;
using System.Web.Mvc;
using HaveAVoice.Helpers;
using System.Web.Routing;
using HaveAVoice.Models;

namespace HaveAVoice.Helpers.UI {
    public class UserHelper {
        public static bool UserIsNull(User aUser) {
            return aUser == null;
        }

        public static string ListenImage(int listenToUserId, string listenToUsername, bool canListen) {
            if (canListen) {
                return ImageHelper.ImageLink("Listener", 
                    "ListenToUser",
                    "?listenToUserId=" + listenToUserId, 
                    "http://alyintelligent.com/images/add_buddy.jpg", 
                    "Listen to the userToListenTo " + listenToUsername, 
                    "border: none", 
                    null);
            }

            return null;

        }

        public static string MessageImage(int toUserId, string toUsername, bool canMessage) {
            if (canMessage) {
                return ImageHelper.ImageLink("Message", 
                    "SendMessage", 
                    "?toUserId=" + toUserId, 
                    "http://alyintelligent.com/images/icon_Sendmail.jpg", 
                    "Message the userToListenTo" + toUsername, 
                    "border: none", 
                    null);
            }

            return null;

        }

        public static String UserAgreement() {
            return "There are the terms, agree to this or die!";
        }
       
    }
}
