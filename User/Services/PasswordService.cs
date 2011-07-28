using System;
using Social.Email;
using Social.Email.Exceptions;
using Social.Generic.Helpers;
using Social.Generic.Models;
using Social.User.Repositories;
using Social.Validation;

namespace Social.User.Services {
    public class PasswordService<T> : IPasswordService<T> {
        private const string FORGOT_PASSWORD_TITLE = "have a voice | forgot password";
        private const string FORGOT_PASSWORD_BODY = "Hello! <br/ ><br/ > To continue with the forgot password process and change your password please click the following link: <br/ >";

        public const double FORGOT_PASSWORD_MAX_DAYS = 15;

        private IValidationDictionary theValidationDictionary;
        private IEmail theEmailService;
        private IUserRetrievalService<T> theUserRetrievalService;
        private IPasswordRepository<T> thePasswordRepository;

        public PasswordService(IValidationDictionary aValidationDictionary, IEmail aEmailService, IUserRetrievalService<T> aUserRetrievalService, 
                               IPasswordRepository<T> aPasswordRepo) {
            theValidationDictionary = aValidationDictionary;
            theEmailService = aEmailService;
            theUserRetrievalService = aUserRetrievalService;
            thePasswordRepository = aPasswordRepo;
        }

        public bool ForgotPasswordRequest(string aBaseUrl, string anEmail) {
            T myUser = theUserRetrievalService.GetUser(anEmail);

            if (myUser == null) {
                AddUnrecognizedEmailToValidationDictionary(anEmail);
                return false;
            }

            string myForgotPasswordHash =
                System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(anEmail + DateTime.UtcNow.ToString(), "SHA1");
            thePasswordRepository.UpdateUserForgotPasswordHash(anEmail, myForgotPasswordHash);

            SendForgotPasswordCode(aBaseUrl, anEmail, myForgotPasswordHash);

            return true;
        }

        public bool ChangePassword(string anEmail, string aForgotPasswordHash, string aPassword, string aRetypedPassword) {
            if (!PasswordHashExists(aForgotPasswordHash)) {
                return false;
            }

            AbstractUserModel<T> myUser = thePasswordRepository.GetUserByEmailAndForgotPasswordHash(anEmail, aForgotPasswordHash);

            if (!ValidPasswordHashAndEmail(myUser, anEmail, aForgotPasswordHash) | !ValidatePassword(aPassword, aRetypedPassword)) {
                return false;
            }

            string myNewPassword = HashHelper.DoHash(aPassword);

            thePasswordRepository.ChangePassword(myUser.Id, myNewPassword);

            return true;
        }

        private bool PasswordHashExists(string aHash) {
            if (aHash.Trim().Length == 0) {
                theValidationDictionary.AddError("ForgotPasswordHash", aHash, "Error reading the forgot password hash. Please click the link in the email again.");
            }

            return theValidationDictionary.isValid;
        }

        private bool ValidPasswordHashAndEmail(AbstractUserModel<T> aUser, string anEmail, string aForgotPasswordHash) {

            if (aUser == null) {
                theValidationDictionary.AddError("Email", anEmail, "The email does not match the link you clicked. Please re-enter your email or go through the forgot password process again.");
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

        private void SendForgotPasswordCode(string aBaseUrl, string anEmail, string aPasswordHash) {
            string myUrl = aBaseUrl + "/Password/Process/" + aPasswordHash;
            string myForgotPasswordLink = "<a href=\"" + myUrl + "\">" + myUrl + "</a>";

            try {
                theEmailService.SendEmail(anEmail, FORGOT_PASSWORD_TITLE, FORGOT_PASSWORD_BODY + myForgotPasswordLink);
            } catch (Exception e) {
                throw new EmailException("Couldn't send email.", e);
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