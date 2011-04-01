using System.Collections.Generic;
using Social.Generic.Models;
using Social.User.Helpers;
using Social.Validation;
using UniversityOfMe.Helpers;
using UniversityOfMe.Models;
using UniversityOfMe.Models.Social;
using UniversityOfMe.Services;
using UniversityOfMe.Controllers.Helpers;
using Social.Authentication.Services;

public class RegistrationStrategy : IRegistrationStrategy<User> {
    public bool ExtraFieldsAreValid(AbstractUserModel<User> aUser, IValidationDictionary aValidationDictionary) {
        IUniversityService theUniversityService = new UniversityService();

        if (!theUniversityService.IsValidUniversityEmailAddress(aUser.Email)) {
            IEnumerable<string> myValidEmails = theUniversityService.ValidEmails();
            string myValidEmailsInString = string.Join(",", myValidEmails);

            aValidationDictionary.AddError("Email", aUser.Email, "I'm sorry that is not a valid University email. We currently only accept the following emails: " + myValidEmailsInString);
        }

        return aValidationDictionary.isValid;
    }

    public void PostRegistration(AbstractUserModel<User> aUser) {
        IUniversityService theUniversityService = new UniversityService();
        theUniversityService.AddUserToUniversity(aUser.CreateNewModel());
    }

    public void BackUpPlanForEmailNotSending(AbstractUserModel<User> aUser) {
        IAuthenticationService<User, Role, Permission, UserRole, PrivacySetting, RolePermission> myAuthService = InstanceHelper.CreateAuthencationService();
        myAuthService.ActivateNewUser(aUser.ActivationCode);
    }
}