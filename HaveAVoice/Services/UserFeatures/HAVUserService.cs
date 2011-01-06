using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using HaveAVoice.Exceptions;
using HaveAVoice.Helpers;
using HaveAVoice.Models;
using HaveAVoice.Models.View;
using HaveAVoice.Repositories;
using HaveAVoice.Repositories.UserFeatures;
using HaveAVoice.Services.Helpers;
using HaveAVoice.Validation;


namespace HaveAVoice.Services.UserFeatures {
    public class HAVUserService : HAVBaseService, IHAVUserService {
        private const string ACTIVATION_SUBJECT = "have a voice | account activation";
        private const string ACTIVATION_BODY = "Hello! <br/ ><br/ > To complete account creation please click the following link: <br/ >";

        private IValidationDictionary theValidationDictionary;
        private IHAVUserRetrievalService theUserRetrievalService;
        private IHAVAuthenticationService theAuthService;
        private IHAVUserPictureService theUserPictureService;
        private IHAVUserRepository theUserRepo;
        private IHAVEmail theEmailService;

        public HAVUserService(IValidationDictionary theValidationDictionary)
            : this(theValidationDictionary, new HAVUserRetrievalService(), new HAVAuthenticationService(), new HAVUserPictureService(), 
                    new EntityHAVUserRepository(), new HAVEmail(), new HAVBaseRepository()) { }
        
        public HAVUserService(IValidationDictionary aValidationDictionary, IHAVUserRetrievalService aUserRetrievalService, 
                                         IHAVAuthenticationService anAuthService, IHAVUserPictureService aUserPictureService,  
                                         IHAVUserRepository aUserRepo, IHAVEmail aEmailService, IHAVBaseRepository baseRepository) : base(baseRepository) {
            theValidationDictionary = aValidationDictionary;
            theUserRetrievalService = aUserRetrievalService;
            theAuthService = anAuthService;
            theUserPictureService = aUserPictureService;
            theUserRepo = aUserRepo;
            theEmailService = aEmailService;
        }

        public bool CreateUser(User aUserToCreate, bool aCaptchaValid, bool aAgreement, string aIpAddress) {
            if (!ValidateNewUser(aUserToCreate)) {
                return false;
            }

            if (!ValidCaptchaImage(aCaptchaValid)) {
                return false;
            }

            if(!ValidateAgreement(aAgreement)) {
                return false;
            }

            aUserToCreate = CompleteAddingFieldsToUser(aUserToCreate, aIpAddress);
            aUserToCreate = theUserRepo.CreateUser(aUserToCreate);

            EmailException myEmailException = null;
            try {
                SendActivationCode(aUserToCreate);
            } catch (EmailException e) {
                myEmailException = e;
            }

            if (myEmailException != null) {
                try {
                    theAuthService.ActivateNewUser(aUserToCreate.ActivationCode);
                } catch (Exception e) {
                    theUserRepo.DeleteUser(aUserToCreate);
                    throw e;
                }
                throw new EmailException("Couldn't send aEmail.", myEmailException);
            }

            return true;
        }

        private User CompleteAddingFieldsToUser(User aUserToCreate, string aIpAddress) {
            TimeZone myTimezone = TimeZone.CurrentTimeZone;
            aUserToCreate.UTCOffset = myTimezone.GetUtcOffset(DateTime.Now).ToString();

            aUserToCreate.Password = PasswordHelper.HashPassword(aUserToCreate.Password);
            aUserToCreate.RegistrationDate = DateTime.UtcNow;
            aUserToCreate.RegistrationIp = aIpAddress;
            aUserToCreate.LastLogin = DateTime.UtcNow;

            string aItemToHash = aUserToCreate.Username + aUserToCreate.Email + DateTime.Today;
            aUserToCreate.ActivationCode = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(aItemToHash, "SHA1");

            return aUserToCreate;
        }

        private void SendActivationCode(User aUser) {
            string myUrl = HAVConstants.BASE_URL + "/Authentication/ActivateAccount/" + aUser.ActivationCode;
            string myActivationLink = "<a href=\"" + myUrl + "\">" + myUrl + "</a>";

            try {
                theEmailService.SendEmail(aUser.Email, ACTIVATION_SUBJECT, ACTIVATION_BODY + myActivationLink);
            } catch (Exception e) {
                throw new EmailException("Couldn't send aEmail.", e);
            }
        }
        
        public IEnumerable<UserDetailsModel> GetUserList(User aExcludedUser) {
            return theUserRepo.GetUserList(aExcludedUser);
        }

        public bool EditUser(EditUserModel aUser) {
            bool isValidFileImage = ValidateFileImage(aUser.ImageFile.FileName);

            if (!isValidFileImage || !ValidateEditedUser(aUser.UserInformation, aUser.OriginalUsername, aUser.OriginalEmail)) {
                return false;
            }

            string password = aUser.NewPassword;

            if (password.Trim() == string.Empty) {
                aUser.UserInformation.Password = aUser.OriginalPassword;
            } else if (!ValidatePassword(password, aUser.RetypedPassword)) {
                return false;
            } else {
                aUser.UserInformation.Password = PasswordHelper.HashPassword(password);
            }

            if (ShouldUploadImage(isValidFileImage, aUser.ImageFile.FileName)) {
                theUserPictureService.UploadProfilePicture(aUser.UserInformation, aUser.ImageFile);
            }

            theUserRepo.UpdateUser(aUser.UserInformation);

            return true;
            
        }

        private bool ValidateFileImage(string aImageFile) {
            if(theUserPictureService.IsValidImage(aImageFile)){
                theValidationDictionary.AddError("ProfilePictureUpload", aImageFile, "Image must be either a .jpg, .jpeg, or .gif.");
            }

            return theValidationDictionary.isValid;
        }

        private bool ShouldUploadImage(bool aValidImage, string anImageFile) {
            return !String.IsNullOrEmpty(anImageFile) && aValidImage;
        }

        public EditUserModel GetUserForEdit(User aUser) {
            int myUserId = aUser.Id;
            IEnumerable<SelectListItem> myTimezones =
                new SelectList(TimezoneHelper.GetTimeZones(), TimezoneHelper.GetTimezone(aUser.UTCOffset));
            IEnumerable<SelectListItem> states =
                new SelectList(HAVConstants.STATES, aUser.State);
            User myUser = theUserRetrievalService.GetUser(myUserId);

            return new EditUserModel(myUser) {
                Timezones = myTimezones,
                States = states,
                ProfilePictureURL= PhotoHelper.ProfilePicture(myUser)
            };
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

        private bool ValidateNewUser(User aUser) {
            if (aUser.Password.Trim().Length == 0) {
                theValidationDictionary.AddError("Password", aUser.Password.Trim(), "Password is required.");
            }
            if (aUser.Username.Trim().Length == 0) {
                theValidationDictionary.AddError("Username", aUser.Username.Trim(), "Username is required.");
            } else if (theUserRepo.UsernameRegistered(aUser.Username)) {
                theValidationDictionary.AddError("Username", aUser.Username.Trim(), "Someone already registered with that username. Please try another one.");
            }
            if (aUser.Email.Trim().Length == 0) {
                theValidationDictionary.AddError("Email", aUser.Email.Trim(), "E-mail is required.");
            } else if (theUserRepo.EmailRegistered(aUser.Email.Trim())) {
                theValidationDictionary.AddError("Email", aUser.Email.Trim(), "Someone already registered with that email. Please try another one.");
            }

            return ValidateUser(aUser);
        }

        private bool ValidateEditedUser(User aUser, string aOriginalUsername, string aOriginalEmail) {
            ValidateUsername(aUser, aOriginalUsername);
            ValidEmail(aUser.Email, aOriginalEmail);
            return ValidateUser(aUser);
        }

        private void ValidateUsername(User aUser, string aOriginalUsername) {
            if (aUser.Username.Trim().Length == 0) {
                theValidationDictionary.AddError("Username", aUser.Username.Trim(), "Username is required.");
            } else if (aOriginalUsername != null && (aOriginalUsername != aUser.Username)
                && (theUserRepo.UsernameRegistered(aUser.Username))) {
                theValidationDictionary.AddError("Username", aUser.Username, "Someone already registered with that username. Please try another one.");
            }
        }

        private bool ValidateUser(User aUser) {
            if (aUser.FirstName.Trim().Length == 0) {
                theValidationDictionary.AddError("FirstName", aUser.FirstName.Trim(), "First name is required.");
            }
            if (aUser.LastName.Trim().Length == 0) {
                theValidationDictionary.AddError("LastName", aUser.LastName.Trim(), "Last name is required.");
            }
            if (aUser.City.Trim().Length == 0) {
                theValidationDictionary.AddError("City", aUser.City.Trim(), "City is required.");
            }
            if (aUser.State.Trim().Length != 2) {
                theValidationDictionary.AddError("State", aUser.State.Trim(), "State is required.");
            }
            if (aUser.Email.Length > 0 && !Regex.IsMatch(aUser.Email, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$")) {
                theValidationDictionary.AddError("Email", aUser.Email, "Invalid email address.");
            }
            if (aUser.DateOfBirth.Year == 1) {
                theValidationDictionary.AddError("DateOfBirth", aUser.DateOfBirth.ToString(), "Date of Birth required.");
            }
            if (aUser.DateOfBirth > DateTime.Today.AddYears(-18)) {
                theValidationDictionary.AddError("DateOfBirth", aUser.DateOfBirth.ToString(), "You must be at least 18 years old.");
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

        private bool ValidateUser(User aUser, string anEmail) {
            bool myResult = true;
            if (aUser == null) {
                theValidationDictionary.AddError("Email", anEmail, "That email doesn't match the requested account.");
                myResult = false;
            }

            return myResult;
        }

        private bool ValidForgotPasswordHash(string aHash) {
            if (aHash.Trim().Length == 0) {
                theValidationDictionary.AddError("ForgotPasswordHash", aHash, "Error reading the forgot password hash. Please click the link in the email again.");
            }

            return theValidationDictionary.isValid;
        }

        private bool ValidEmail(string anEmail, string anOriginalEmail) {
            if (anEmail.Trim().Length == 0) {
                theValidationDictionary.AddError("Email", anEmail.Trim(), "E-mail is required.");
            } else if (anOriginalEmail != null && (anOriginalEmail != anEmail)
                && (theUserRepo.EmailRegistered(anEmail))) {
                theValidationDictionary.AddError("Email", anEmail, "Someone already registered with that email. Please try another one.");
            }

            return theValidationDictionary.isValid;
        }

        #endregion
    }
}