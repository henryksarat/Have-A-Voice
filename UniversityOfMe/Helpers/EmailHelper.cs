namespace UniversityOfMe.Helpers {
    public class EmailHelper {
        public static string ExtractEmailExtension(string anEmailAddress) {
            int myLengthOfEmailExtension = anEmailAddress.Length - anEmailAddress.IndexOf('@') - 1;
            return anEmailAddress.Substring(anEmailAddress.IndexOf('@') + 1, myLengthOfEmailExtension);
        }
    }
}
