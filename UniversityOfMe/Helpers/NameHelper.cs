using Social.User.Helpers;
using UniversityOfMe.Models;
using UniversityOfMe.Models.SocialModels;

namespace UniversityOfMe.Helpers {
    public class NameHelper {
        public static string FullName(User aUser) {
            return NameHelper<User>.FullName(SocialUserModel.Create(aUser));
        }

        public static string Anonymous() {
            return Social.Generic.Constants.Constants.ANONYMOUS;
        }
    }
}