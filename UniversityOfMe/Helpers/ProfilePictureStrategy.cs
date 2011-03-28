using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Social.Authentication.Helpers;
using UniversityOfMe.Models;
using Social.Generic;
using Social.Generic.Constants;

namespace UniversityOfMe.Helpers {
    public class ProfilePictureStrategy : IProfilePictureStrategy<User> {
        public string GetProfilePicture(User aUser) {
            return Constants.NO_PROFILE_PICTURE_URL;
        }
    }
}