using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HaveAVoice.Controllers.Helpers;
using HaveAVoice.Models;
using HaveAVoice.Models.View;
using HaveAVoice.Services.Petitions;
using Social.Admin.Exceptions;
using Social.Generic.ActionFilters;
using Social.Generic.Models;
using Social.Validation;

namespace HaveAVoice.Controllers.Petitions {
    public class PetitionController : HAVBaseController {
        private const string PETITION_CREATED = "The petition has been created successfully!";
        private const string PETITION_DEACTIVATED = "The petition has been deactivated.";
        private const string NO_PETITIONS = "There are currently no petitions active. Create one!";
        private const string PETITION_FINISHED = "The petition has been marked as finished and is closed.";

        private const string PETITION_CREATE_ERROR = "An error occurred while creating the petition.";
        private const string PETITION_DEACTIVATED_ERROR = "An error occurred while deactivating the petition.";
        private const string PETITION_DEACTIVATED_PERMISSION_ERROR = "You must be the creator of the petition to deactivate it.";
        private const string PETITION_GET_ERROR = "An error occurred while retrieving the petition.";
        private const string PETITION_LIST_ERROR = "An error occurred while retrieving the petitions.";
        private const string PETITION_FINISHED_ERROR = "An error occurred while trying to close your petition. Please try again.";

        IValidationDictionary theValidationDictionary;
        IPetitionService thePetitionService;

        public PetitionController() {
            theValidationDictionary = new ModelStateWrapper(this.ModelState);
            thePetitionService = new PetitionService(theValidationDictionary);
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult Create() {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            return View("Create", new CreatePetitionModel());
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        public ActionResult Create(CreatePetitionModel aCreatePetitionModel) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            UserInformationModel<User> myUserInformation = GetUserInformatonModel();

            try {
                Petition myPetition = thePetitionService.CreatePetition(myUserInformation, aCreatePetitionModel);

                if (myPetition != null) {
                    TempData["Message"] += MessageHelper.SuccessMessage(PETITION_CREATED);
                    return RedirectToAction("Details", new { id = myPetition.Id });
                }
            } catch (Exception myException) {
                LogError(myException, PETITION_CREATE_ERROR);
                TempData["Message"] = MessageHelper.ErrorMessage(PETITION_CREATE_ERROR);
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
                UserInformationModel<User> myUserInformation = GetUserInformatonModel();

                bool myResult = thePetitionService.SetPetitionAsInactive(myUserInformation, id);

                if (myResult) {
                    TempData["Message"] += MessageHelper.SuccessMessage(PETITION_DEACTIVATED);
                    return RedirectToAction("List", "Group");
                }
            } catch(PermissionDenied) {
                TempData["Message"] += MessageHelper.ErrorMessage(PETITION_DEACTIVATED_PERMISSION_ERROR);
            } catch (Exception myException) {
                LogError(myException, PETITION_DEACTIVATED_ERROR);
                TempData["Message"] += MessageHelper.ErrorMessage(PETITION_DEACTIVATED_ERROR);
                
            }

            return RedirectToAction("Details", new { id = id });
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult Details(int id) {
            try {
                UserInformationModel<User> myUser = GetUserInformatonModel();

                DisplayPetitionModel myPetition = thePetitionService.GetPetition(myUser, id);

                return View("Details", myPetition);
            } catch (Exception myException) {
                LogError(myException, PETITION_GET_ERROR);
                return RedirectToAction("List");
            }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult List() {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            IEnumerable<Petition> myPetitions = new List<Petition>();

            try {
                UserInformationModel<User> myUser = GetUserInformatonModel();

                myPetitions = thePetitionService.GetPetitions(myUser);

                if (myPetitions.Count<Petition>() == 0) {
                    ViewData["Message"] = MessageHelper.NormalMessage(NO_PETITIONS);
                }

                return View("List", myPetitions);
            } catch (Exception myException) {
                LogError(myException, PETITION_LIST_ERROR);
                return SendToErrorPage(PETITION_LIST_ERROR);
            }
        }

        [AcceptVerbs(HttpVerbs.Get), ExportModelStateToTempData]
        public ActionResult MarkPetitionAsFinished(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }


            try {
                UserInformationModel<User> myUser = GetUserInformatonModel();

                thePetitionService.SetPetitionAsInactive(myUser, id);
                TempData["Message"] = MessageHelper.SuccessMessage(PETITION_FINISHED);
            } catch (Exception myException) {
                LogError(myException, PETITION_LIST_ERROR);
                TempData["Message"] = MessageHelper.ErrorMessage(PETITION_FINISHED_ERROR);
            }

            return RedirectToAction("Details", new { id = id });
        }
    }
}
