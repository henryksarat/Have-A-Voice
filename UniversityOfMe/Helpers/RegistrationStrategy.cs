using System.Collections.Generic;
using Social.User.Helpers;
using Social.User.Models;
using Social.Validation;
using UniversityOfMe.Helpers;
using UniversityOfMe.Models;
using UniversityOfMe.Models.Social;
using UniversityOfMe.Services;

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

    public AbstractUserModel<User> AddFieldsToUserObject(AbstractUserModel<User> aUser) {
        User myUser = aUser.FromModel();
        myUser.UniversityId = EmailHelper.ExtractEmailExtension(myUser.Email);
        return SocialUserModel.Create(myUser);
    }
}