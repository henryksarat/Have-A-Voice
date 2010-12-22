using System;
using HaveAVoice.Exceptions;
using HaveAVoice.Helpers;
using HaveAVoice.Models;
using HaveAVoice.Repositories;
using HaveAVoice.Repositories.UserFeatures;
using HaveAVoice.Services.Helpers;
using HaveAVoice.Validation;

namespace HaveAVoice.Services.UserFeatures {
    public class HAVPasswordService : HAVBaseService, IHAVPasswordService {
        public const double FORGOT_PASSWORD_MAX_DAYS = 15;

        private IValidationDictionary theValidationDictionary;
        private IHAVEmail theEmailService;
        private IHAVUserRetrievalService theUserRetrievalService;
        private IHAVPasswordRepository thePasswordRepository;
        private IHAVUserRepository theUserRepo;

        public HAVPasswordService(IValidationDictionary theValidationDictionary)
            : this(theValidationDictionary, new HAVEmail(), new HAVUserRetrievalService(), new HAVBaseRepository(), new EntityHAVPasswordRepository(), new EntityHAVUserRepository()) { }

        public HAVPasswordService(IValidationDictionary aValidationDictionary, IHAVEmail aEmailService, IHAVUserRetrievalService aUserRetrievalService, 
                                                IHAVBaseRepository baseRepository, IHAVPasswordRepository aPasswordRepo, IHAVUserRepository aUserRepo) : base(baseRepository) {
            theValidationDictionary = aValidationDictionary;
            theEmailService = aEmailService;
            theUserRetrievalService = aUserRetrievalService;
            thePasswordRepository = aPasswordRepo;
            theUserRepo = aUserRepo;
        }

        public bool ForgotPasswordRequest(string anEmail) {
            User myUser = theUserRetrievalService.GetUser(anEmail);

            if (myUser == null) {
                AddUnrecognizedEmailToValidationDictionary(anEmail);
                return false;
            }

            string myForgotPasswordHash =
                System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(anEmail + DateTime.UtcNow.ToString(), "SHA1");
            thePasswordRepository.UpdateUserForgotPasswordHash(anEmail, myForgotPasswordHash);

            SendForgotPasswordCode(anEmail, myForgotPasswordHash);

            return true;
        }

        public bool ChangePassword(string anEmail, string aForgotPasswordHash, string aPassword, string aRetypedPassword) {
            if (!PasswordHashExists(aForgotPasswordHash)) {
                return false;
            }

            User myUser = thePasswordRepository.GetUserByEmailAndForgotPasswordHash(anEmail, aForgotPasswordHash);

            if (!ValidPasswordHash(myUser, aForgotPasswordHash)) {
                return false;
            }

            if (!ValidatePassword(aPassword, aRetypedPassword)) {
                return false;
            }

            string myNewPassword = PasswordHelper.HashPassword(aPassword);

            thePasswordRepository.ChangePassword(myUser.Id, myNewPassword);

            return true;
        }

        private bool PasswordHashExists(string aHash) {
            if (aHash.Trim().Length == 0) {
                theValidationDictionary.AddError("ForgotPasswordHash", aHash, "Error reading the forgot password hash. Please click the link in the email again.");
            }

            return theValidationDictionary.isValid;
        }

        private bool ValidPasswordHash(User aUser, string aForgotPasswordHash) {

            if (aUser == null) {
                theValidationDictionary.AddError("ForgotPasswordHash", aForgotPasswordHash, "The email does not match the link you clicked. Please re-enter your email or go through the forgot password process again.");
                return false;
            }
            DateTime myDateTime = (DateTime)aUser.ForgotPasswordHashDateTimeStamp;
            TimeSpan myDifference = DateTime.UtcNow - myDateTime;
            if (myDifference.Days > FORGOT_PASSWORD_MAX_DAYS) {
                theValidationDictionary.AddError("ForgotPasswordHash", aForgotPasswordHash, "This link is expired. Please go through the forgot password process again.");
                return false;
            }

            return true;
        }

        private void AddUnrecognizedEmailToValidationDictionary(string anEmail) {
            theValidationDictionary.AddError("Email", anEmail, "That is not an email we recognize.");
        }

        private void SendForgotPasswordCode(string anEmail, string aPasswordHash) {
            try {
                theEmailService.SendEmail(anEmail, "have a voice | forgot password", "Click this link to change your password.... " + aPasswordHash);
            } catch (Exception e) {
                throw new EmailException("Couldn't send aEmail.", e);
            }
        }

        private bool ValidatePassword(string aPassword, string aRetypedPassword) {
            if (aPassword.Trim().Length == 0) {
                theValidationDictionary.AddError("Password", "", "Password is required.");
            }
            if (aRetypedPassword == null || aRetypedPassword.Trim().Length == 0) {
                theValidationDictionary.AddError("RetypedPassword", "", "Please type your password again.");
            } else if (!aPassword.Equals(aRetypedPassword)) {
                theValidationDictionary.AddError("RetypedPassword", "", "Passwords must match.");
            }

            return theValidationDictionary.isValid;
        }
    }
}