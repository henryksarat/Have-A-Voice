using System;
using Social.Email;
using Social.Email.Exceptions;
using Social.Generic.Constants;
using Social.Generic.Helpers;
using Social.Generic.Models;
using Social.Services.UserFeatures;
using Social.User.Helpers;
using Social.Validation;

namespace Social.User {
    public class UserService<T, U, V> : IUserService<T, U, V> {
        private const string INVALID_EMAIL = "That is not a valid email.";

        private IValidationDictionary theValidationDictionary;
        private IUserRepository<T, U, V> theUserRepo;
        private IEmail theEmailService;

        public UserService(IValidationDictionary aValidationDictionary, IUserRepository<T, U, V> aUserRepo, IEmail aEmailService) {
            theValidationDictionary = aValidationDictionary;
            theUserRepo = aUserRepo;
            theEmailService = aEmailService;
        }

        public bool CreateUser(AbstractUserModel<T> aUserToCreate, bool aCaptchaValid, bool aAgreement, 
                               string aIpAddress, string aBaseUrl, string anActivationSubject, string anActivationBody, 
                               IRegistrationStrategy<T> aRegistrationStrategy) {
            if (!ValidateNewUser(aUserToCreate, aBaseUrl) | !ValidateAgreement(aAgreement)) {
                return false;
            }

            if (!ValidCaptchaImage(aCaptchaValid)) {
                return false;
            }

            if (!aRegistrationStrategy.ExtraFieldsAreValid(aUserToCreate, theValidationDictionary)) {
                return false;
            }

            aUserToCreate = CompleteAddingFieldsToUser(aUserToCreate, aIpAddress);
            aUserToCreate = theUserRepo.CreateUser(aUserToCreate.CreateNewModel(), Constants.NOT_CONFIRMED_USER_ROLE);

            try {
                aRegistrationStrategy.PostRegistration(aUserToCreate);
            } catch (Exception myException) {
                theUserRepo.DeleteUser(aUserToCreate.CreateNewModel());
                throw myException;
            }

            EmailException myEmailException = null;
            try {
                SendActivationCode(aUserToCreate, aBaseUrl, anActivationSubject, anActivationBody);
            } catch (EmailException e) {
                myEmailException = e;
            }

            if (myEmailException != null) {
                try {
                    aRegistrationStrategy.BackUpPlanForEmailNotSending(aUserToCreate);
                } catch (Exception myException) {
                    theUserRepo.DeleteUser(aUserToCreate.Model);
                    throw myException;
                }
                throw new EmailException("Couldn't send aEmail.", myEmailException);
            }

            return true;
        }


        private AbstractUserModel<T> CompleteAddingFieldsToUser(AbstractUserModel<T> aUserToCreate, string aIpAddress) {
            aUserToCreate.Password = HashHelper.DoHash(aUserToCreate.Password);
            aUserToCreate.RegistrationDate = DateTime.UtcNow;
            aUserToCreate.RegistrationIp = aIpAddress;
            aUserToCreate.LastLogin = DateTime.UtcNow;

            string aItemToHash = NameHelper<T>.FullName(aUserToCreate) + aUserToCreate.Email + DateTime.Today;
            aUserToCreate.ActivationCode = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(aItemToHash, "SHA1");

            return aUserToCreate;
        }

        private bool ShouldUploadImage(bool aValidImage, string anImageFile) {
            return !String.IsNullOrEmpty(anImageFile) && aValidImage;
        }

        private void SendActivationCode(AbstractUserModel<T> aUser, string aBaseUrl, string anActivationSubject, string anActivationBody) {
            string myUrl = aBaseUrl + "/Authentication/ActivateAccount/" + aUser.ActivationCode;

            try {
                theEmailService.SendEmail(aUser.Email, anActivationSubject, anActivationBody + myUrl);
            } catch (Exception e) {
                throw new EmailException("Couldn't send aEmail.", e);
            }
        }

        #region Validation"

        private bool ValidateAgreement(bool aAgreement) {
            if (aAgreement == false) {
                theValidationDictionary.AddError("Agreement", "false", "You must agree to the terms.");
            }

            return theValidationDictionary.isValid;
        }

        private bool ValidCaptchaImage(bool captchaValid) {
            if (captchaValid == false) {
                theValidationDictionary.AddError("Captcha", "false", "Captcha image verification failure.");
            }

            return theValidationDictionary.isValid;
        }

        private bool ValidateNewUser(AbstractUserModel<T> aUser, string aBaseUrl) {
            if (aUser.Password.Trim().Length == 0) {
                theValidationDictionary.AddError("Password", aUser.Password.Trim(), "Password is required.");
            }
            if (!EmailValidation.IsValidEmail(aUser.Email)) {
                theValidationDictionary.AddError("Email", aUser.Email.Trim(), "E-mail is required.");
            } else if (theUserRepo.EmailRegistered(aUser.Email.Trim())) {
                theValidationDictionary.AddError("Email", aUser.Email.Trim(), "Someone already registered with that email. Please try another one.");
            }
            if (aUser.ShortUrl.Length == 0) {
                theValidationDictionary.AddError("ShortUrl", aUser.ShortUrl, "You must give yourself a " + aBaseUrl + " url.");
            } else if (theUserRepo.ShortUrlTaken(aUser.ShortUrl)) {
                theValidationDictionary.AddError("ShortUrl", aUser.ShortUrl, "That " + aBaseUrl + " url is already taken by someone else.");
            }

            return ValidateUser(aUser);
        }

        private bool ValidateUser(AbstractUserModel<T> aUser) {
            if (aUser.FirstName.Trim().Length == 0) {
                theValidationDictionary.AddError("FirstName", aUser.FirstName.Trim(), "First name is required.");
            }
            if (aUser.LastName.Trim().Length == 0) {
                theValidationDictionary.AddError("LastName", aUser.LastName.Trim(), "Last name is required.");
            }
            if (aUser.Gender.Equals(Constants.SELECT) || aUser.Gender.Trim().Length == 0) {
                theValidationDictionary.AddError("Gender", Constants.SELECT, "Gender is required.");
            }
            if (!EmailValidation.IsValidEmail(aUser.Email)) {
                theValidationDictionary.AddError("Email", aUser.Email, INVALID_EMAIL);
            }
            if (aUser.DateOfBirth.Year == 1) {
                theValidationDictionary.AddError("DateOfBirth", aUser.DateOfBirth.ToString("MM-dd-yyyy"), "Date of Birth required.");
            }
            if (aUser.DateOfBirth > DateTime.Today.AddYears(-18)) {
                theValidationDictionary.AddError("DateOfBirth", aUser.DateOfBirth.ToString("MM-dd-yyyy"), "You must be at least 18 years old.");
            }

            return theValidationDictionary.isValid;
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

        private bool ValidForgotPasswordHash(string aHash) {
            if (aHash.Trim().Length == 0) {
                theValidationDictionary.AddError("ForgotPasswordHash", aHash, "Error reading the forgot password hash. Please click the link in the email again.");
            }

            return theValidationDictionary.isValid;
        }

        private bool ValidEmail(string anEmail, string anOriginalEmail) {
            if (!EmailValidation.IsValidEmail(anEmail)) {
                theValidationDictionary.AddError("Email", anEmail.Trim(), INVALID_EMAIL);
            } else if (anOriginalEmail != null && (anOriginalEmail != anEmail)
                && (theUserRepo.EmailRegistered(anEmail))) {
                theValidationDictionary.AddError("Email", anEmail, "Someone already registered with that email. Please try another one.");
            }

            return theValidationDictionary.isValid;
        }

        #endregion
    }
}