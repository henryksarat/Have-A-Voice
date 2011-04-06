using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using BaseWebsite.Controllers;
using HaveAVoice.Helpers.UserInformation;
using HaveAVoice.Services.Clubs;
using HaveAVoice.Services.UserFeatures;
using Social.Authentication;
using Social.Authentication.Helpers;
using Social.Generic.ActionFilters;
using Social.Generic.Models;
using Social.Generic.Services;
using Social.Validation;
using UniversityOfMe.Controllers.Helpers;
using UniversityOfMe.Helpers;
using UniversityOfMe.Models;
using UniversityOfMe.Models.Social;
using UniversityOfMe.Models.View;
using UniversityOfMe.Repositories;
using UniversityOfMe.Services;
using UniversityOfMe.Services.Professors;
using Social.Generic.Constants;
using Social.Generic.Helpers;

namespace UniversityOfMe.Controllers.Clubs {
 
    public class ClubController : BaseController<User, Role, Permission, UserRole, PrivacySetting, RolePermission, WhoIsOnline> {
        public const string CLUB_CREATED = "Club created successfully!";
        public const string CLUB_LIST_ERROR = "Error getting club list. Please try again.";
        public const string CLUB_TYPE_ERROR = "Error getting club types. Please try again.";
        public const string GET_CLUB_ERROR = "An error has occurred while retrieving the club. Please try again.";

        IClubService theClubService;
        IUniversityService theUniversityService;

        public ClubController()
            : base(new BaseService<User>(new EntityBaseRepository()),
                   UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository())),
                   InstanceHelper.CreateAuthencationService(),
                   new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository())) {
            UserInformationFactory.SetInstance(UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository())));
            IValidationDictionary myModelState = new ModelStateWrapper(this.ModelState);
            theClubService = new ClubService(myModelState);
            theUniversityService = new UniversityService();
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult Create(string universityId) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            if (!UniversityHelper.IsFromUniversity(GetUserInformatonModel().Details, universityId)) {
                return SendToResultPage(UOMConstants.NOT_APART_OF_UNIVERSITY);
            }

            IDictionary<string, string> myClubTypes = DictionaryHelper.DictionaryWithSelect();

            try {
                myClubTypes = theClubService.CreateAllClubTypesDictionaryEntry();
            } catch (Exception myExpcetion) {
                LogError(myExpcetion, CLUB_TYPE_ERROR);
                ViewData["Message"] = CLUB_TYPE_ERROR;
            }

            return View("Create", new CreateClubModel() {
                ClubTypes = new SelectList(myClubTypes, "Value", "Key")
            });
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        public ActionResult Create(CreateClubModel club) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            if (!UniversityHelper.IsFromUniversity(GetUserInformatonModel().Details, club.UniversityId)) {
                return SendToResultPage(UOMConstants.NOT_APART_OF_UNIVERSITY);
            }

            UserInformationModel<User> myUserInformation = GetUserInformatonModel();


            try {
                bool myResult = theClubService.CreateClub(myUserInformation, club);

                if (myResult) {
                    TempData["Message"] = CLUB_CREATED;
                    return RedirectToAction("List");
                }
            } catch (Exception myException) {
                LogError(myException, ErrorKeys.ERROR_MESSAGE);
                ViewData["Message"] = ErrorKeys.ERROR_MESSAGE;
            }

            return RedirectToAction("Create");
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult Details(int id) {
            try {
                Club myClub = theClubService.GetClub(id);

                if (!UniversityHelper.IsFromUniversity(GetUserInformatonModel().Details, myClub.UniversityId)) {
                    return SendToResultPage(UOMConstants.NOT_APART_OF_UNIVERSITY);
                }

                return View("Details", myClub);
            } catch (Exception myException) {
                LogError(myException, GET_CLUB_ERROR);
                return SendToResultPage(GET_CLUB_ERROR);
            }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult List(string universityId) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            if (!UniversityHelper.IsFromUniversity(GetUserInformatonModel().Details, universityId)) {
                return SendToResultPage(UOMConstants.NOT_APART_OF_UNIVERSITY);
            }

            IEnumerable<Club> myClubs = new List<Club>();

            try {
                myClubs = theClubService.GetClubs(universityId);
            } catch (Exception myException) {
                LogError(myException, CLUB_LIST_ERROR);
                ViewData["Message"] = CLUB_LIST_ERROR;
            }

            return View("List", myClubs);
        }
        
        protected override AbstractUserModel<User> GetSocialUserInformation() {
            return SocialUserModel.Create(GetUserInformaton());
        }

        protected override AbstractUserModel<User> CreateSocialUserModel(User aUser) {
            return SocialUserModel.Create(aUser);
        }

        protected override IProfilePictureStrategy<User> ProfilePictureStrategy() {
            return new ProfilePictureStrategy();
        }

        protected override string UserEmail() {
            return GetUserInformaton().Email;
        }

        protected override string UserPassword() {
            return GetUserInformaton().Password;
        }

        protected override int UserId() {
            return GetUserInformaton().Id;
        }

        protected override string ErrorMessage(string aMessage) {
            return aMessage;
        }

        protected override string NormalMessage(string aMessage) {
            return aMessage;
        }

        protected override string SuccessMessage(string aMessage) {
            return aMessage;
        }
    }
}
