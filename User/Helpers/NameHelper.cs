using Social.Generic.Models;

namespace Social.User.Helpers {
    public class NameHelper<U> {
        public static string FullName(AbstractUserModel<U> aUser) {
            return aUser.FirstName + " " + aUser.LastName;
        }
    }
}
