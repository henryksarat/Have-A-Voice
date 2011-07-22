using System.Collections.Generic;
using HaveAVoice.Helpers;
using HaveAVoice.Services.Helpers;
using HaveAVoice.Util;

namespace HaveAVoice.Models.View {
    public class LoggedInModel {
        public NavigationModel NavigationModel { get; private set; }
        private bool theIsMyProfile;

        public LoggedInModel(User aPanelForUser, User aLoggedInUser, SiteSection aSection) {
            theIsMyProfile = aPanelForUser.Id == aLoggedInUser.Id;

            NavigationModel = new NavigationModel(aPanelForUser, aSection, theIsMyProfile);

            if(PrivacyHelper.IsAllowed(aPanelForUser, Helpers.Enums.PrivacyAction.DisplayProfile)) {
                BuildMenu(aPanelForUser, aLoggedInUser);
            }

            BuildFanNavigationItem(aPanelForUser, aLoggedInUser, aSection);
        }

        private void BuildFanNavigationItem(User aPanelForUser, User aLoggedInUser, SiteSection aSection) {
            if (aLoggedInUser != null && IsAProfilePage(aSection) && !theIsMyProfile) {
                if (!FanHelper.IsFan(aPanelForUser.Id, aLoggedInUser)) {
                    NavigationModel.FanMetaData = new NavigationItemModel() {
                        AltText = "Fan this user",
                        Url = "/Fan/Add/" + aPanelForUser.Id,
                        DisplayText = "Fan"
                    };
                } else {
                    NavigationModel.FanMetaData = new NavigationItemModel() {
                        AltText = "Unfan this user",
                        Url = "/Fan/Remove/" + aPanelForUser.Id,
                        DisplayText = "De-fan"
                    };
                }
            } else {
                NavigationModel.FanMetaData = new NavigationItemModel() {
                    Display = false
                };
            }
        }

        private static bool IsAProfilePage(SiteSection aSection) {
            return aSection == SiteSection.Profile;
        }

        private void BuildMenu(User aPanelForUser, User aLoggedInUser) {
            SiteSection myHomeSection = theIsMyProfile ? SiteSection.MyProfile : SiteSection.Home;
            UserNavigationMenuModel myHomeMenuItem = new UserNavigationMenuModel(myHomeSection, UserPanelNav.HOME_BUTTON) {
                Url = "/Profile/Show"
            };

            UserNavigationMenuModel myIssueActivityMenuItem = new UserNavigationMenuModel(SiteSection.IssueActivity, UserPanelNav.ISSUE_ACTIVITY_BUTTON) {
                Url = "/Profile/IssueActivity"
            };

            UserNavigationMenuModel myPhotoMenuItem = new UserNavigationMenuModel(SiteSection.Photos, UserPanelNav.PHOTO_BUTTON) {
                Url = "/PhotoAlbum/List"
            };

            UserNavigationMenuModel myEventMenuItem = new UserNavigationMenuModel(SiteSection.Calendar, UserPanelNav.CALENDAR_BUTTON) {
                Url = "/Calendar/List"
            };


            if (theIsMyProfile) {
                myHomeMenuItem.AltText = "my homepage";
                myIssueActivityMenuItem.AltText = "issues I'm participating in";
                myPhotoMenuItem.AltText = "my photos";
                myEventMenuItem.AltText = "my events";
            } else {
                myHomeMenuItem.AltText = NameHelper.FullName(aPanelForUser) + " profile page";
                myIssueActivityMenuItem.AltText = "issues that " + aPanelForUser.FirstName + " participated in";
                myPhotoMenuItem.AltText = aPanelForUser.FirstName + "'s photos";
                myEventMenuItem.AltText = aPanelForUser.FirstName + "'s events";                
            }

            List<UserNavigationMenuModel> myMenu = new List<UserNavigationMenuModel>();
            myMenu.Add(myHomeMenuItem);
            myMenu.Add(myIssueActivityMenuItem);
            myMenu.Add(myPhotoMenuItem);
            myMenu.Add(myEventMenuItem);

            NavigationModel.UserMenuMetaData = myMenu;
        }
    }
}