using System;
using System.Web;
using System.Web.Mvc;
using Social.Authentication;
using Social.Generic.ActionFilters;
using Social.Generic.Models;
using Social.User.Services;
using Social.Users.Services;
using UniversityOfMe.Helpers;
using UniversityOfMe.Models;
using UniversityOfMe.Models.View;
using UniversityOfMe.Repositories;
using UniversityOfMe.Repositories.UserRepos;
using UniversityOfMe.Services.UserFeatures;
using UniversityOfMe.UserInformation;
using Social.Generic.Constants;
using UniversityOfMe.Services.Users;
using UniversityOfMe.Services.Status;
using Social.Validation;
using Social.Admin.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace UniversityOfMe.Controllers.Profile {
    public class UserStatusController : UOFMeBaseController {
        private const string CREATE_SUCCESS = "Your status has been set!";
        private const string DELETE_SUCCESS = "The status has been deleted!";

        private const string CREATE_ERROR = "Error setting your status. Please try again.";
        private const string DELETE_ERROR = "Error deleting the status.";
        private const string LIST_ERROR = "Error retrieving the most recent statuses for your university.";


        private IUserStatusService theUserStatusService;
        IValidationDictionary theValidation;

        public UserStatusController() {
            UserInformationFactory.SetInstance(UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository()), new GetUserStrategy()));
            theValidation = new ModelStateWrapper(this.ModelState);
            theUserStatusService = new UserStatusService(theValidation);
            
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        public ActionResult Create(string userStatus, bool everyone) {
            try {
                UserInformationModel<User> myUserInfo = GetUserInformatonModel();

                bool myResult = theUserStatusService.CreateUserStatus(myUserInfo, userStatus, everyone);

                if (myResult) {
                    TempData["Message"] += SuccessMessage(CREATE_SUCCESS);
                }
            } catch (Exception e) {
                LogError(e, CREATE_ERROR);
                TempData["Message"] += ErrorMessage(CREATE_ERROR);
                theValidation.ForceModleStateExport();
            }

            return RedirectToProfile();
        }

        [AcceptVerbs(HttpVerbs.Get), ExportModelStateToTempData]
        public ActionResult Delete(int id, string sourceController, string sourceAction, int sourceId) {
            try {
                UserInformationModel<User> myUserInfo = GetUserInformatonModel();

                theUserStatusService.DeleteUserStatus(myUserInfo, id);

                TempData["Message"] += SuccessMessage(DELETE_SUCCESS);
            } catch (PermissionDenied anException) {
                TempData["Message"] += WarningMessage(anException.Message);
            } catch (Exception e) {
                LogError(e, DELETE_ERROR);
                TempData["Message"] += ErrorMessage(DELETE_ERROR);
            }

            return RedirectToAction(sourceAction, sourceController, new { id = sourceId });
        }

        [AcceptVerbs(HttpVerbs.Get), ExportModelStateToTempData]
        public ActionResult List() {
            try {
                UserInformationModel<User> myUserInfo = GetUserInformatonModel();
                string myUniversity = UniversityHelper.GetMainUniversity(myUserInfo.Details).Id;
                LoggedInListModel<UserStatus> myLoggedInModel = new LoggedInListModel<UserStatus>(myUserInfo.Details);
                IEnumerable<UserStatus> myUserStatuses = theUserStatusService.GetLatestUserStatusesWithinUniversity(myUserInfo, myUniversity, 30);
                myLoggedInModel.Set(myUserStatuses);

                return View("List", myLoggedInModel);
            } catch (Exception e) {
                LogError(e, LIST_ERROR);
                TempData["Message"] += ErrorMessage(LIST_ERROR);
            }

            return RedirectToHomePage();
        }
    }
}
