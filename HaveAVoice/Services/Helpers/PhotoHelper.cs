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
            Photo myProfilePicture = (from u in aUser.Photos where u.ProfilePicture == true select u).FirstOrDefault<Photo>();
            string myProfileUrl = HAVConstants.NO_PROFILE_PICTURE_URL;
            if (myProfilePicture != null) {
                myProfileUrl = HAVConstants.PHOTO_LOCATION_FROM_VIEW + myProfilePicture.ImageName;
            } else {
                IHAVPhotoService myPhotoService = new HAVPhotoService();
                Photo myPhoto = myPhotoService.GetProfilePicutre(aUser);

                if (myPhoto != null) {
                    myProfileUrl =  HAVConstants.PHOTO_LOCATION_FROM_VIEW + myPhoto.ImageName;
                }
            }

            return myProfileUrl;
        }

        public static string ConstructUrl(string anImageName) {
            return HAVConstants.PHOTO_LOCATION_FROM_VIEW + anImageName;
        }
    }
}