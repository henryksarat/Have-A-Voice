﻿using HaveAVoice.Models;
using Social.Generic.Models;
using Social.User.Helpers;
using Social.Validation;
using Social.Authentication.Services;
using HaveAVoice.Services.UserFeatures;
using HaveAVoice.Models.SocialWrappers;

namespace HaveAVoice.Helpers {
    public class RegistrationStrategy : IRegistrationStrategy<User> {
        public void PostRegistration(AbstractUserModel<User> aUser) {
            //No post registration for have a voice
        }

        public void BackUpPlanForEmailNotSending(AbstractUserModel<User> aUser) {
            IHAVAuthenticationService myAuthService = new HAVAuthenticationService();
            myAuthService.ActivateNewUser(aUser.ActivationCode);
        }

        public bool ExtraFieldsAreValid(AbstractUserModel<User> aUser, IValidationDictionary aValidationDictionary) {
            return ExtraFieldsAreValidForHaveAVoice((SocialUserModel)aUser, aValidationDictionary);
        }

        private bool ExtraFieldsAreValidForHaveAVoice(SocialUserModel aUser, IValidationDictionary aValidationDictionary) {
            if (aUser.State.Trim().Length != 2) {
                aValidationDictionary.AddError("State", aUser.State.Trim(), "State is required.");
            }
            if (aUser.City.Trim().Length == 0) {
                aValidationDictionary.AddError("City", aUser.City.Trim(), "City is required.");
            }
            if (!(aUser.Zip.ToString().Trim().Length == 3 
                && aUser.StringZip.Trim().Length == 5) && !(aUser.Zip.ToString().Trim().Length == 4 && aUser.StringZip.Trim().Length == 5) 
                && aUser.Zip.ToString().Trim().Length != 5) {
                aValidationDictionary.AddError("Zip", aUser.Zip.ToString(), "The zip code must be 5 digits.");
            }
            return aValidationDictionary.isValid;
        }
    }
}