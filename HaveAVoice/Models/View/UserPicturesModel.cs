using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;
using System.Web.Mvc;
using HaveAVoice.Helpers;

namespace HaveAVoice.Models.View {
    public class UserPicturesModel {
        public string ProfilePictureURL { get; set; }
        public IEnumerable<UserPicture> UserPictures { get; set; }
        public List<int> SelectedUserPictures { get; set; }

        public UserPicturesModel(UserPicture aProfilePicture, IEnumerable<UserPicture> aUserPictures, List<int> aSelectedUserPictures) {
            this.ProfilePictureURL = HAVConstants.USER_PICTURE_LOCATION + "/" + aProfilePicture.ImageName;
            this.UserPictures = aUserPictures;
            this.SelectedUserPictures = aSelectedUserPictures;
        }
    }
}
