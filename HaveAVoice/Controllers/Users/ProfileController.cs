using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HaveAVoice.Controllers.Admin;
using HaveAVoice.Services.UserFeatures;
using HaveAVoice.Validation;
using HaveAVoice.Repositories;
using HaveAVoice.Services;
using HaveAVoice.Models;
using System.Text;
using HaveAVoice.Models.View;
using HaveAVoice.Controllers.Helpers;
using HaveAVoice.Controllers.ActionFilters;
using HaveAVoice.Exceptions;
using HaveAVoice.Helpers;
using HaveAVoice.Helpers.Enums;

namespace HaveAVoice.Controllers.Users {
    public class ProfileController : HAVBaseController {
        private const string EMPTY_FRIEND_FEED = "You have nothing in your feed. Make some friends to start seeing activity here!";
        private const string EMPTY_POLITICIAN_FEED = "No politicians have posted anything interesting yet!";
        private const string EMPTY_PROFILE = "The user has no activity.";
        private const string MY_EMPTY_ISSUE_ACTIVITY = "You have not participated in any issues. Go out and start posting!";
        private const string EMPTY_ISSUE_ACTIVITY = "The user has yet to create any issues and reply to any exisiting issues.";
        private const string USER_PAGE_ERROR = "Unable to view the user page. Please try again.";
        private const string USER_ACTIVITY_ERROR = "Unable to get the user's issue activity. Please try again.";
        private const string PERSON_FILTER = "PersonFilter";
        private const string ISSUE_STANCE_FILTER = "IssueStanceFilter";
        private const string INVALID_SHORT_URL = "No user is assigned with that web address. Verify the spelling and try again.";

        private const string PROFILE_VIEW = "Show";

        private IHAVProfileService theService;

        public ProfileController()
            : base(new HAVBaseService(new HAVBaseRepository())) {
            theService = new HAVProfileService(new ModelStateWrapper(this.ModelState));
        }

        public ProfileController(IHAVProfileService aService, IHAVBaseService aBaseService)
            : base(aBaseService) {
            theService = aService;
        }

        [RequiredRouteValueAttribute.RequireRouteValues(new[] { "shortName" })]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Show(string shortName) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            User myViewingUser = GetUserInformaton();

            try {
                UserProfileModel myProfile = theService.Profile(shortName, myViewingUser);
                if (myProfile == null) {
                    return SendToErrorPage(INVALID_SHORT_URL);
                }

                LoggedInWrapperModel<UserProfileModel> myModel = new LoggedInWrapperModel<UserProfileModel>(myProfile.User, SiteSection.Profile);
                myModel.Model = myProfile;

                if (myModel.Model.IsEmpty()) {
                    ViewData["Message"] = MessageHelper.NormalMessage(EMPTY_PROFILE);
                }

                return View(PROFILE_VIEW, myModel);
            } catch (Exception e) {
                LogError(e, USER_PAGE_ERROR);
                return SendToErrorPage(USER_PAGE_ERROR);
            }
        }

        [RequiredRouteValueAttribute.RequireRouteValues(new[] { "id" })]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Show(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            User myViewingUser = GetUserInformaton();
            
            try {
                UserProfileModel myProfile = theService.Profile(id, myViewingUser);
                LoggedInWrapperModel<UserProfileModel> myModel = new LoggedInWrapperModel<UserProfileModel>(myProfile.User, SiteSection.Profile);
                myModel.Model = myProfile;

                if (myModel.Model.IsEmpty()) {
                    ViewData["Message"] = MessageHelper.NormalMessage(EMPTY_PROFILE);
                }

                return View(PROFILE_VIEW, myModel);
            } catch (Exception e) {
                LogError(e, USER_PAGE_ERROR);
                return SendToErrorPage(USER_PAGE_ERROR);
            }
        }

        [RequiredRouteValueAttribute.RequireRouteValues(new string[] { })]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Show() {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            UserInformationModel myUser = GetUserInformatonModel();
            LoggedInWrapperModel<UserProfileModel> myModel = new LoggedInWrapperModel<UserProfileModel>(myUser.Details, SiteSection.MyProfile);

            try {
                if (HAVPermissionHelper.AllowedToPerformAction(myUser, HAVPermission.Authority_Feed)) {
                    myModel.Model = theService.AuthorityProfile(myUser.Details);
                } else {
                    myModel.Model = theService.MyProfile(myUser.Details);
                }

                SaveFeedInformationToTempDataForFiltering(myModel.Model, PersonFilter.All);

                if (myModel.Model.IsEmpty()) {
                    ViewData["Message"] = MessageHelper.NormalMessage(EMPTY_FRIEND_FEED);
                }
            } catch (Exception e) {
                LogError(e, USER_PAGE_ERROR);
                ViewData["Message"] = USER_PAGE_ERROR;
            }

                return View("Show", myModel);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult FilterFeed(PersonFilter filterValue) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            User myUser = GetUserInformatonModel().Details;

            UserProfileModel myOriginalUserProfileModel = GetOriginalUserProfileModel();

            LoggedInWrapperModel<UserProfileModel> myModel = new LoggedInWrapperModel<UserProfileModel>(myUser, SiteSection.MyProfile);
            myModel.Model = new UserProfileModel(myOriginalUserProfileModel.User) {
                LocalIssue = myOriginalUserProfileModel.LocalIssue,
                IssueFeed = (from i in myOriginalUserProfileModel.OriginalIssueFeed
                             where i.PersonFilter.Equals(filterValue)
                             select i).ToList<IssueFeedModel>(),
                IssueReplyFeed = (from ir in myOriginalUserProfileModel.OriginalIssueReplyFeed
                                  where ir.PersonFilter.Equals(filterValue)
                                  select ir)
            };

            SaveFeedInformationToTempDataForFiltering(GetOriginalUserProfileModel(), filterValue);

            if (myModel.Model.IsEmpty()) {
                if (filterValue == PersonFilter.People) {
                    ViewData["Message"] = MessageHelper.NormalMessage(EMPTY_FRIEND_FEED);
                } else if (filterValue == PersonFilter.Politicians) {
                    ViewData["Message"] = MessageHelper.NormalMessage(EMPTY_POLITICIAN_FEED);
                }
            }

            return View(PROFILE_VIEW, myModel);
        }

        [RequiredRouteValueAttribute.RequireRouteValues(new[] { "id" })]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult IssueActivity(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            User myUser = GetUserInformaton();
            
            try {
                UserProfileModel myProfile = theService.UserIssueActivity(id, myUser);
                LoggedInWrapperModel<UserProfileModel> myModel = new LoggedInWrapperModel<UserProfileModel>(myProfile.User, SiteSection.IssueActivity);
                myModel.Model = myProfile;

                if (myModel.Model.IsEmpty()) {
                    ViewData["Message"] = MessageHelper.NormalMessage(EMPTY_ISSUE_ACTIVITY);
                }

                return View("IssueActivity", myModel);
            } catch (CustomException e) {
                return SendToErrorPage(e.Message);
            } catch (Exception e) {
                LogError(e, USER_ACTIVITY_ERROR);
                return SendToErrorPage(USER_ACTIVITY_ERROR);
            }
        }

        [RequiredRouteValueAttribute.RequireRouteValues(new string[] { })]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult IssueActivity() {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            User myUser = GetUserInformaton();
            LoggedInWrapperModel<UserProfileModel> myModel = new LoggedInWrapperModel<UserProfileModel>(myUser, SiteSection.MyIssueActivity);

            try {
                myModel.Model = theService.UserIssueActivity(myUser.Id, myUser);

                if (myModel.Model.IsEmpty()) {
                    ViewData["Message"] = MessageHelper.NormalMessage(MY_EMPTY_ISSUE_ACTIVITY);
                }
            } catch (CustomException e) {
                return SendToErrorPage(e.Message);
            } catch (Exception e) {
                LogError(e, USER_ACTIVITY_ERROR);
                ViewData["Message"] = USER_ACTIVITY_ERROR;
            }

            return View("IssueActivity", myModel);
        }

        private void SaveFeedInformationToTempDataForFiltering(UserProfileModel aProfileModel, PersonFilter aFilter) {
            TempData[HAVConstants.ORIGINAL_MYPROFILE_FEED_TEMP_DATA] = aProfileModel;
            TempData[HAVConstants.FILTER_TEMP_DATA] = aFilter;
        }

        private UserProfileModel GetOriginalUserProfileModel() {
            return (UserProfileModel)TempData[HAVConstants.ORIGINAL_MYPROFILE_FEED_TEMP_DATA];
        }
    }
}
