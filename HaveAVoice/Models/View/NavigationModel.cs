using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Helpers;
using HaveAVoice.Services.Helpers;

namespace HaveAVoice.Models.View {
    public class NavigationModel {
        public User User { get; private set; }
        public string ProfilePictureUrl { get; private set; }
        public SiteSection SiteSection { get; private set; }

        public NavigationModel(User aUser, SiteSection aSection) {
            User = aUser;
            ProfilePictureUrl = PhotoHelper.ProfilePicture(aUser);
            SiteSection = aSection;
        }
    }
}