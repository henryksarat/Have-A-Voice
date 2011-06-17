using System;
using System.Web;
using System.Web.Mvc;
using Social.Authentication;
using Social.Generic.ActionFilters;
using Social.Generic.Models;
using Social.Users.Services;
using Social.Validation;
using UniversityOfMe.Helpers;
using UniversityOfMe.Models;
using UniversityOfMe.Repositories;
using UniversityOfMe.Services;
using UniversityOfMe.Services.Clubs;
using UniversityOfMe.Services.Professors;
using UniversityOfMe.UserInformation;
using UniversityOfMe.Models.View;
using System.Collections.Generic;

namespace UniversityOfMe.Controllers.Clubs {
    public class ClubMemberController : UOFMeBaseController {
        private const string PENDING = "You are already pending to join the club.";
        private const string APPROVED = "You are already apart of the club.";
        
        private const string USER_REMVOED = "User removed successfully!";
        private const string REQUEST_SUCCESS = "Your request to join the club has been submitted! Now an admin of the club must approve you before you can join.";
        private const string CANCEL_REQUEST = "Your request to join the club has been cancelled.";
        private const string CLUB_MEMBER_APPROVED = "The club member has been approved to join the club!";
        private const string CLUB_MEMBER_DENIED = "The club member has been denied to join the club!";

        private const string CANCEL_REQUEST_ERROR = "An error occurred while canceling your request to join the club. Please try again.";
        private const string REQUEST_ERROR = "An error occurred while submitted your request. Please try again.";
        private const string REMOVE_ERROR = "Error while removing the member from the club. Please try again.";
        private const string CLUB_MEMBER_ERROR = "Unable to get the club member information. Please try again.";
        private const string CLUB_MEMBER_VERDICT_ERROR = "An error occurred while approving or denying the club member.";
        private const string CLUB_MEMBER_LIST = "An error occurred while getting the list of members for the club.";

        IValidationDictionary theValidationDictionary;
        IClubService theClubService;
        IUniversityService theUniversityService;

        public ClubMemberController() {
            UserInformationFactory.SetInstance(UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository()), new GetUserStrategy()));
            theValidationDictionary = new ModelStateWrapper(this.ModelState);
            theClubService = new ClubService(theValidationDictionary);
            theUniversityService = new UniversityService(theValidationDictionary);
        }

        [AcceptVerbs(HttpVerbs.Get), ExportModelStateToTempData]
        public ActionResult Remove(int userId, int clubId) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            try {
                bool myResult = theClubService.RemoveClubMember(GetUserInformatonModel(), userId, clubId);

                if (myResult) {
                    TempData["Message"] = MessageHelper.SuccessMessage(USER_REMVOED);
                }
            } catch (Exception myException) {
                LogError(myException, REMOVE_ERROR);
                TempData["Message"] = MessageHelper.ErrorMessage(REMOVE_ERROR);
            }

            return RedirectToAction("Details", "Club", new { id = clubId });
        }

        [AcceptVerbs(HttpVerbs.Get), ExportModelStateToTempData]
        public ActionResult RequestToJoin(int clubId, string universityId) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            try {
                UserInformationModel<User> myUserInformation = GetUserInformatonModel();

                if (!UniversityHelper.IsFromUniversity(myUserInformation.Details, universityId)) {
                    return SendToResultPage(UOMConstants.NOT_APART_OF_UNIVERSITY);
                }

                if (theClubService.IsPendingApproval(myUserInformation.Details.Id, clubId)) {
                    TempData["Message"] = MessageHelper.NormalMessage(PENDING);
                } else if (theClubService.IsApartOfClub(myUserInformation.Details.Id, clubId)) {
                    TempData["Message"] = MessageHelper.NormalMessage(APPROVED);
                } else {
                    theClubService.RequestToJoinClub(myUserInformation, clubId);
                    TempData["Message"] = MessageHelper.SuccessMessage(REQUEST_SUCCESS);
                }
            } catch (Exception myException) {
                LogError(myException, REMOVE_ERROR);
                TempData["Message"] = MessageHelper.ErrorMessage(REQUEST_ERROR);
            }

            return RedirectToAction("Details", "Club", new { id = clubId });
        }

        [AcceptVerbs(HttpVerbs.Get), ExportModelStateToTempData]
        public ActionResult Cancel(int clubId, string universityId) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            try {
                UserInformationModel<User> myUserInformation = GetUserInformatonModel();

                theClubService.CancelRequestToJoin(myUserInformation, clubId);
                TempData["Message"] = MessageHelper.SuccessMessage(CANCEL_REQUEST);
            } catch (Exception myException) {
                LogError(myException, REMOVE_ERROR);
                TempData["Message"] = MessageHelper.ErrorMessage(REMOVE_ERROR);
            }

            return RedirectToAction("Details", "Club", new { id = clubId });
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult Details(int clubId, int clubMemberId) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            try {
                UserInformationModel<User> myUserInformation = GetUserInformatonModel();
                ClubMember myClubMember = theClubService.GetClubMember(myUserInformation, clubMemberId);
                if (myClubMember != null) {
                    LoggedInWrapperModel<ClubMember> myLoggedIn = new LoggedInWrapperModel<ClubMember>(myUserInformation.Details);
                    myLoggedIn.Set(myClubMember);
                    return View("Details", myLoggedIn);
                }
                TempData["Message"] = MessageHelper.WarningMessage(CLUB_MEMBER_ERROR);
            } catch (Exception myException) {
                LogError(myException, CLUB_MEMBER_ERROR);
                TempData["Message"] = MessageHelper.ErrorMessage(CLUB_MEMBER_ERROR);
            }

            return RedirectToAction("Details", "Club", new { id = clubId });
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult List(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            try {
                UserInformationModel<User> myUserInformation = GetUserInformatonModel();
                IEnumerable<ClubMember> myClubMembers = theClubService.GetActiveClubMembers(id);
                LoggedInListModel<ClubMember> myLoggedInModel = new LoggedInListModel<ClubMember>(myUserInformation.Details);
                myLoggedInModel.Set(myClubMembers);
                return View("List", myLoggedInModel);
            } catch (Exception myException) {
                LogError(myException, CLUB_MEMBER_LIST);
                TempData["Message"] = MessageHelper.ErrorMessage(CLUB_MEMBER_LIST);
                return RedirectToAction("List", "Club", new { id = id });
            }
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        public ActionResult Verdict(int clubMemberId, int clubId, bool approved, string title, bool administrator) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            try {
                UserInformationModel<User> myUserInformation = GetUserInformatonModel();
                bool myResult = false;
                if (approved) {
                    myResult = theClubService.ApproveClubMember(myUserInformation, clubMemberId, title, administrator);
                    if(myResult) {
                        TempData["Message"] = MessageHelper.NormalMessage(CLUB_MEMBER_APPROVED);
                    }
                } else {
                    theClubService.DenyClubMember(myUserInformation, clubMemberId);
                    myResult = true;
                    TempData["Message"] = MessageHelper.NormalMessage(CLUB_MEMBER_DENIED);
                }

                if (myResult) {
                    return RedirectToAction("Details", "Club", new { id = clubId });
                }
            } catch (Exception myException) {
                LogError(myException, CLUB_MEMBER_ERROR);
                TempData["Message"] = MessageHelper.ErrorMessage(CLUB_MEMBER_VERDICT_ERROR);
            }
            
            //Force for radio button value export
            theValidationDictionary.ForceModleStateExport();
            
            return RedirectToAction("Details", new { clubId = clubId, clubMemberId = clubMemberId });
        }
    }
}
