using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;
using HaveAVoice.Helpers;
using HaveAVoice.Services.UserFeatures;

namespace HaveAVoice.Services.Helpers {
    public class PhotoHelper {
        public static string ProfilePicture(User aUser) {
            UserPicture myProfilePicture = (from u in aUser.UserPictures where u.ProfilePicture == true select u).FirstOrDefault<UserPicture>();
            string myProfileUrl = HAVConstants.NO_PROFILE_PICTURE_URL;
            if (myProfilePicture != null) {
                myProfileUrl = HAVConstants.USER_PICTURE_LOCATION_FROM_VIEW + myProfilePicture.ImageName;
            } else {
                IHAVUserPictureService myUserPictureService = new HAVUserPictureService();
                UserPicture myUserPicture = myUserPictureService.GetProfilePicutre(aUser);

                if (myUserPicture != null) {
                    myProfileUrl =  HAVConstants.USER_PICTURE_LOCATION_FROM_VIEW + myUserPicture.ImageName;
                }
            }

            return myProfileUrl;
        }

        public static string ConstructUrl(string anImageName) {
            return HAVConstants.USER_PICTURE_LOCATION_FROM_VIEW + anImageName;
        }
    }
}