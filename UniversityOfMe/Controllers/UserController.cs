using System;
using System.Web;
using System.Web.Mvc;
using Social.Email;
using Social.Generic.ActionFilters;
using Social.Generic.Constants;
using Social.Services.UserFeatures;
using Social.User;
using Social.Validation;
using UniversityOfMe.Helpers;
using UniversityOfMe.Models;
using UniversityOfMe.Models.View;
using UniversityOfMe.Repositories;

namespace UniversityOfMe.Controllers {
    public class UserController : BaseSocialController {
        private IUserService<User, Role, UserRole> theRegistrationService;
        private IValidationDictionary theValidationDictionary;

        public UserController() {
            theValidationDictionary = new ModelStateWrapper(this.ModelState);
            theRegistrationService = new UserService<User, Role, UserRole>(theValidationDictionary, new EntityUserRepository(), new SocialEmail());
        }

        public UserController(IUserService<User, Role, UserRole> service, IUserService<User, Role, UserRole> aRegistratonService) {
            theRegistrationService = aRegistratonService;
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult Create() {
            return View("Create", new CreateUserModel() {
                Genders = new SelectList(Constants.GENDERS, Constants.SELECT)
            });
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        public ActionResult Create(CreateUserModel aUserToCreate, bool agreement) {
            try {
                bool myResult = theRegistrationService.CreateUser(aUserToCreate, true, agreement,
                                                        HttpContext.Request.UserHostAddress,
                                                        UOMConstants.BASE_URL, UOMConstants.ACTIVATION_SUBJECT, UOMConstants.ACTIVATION_BODY, 
                                                        new RegistrationStrategy());
                if (myResult) {
                    return SendToResultPage("Success!");
                }
            } catch (Exception anException) {
                return SendToErrorPage("Error");
            }
            
            return RedirectToAction("Create");
        }
    }
}
