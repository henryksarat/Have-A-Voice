using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;
using System.Web.Mvc;
using HaveAVoice.Helpers;

namespace HaveAVoice.Models.View {
    public class UserPicturesModel {
        public int UserId { get; set; }
        public string ProfilePictureURL { get; set; }
        public IEnumerable<UserPicture> UserPictures { get; set; }
        public List<int> SelectedUserPictures { get; set; }

        public UserPicturesModel() {
            this.ProfilePictureURL = HAVConstants.NO_PROFILE_PICTURE_URL;
            this.UserPictures = new List<UserPicture>();
            this.SelectedUserPictures = new List<int>();
        }
    }
}
