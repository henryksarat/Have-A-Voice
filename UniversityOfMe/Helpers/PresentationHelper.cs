using System.Text.RegularExpressions;

namespace UniversityOfMe.Helpers {
    public static class PresentationHelper {
        public static string ReplaceCarriageReturnWithBR(string aString) {
            Regex regex = new Regex(@"\r\n");
            return regex.Replace(aString, "<br />");
        }
    }
}