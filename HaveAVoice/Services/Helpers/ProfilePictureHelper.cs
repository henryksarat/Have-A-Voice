using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;
using HaveAVoice.Helpers;

namespace HaveAVoice.Services.Helpers {
    public class ProfilePictureHelper {
        public static string ProfilePicture(User aUser) {
            UserPicture myProfilePicture = (from u in aUser.UserPictures where u.ProfilePicture == true select u).FirstOrDefault<UserPicture>();
            if (myProfilePicture != null) {
                return HAVConstants.USER_PICTURE_LOCATION_FROM_VIEW + myProfilePicture.ImageName;
            } else {
                return HAVConstants.NO_PROFILE_PICTURE_URL;
            }       
        }
    }
}