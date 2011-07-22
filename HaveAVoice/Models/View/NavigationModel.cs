using System.Collections.Generic;
using HaveAVoice.Helpers;
using HaveAVoice.Services.Helpers;
using HaveAVoice.Util;

namespace HaveAVoice.Models.View {
    public class NavigationModel {
        public IEnumerable<UserNavigationMenuModel> UserMenuMetaData { get; set; }
        public NavigationItemModel FanMetaData { get; set; }
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
        public bool IsMyOwnProfile { get; set; }

        public NavigationModel(User aUser, SiteSection aSection, bool aMyOwnProfile) {
            IsMyOwnProfile = aMyOwnProfile;
            User = aUser;
            ProfilePictureUrl = PhotoHelper.ProfilePicture(aUser);
            FullName = NameHelper.FullName(aUser);
            SiteSection = aSection;

            SiteSection myHomeSiteSection = IsMyOwnProfile ? SiteSection.MyProfile : SiteSection.Home;
            UserNavigationMenuModel myHomeMenuItem = new UserNavigationMenuModel(myHomeSiteSection, UserPanelNav.HOME_GREY_BUTTON);
            UserNavigationMenuModel myIssueActivityMenuItem = new UserNavigationMenuModel(SiteSection.IssueActivity, UserPanelNav.ISSUE_ACTIVITY_GREY_BUTTON);
            UserNavigationMenuModel myPhotoMenuItem = new UserNavigationMenuModel(SiteSection.Photos, UserPanelNav.PHOTO_GREY_BUTTON);
            UserNavigationMenuModel myEventMenuItem = new UserNavigationMenuModel(SiteSection.Calendar, UserPanelNav.CALENDAR_GREY_BUTTON);

            List<UserNavigationMenuModel> myMenu = new List<UserNavigationMenuModel>();
            myMenu.Add(myHomeMenuItem);
            myMenu.Add(myIssueActivityMenuItem);
            myMenu.Add(myPhotoMenuItem);
            myMenu.Add(myEventMenuItem);

            UserMenuMetaData = myMenu;
        }
    }
}