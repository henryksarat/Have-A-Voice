using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Social.Authentication;
using Social.Generic.ActionFilters;
using Social.Generic.Constants;
using Social.Generic.Helpers;
using Social.Generic.Models;
using Social.Users.Services;
using Social.Validation;
using Social.Admin.Exceptions;
using Social.Photo.Exceptions;
using Social.Generic.Exceptions;
using HaveAVoice.Services.Groups;
using HaveAVoice.Controllers.Helpers;
using HaveAVoice.Models;
using HaveAVoice.Models.View;
using System.Linq;
using HaveAVoice.Helpers.Search;

namespace HaveAVoice.Controllers.Groups {
    public class GroupController : HAVBaseController {
        private const string GROUP_CREATED = "Group created successfully!";
        private const string GROUP_EDITED = "Group edited successfully!";
        private const string GROUP_DEACTIVATED = "The group has been deactivated! You can always activate it again by going on the club page and activating it.";
        private const string GROUP_ACTIVATED = "The group has been activated again!";
        private const string NO_GROUPS = "There are no groups currently. Go ahead and create one!";
        private const string NOT_IN_GROUPS = "You are not in any groups yet. Do a search and join some!";

        private const string GROUP_LIST_ERROR = "Error getting group list. Please try again.";
        private const string GROUP_GET_FOR_EDIT_ERROR = "Error getting the group for an edit. Please try again.";
        private const string GET_GROUP_ERROR = "An error has occurred while retrieving the group. Please try again.";
        private const string GROUP_DEACTIVATED_ERROR = "An error has occurred while deactivating the group.";
        private const string GROUP_ACTIVATED_ERROR = "An error has occurred while activating the group again.";

        IValidationDictionary theValidationDictionary;
        IGroupService theGroupService;

        public GroupController() {
            theValidationDictionary = new ModelStateWrapper(this.ModelState);
            theGroupService = new GroupService(theValidationDictionary);
        }

        [AcceptVerbs(HttpVerbs.Get), ExportModelStateToTempData]
        public ActionResult Activate(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            try {
                bool myResult = theGroupService.ActivateGroup(GetUserInformatonModel(), id);

                if (myResult) {
                    TempData["Message"] += MessageHelper.SuccessMessage(GROUP_ACTIVATED);
                }
            } catch (Exception myException) {
                LogError(myException, GROUP_DEACTIVATED_ERROR);
                TempData["Message"] += MessageHelper.ErrorMessage(GROUP_ACTIVATED_ERROR);
            }

            return RedirectToAction("Details", new { id = id });
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult Create() {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            return View("Create", new EditGroupModel() {
                States = new SelectList(UnitedStates.STATES, Constants.SELECT)
            });
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        public ActionResult Create(EditGroupModel aGroupModel) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            UserInformationModel<User> myUserInformation = GetUserInformatonModel();

            try {
                Group myGroup = theGroupService.CreateGroup(myUserInformation, aGroupModel);

                if (myGroup != null) {
                    TempData["Message"] += MessageHelper.SuccessMessage(GROUP_CREATED);
                    return RedirectToAction("Details", new { id = myGroup.Id });
                }
            } catch (Exception myException) {
                LogError(myException, ErrorKeys.ERROR_MESSAGE);
                TempData["Message"] = MessageHelper.ErrorMessage(ErrorKeys.ERROR_MESSAGE);
                theValidationDictionary.ForceModleStateExport();
            }

            return RedirectToAction("Create");
        }

        [AcceptVerbs(HttpVerbs.Get), ExportModelStateToTempData]
        public ActionResult Deactivate(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            try {
                bool myResult = theGroupService.DeactivateGroup(GetUserInformatonModel(), id);

                if (myResult) {
                    TempData["Message"] += MessageHelper.SuccessMessage(GROUP_DEACTIVATED);
                }
            } catch (Exception myException) {
                LogError(myException, GROUP_DEACTIVATED_ERROR);
                TempData["Message"] += MessageHelper.ErrorMessage(GROUP_DEACTIVATED_ERROR);
            }

            return RedirectToAction("List", "Group");
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult Details(int id) {
            try {
                UserInformationModel<User> myUser = GetUserInformatonModel();

                Group myGroup = theGroupService.GetGroup(myUser, id);

                return View("Details", myGroup);
            } catch (Exception myException) {
                LogError(myException, GET_GROUP_ERROR);
                return SendToResultPage(GET_GROUP_ERROR);
            }
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult Edit(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            try {

                UserInformationModel<User> myUser = GetUserInformatonModel();

                EditGroupModel myGroup = theGroupService.GetGroupForEdit(myUser, id);

                return View("Edit", myGroup);
            } catch(PermissionDenied myException) {
                TempData["Message"] += MessageHelper.ErrorMessage(myException.Message);
            } catch (Exception myExpcetion) {
                LogError(myExpcetion, GROUP_GET_FOR_EDIT_ERROR);
                TempData["Message"] += MessageHelper.ErrorMessage(GROUP_GET_FOR_EDIT_ERROR);
            }

            return RedirectToAction("Details", new { id = id });
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        public ActionResult Edit(EditGroupModel anEditGroupModel) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            UserInformationModel<User> myUserInformation = GetUserInformatonModel();

            try {
                bool myResult = theGroupService.EditGroup(myUserInformation, anEditGroupModel);

                if (myResult) {
                    TempData["Message"] += MessageHelper.SuccessMessage(GROUP_EDITED);
                    return RedirectToAction("Details", new { id = anEditGroupModel.Id });
                }
            } catch (Exception myException) {
                LogError(myException, ErrorKeys.ERROR_MESSAGE);
                TempData["Message"] += MessageHelper.ErrorMessage(ErrorKeys.ERROR_MESSAGE);
                theValidationDictionary.ForceModleStateExport();
            }

            return RedirectToAction("Edit", new { id = anEditGroupModel.Id });
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult List() {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            IEnumerable<Group> myGroups = new List<Group>();

            try {
                UserInformationModel<User> myUser = GetUserInformatonModel();

                myGroups = theGroupService.GetGroups(myUser, string.Empty, SearchBy.All, OrderBy.Name);

                if (myGroups.Count<Group>() == 0) {
                    ViewData["Message"] = MessageHelper.NormalMessage(NO_GROUPS);
                }

                GroupSearchModel myGroupSearchModel = new GroupSearchModel() {
                    SearchResults = myGroups,
                    SearchByOptions = new SelectList(theGroupService.SearchByOptions(), "Value", "Key"),
                    OrderByOptions = new SelectList(theGroupService.OrderByOptions(), "Value", "Key")
                };

                return View("List", myGroupSearchModel);
            } catch (Exception myException) {
                LogError(myException, GROUP_LIST_ERROR);
                return SendToErrorPage(GROUP_LIST_ERROR);
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult List(string searchTerm, SearchBy searchBy, OrderBy orderBy) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            IEnumerable<Group> myGroups = new List<Group>();

            GroupSearchModel myGroupSearchModel = new GroupSearchModel() {
                SearchResults = myGroups,
                SearchByOptions = new SelectList(theGroupService.SearchByOptions(), "Value", "Key", searchBy.ToString()),
                OrderByOptions = new SelectList(theGroupService.OrderByOptions(), "Value", "Key", orderBy.ToString())
            };

            try {
                UserInformationModel<User> myUser = GetUserInformatonModel();

                myGroups = theGroupService.GetGroups(myUser, searchTerm, searchBy, orderBy);

                if (myGroups.Count<Group>() == 0) {
                    ViewData["Message"] = MessageHelper.NormalMessage(NO_GROUPS);
                }

                myGroupSearchModel.SearchResults = myGroups;
            } catch(CustomException anException) {
                ViewData["Message"] = MessageHelper.ErrorMessage(anException.Message);
            } catch (Exception myException) {
                LogError(myException, GROUP_LIST_ERROR);
                ViewData["Message"] = GROUP_LIST_ERROR;
            }

            return View("List", myGroupSearchModel);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult MyGroups() {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            IEnumerable<Group> myGroups = new List<Group>();

            try {
                UserInformationModel<User> myUser = GetUserInformatonModel();

                myGroups = theGroupService.GetMyGroups(myUser);

                if (myGroups.Count<Group>() == 0) {
                    ViewData["Message"] = MessageHelper.NormalMessage(NOT_IN_GROUPS);
                }
            } catch (Exception myException) {
                LogError(myException, GROUP_LIST_ERROR);
                ViewData["Message"] = GROUP_LIST_ERROR;
            }

            return View("MyGroups", myGroups);
        }
    }
}
