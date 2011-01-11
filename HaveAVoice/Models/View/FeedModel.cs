using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Helpers.Enums;
using HaveAVoice.Helpers;
using HaveAVoice.Services.Helpers;

namespace HaveAVoice.Models.View {
    public abstract class FeedModel {
        public int Id { get; set; }
        public DateTime DateTimeStamp { get; set; }
        public string Username { get; set; }
        public string ProfilePictureUrl { get; set; }

        public FeedModel(User aUser) {
            Username = aUser.Username;
            ProfilePictureUrl = PhotoHelper.ProfilePicture(aUser);
        }
    }
}