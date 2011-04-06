using Social.Authentication.Helpers;
using UniversityOfMe.Models;

namespace UniversityOfMe.Helpers {
    public class ProfilePictureStrategy : IProfilePictureStrategy<User> {
        public string GetProfilePicture(User aUser) {
            return Social.Generic.Constants.Constants.NO_PROFILE_PICTURE_URL;
        }
    }
}