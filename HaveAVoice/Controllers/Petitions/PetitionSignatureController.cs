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
    public class PetitionSignatureController : HAVBaseController {
        private const string PETITION_SIGNED = "The petition has been signed successfully!";

        private const string PETITION_SIGNED_ERROR = "An error occurred while signing the petition please try again.";


        IValidationDictionary theValidationDictionary;
        IPetitionService thePetitionService;

        public PetitionSignatureController() {
            theValidationDictionary = new ModelStateWrapper(this.ModelState);
            thePetitionService = new PetitionService(theValidationDictionary);
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult Create(int petitionId) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            return View("Create", new CreatePetitionSignatureModel() {
                PetitionId = petitionId
            });
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        public ActionResult Create(CreatePetitionSignatureModel aCreatePetitionSignatureModel) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            UserInformationModel<User> myUserInformation = GetUserInformatonModel();

            try {
                bool myResult = thePetitionService.SignPetition(myUserInformation, aCreatePetitionSignatureModel);

                if (myResult) {
                    TempData["Message"] += MessageHelper.SuccessMessage(PETITION_SIGNED);
                    return RedirectToAction("Details", "Petition", new { id = aCreatePetitionSignatureModel.PetitionId });
                }
            } catch (Exception myException) {
                LogError(myException, PETITION_SIGNED_ERROR);
                TempData["Message"] = MessageHelper.ErrorMessage(PETITION_SIGNED_ERROR);
                theValidationDictionary.ForceModleStateExport();
            }

            return RedirectToAction("Details", "Petition", new { id = aCreatePetitionSignatureModel.PetitionId });
        }
   }
}
