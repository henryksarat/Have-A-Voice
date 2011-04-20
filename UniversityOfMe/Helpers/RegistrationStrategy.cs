using System.Web.Mvc;
using Social.Authentication.Services;
using Social.Generic.Models;
using Social.User.Helpers;
using Social.Validation;
using UniversityOfMe.Controllers.Helpers;
using UniversityOfMe.Helpers;
using UniversityOfMe.Models;
using UniversityOfMe.Services;

public class RegistrationStrategy : IRegistrationStrategy<User> {
    public bool ExtraFieldsAreValid(AbstractUserModel<User> aUser, IValidationDictionary aValidationDictionary) {
        return UniversityHelper.IsValidUniversityEmail(aUser.Email, aValidationDictionary);
    }

    public void PostRegistration(AbstractUserModel<User> aUser) {
        IValidationDictionary myValidation = new ModelStateWrapper(new ModelStateDictionary());
        IUniversityService theUniversityService = new UniversityService(myValidation);
        theUniversityService.AddUserToUniversity(aUser.CreateNewModel());
    }

    public void BackUpPlanForEmailNotSending(AbstractUserModel<User> aUser) {
        IAuthenticationService<User, Role, Permission, UserRole, PrivacySetting, RolePermission> myAuthService = InstanceHelper.CreateAuthencationService();
        myAuthService.ActivateNewUser(aUser.ActivationCode);
    }
}