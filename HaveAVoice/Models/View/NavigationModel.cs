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
        public string FullName { get; private set; }
        public SiteSection SiteSection { get; private set; }
        public Issue LocalIssue { get; set; }
        public string LocalIssueLocation {
            get {
                string myLocation = "Country";

                if (User.City.ToUpper().Equals(LocalIssue.City.ToUpper()) && User.State.ToUpper().Equals(LocalIssue.State.ToUpper())) {
                    myLocation = "City";
                } else if (User.State.ToUpper().Equals(LocalIssue.State.ToUpper())) {
                    myLocation = "State";
                }

                return myLocation;
            }
        }

        public NavigationModel(User aUser, SiteSection aSection) {
            User = aUser;
            ProfilePictureUrl = PhotoHelper.ProfilePicture(aUser);
            FullName = NameHelper.FullName(aUser);
            SiteSection = aSection;
        }
    }
}