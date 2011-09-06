using System.Web.Mvc;

namespace UniversityOfMe.Helpers {
    public class MessageHelper {
        public static string NormalMessage(string aMessage) {
            return BuildMessage("msg clearfix", "Information!", aMessage);
        }

        public static string SuccessMessage(string aMessage) {
            return BuildMessage("msg sccss clearfix", "Success!", aMessage);
        }

        public static string WarningMessage(string aMessage) {
            return BuildMessage("msg warn clearfix", "Warning!", aMessage);
        }

        public static string ErrorMessage(string aMessage) {
            return BuildMessage("msg err clearfix", "Error!", aMessage);
        }

        private static string BuildMessage(string aMessageDivClass, string aMessageType, string aMessage) {
            var myMessageDiv = new TagBuilder("div");
            myMessageDiv.MergeAttribute("id", "message");
            myMessageDiv.MergeAttribute("class", aMessageDivClass);

            var myIconDiv = new TagBuilder("div");
            myIconDiv.MergeAttribute("class", "icon");
            myIconDiv.SetInnerText(" ");

            var myInnerMessageDiv = new TagBuilder("div");
            myInnerMessageDiv.MergeAttribute("class", "cnt");

            var myCloseLink = new TagBuilder("a");
            myCloseLink.MergeAttribute("href", "#");
            myCloseLink.MergeAttribute("class", "close");
            myCloseLink.MergeAttribute("title", "Close");
            myCloseLink.SetInnerText("X");

            var myHeading = new TagBuilder("h6");
            myHeading.SetInnerText(aMessageType);

            myInnerMessageDiv.InnerHtml += myCloseLink.ToString();
            myInnerMessageDiv.InnerHtml += myHeading.ToString();
            myInnerMessageDiv.InnerHtml += aMessage;

            myMessageDiv.InnerHtml += myIconDiv;
            myMessageDiv.InnerHtml += myInnerMessageDiv;

            return myMessageDiv.ToString();
        }
    }
}