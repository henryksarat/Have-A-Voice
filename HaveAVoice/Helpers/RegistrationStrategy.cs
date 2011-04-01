using HaveAVoice.Models;
using Social.Generic.Models;
using Social.User.Helpers;
using Social.Validation;
using Social.Authentication.Services;
using HaveAVoice.Services.UserFeatures;

namespace HaveAVoice.Helpers {
    public class RegistrationStrategy : IRegistrationStrategy<User> {
        public bool ExtraFieldsAreValid(AbstractUserModel<User> aUser, IValidationDictionary aValidationDictionary) {
            if (aUser.State.Trim().Length != 2) {
                aValidationDictionary.AddError("State", aUser.State.Trim(), "State is required.");
            }
            if (aUser.City.Trim().Length == 0) {
                aValidationDictionary.AddError("City", aUser.City.Trim(), "City is required.");
            }
            return aValidationDictionary.isValid;
        }

        public void PostRegistration(AbstractUserModel<User> aUser) {
            //No post registration for have a voice
        }

        public void BackUpPlanForEmailNotSending(AbstractUserModel<User> aUser) {
            IHAVAuthenticationService myAuthService = new HAVAuthenticationService();
            myAuthService.ActivateNewUser(aUser.ActivationCode);
        }
    }
}