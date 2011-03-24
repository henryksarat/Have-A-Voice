namespace Social.Generic.Helpers {
    public class HashHelper {
        public static string DoHash(string aString) {
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(aString, "SHA1");
        }
    }
}