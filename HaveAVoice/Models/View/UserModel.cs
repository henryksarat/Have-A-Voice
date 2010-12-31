using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Helpers;
using HaveAVoice.Services.Helpers;

namespace HaveAVoice.Models.View {
    public class UserModel {
        public User User { get; private set; }
        public string ProfilePictureUrl { get; private set; }

        public UserModel(User aUser) {
            User = aUser;
            ProfilePictureUrl = ProfilePictureHelper.ProfilePicture(aUser);
        }
    }
}