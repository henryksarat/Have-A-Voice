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
        public User PostingUser { get; set; }
        public DateTime DateTimeStamp { get; set; }
        public int UserId { get; set; }
        public string DisplayName { get; set; }
        public string ProfilePictureUrl { get; set; }

        public FeedModel(User aUser) {
            PostingUser = aUser;
            DisplayName = NameHelper.FullName(aUser);
            UserId = aUser.Id;
            ProfilePictureUrl = PhotoHelper.ProfilePicture(aUser);
        }
    }
}