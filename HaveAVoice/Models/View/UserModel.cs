using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Helpers;

namespace HaveAVoice.Models.View {
    public class UserModel {
        public User User { get; private set; }
        public string ProfilePictureUrl { get; set; }

        public UserModel(User aUser) {
            User = aUser;
            ProfilePictureUrl = HAVConstants.NO_PROFILE_PICTURE_URL;
        }
    }
}