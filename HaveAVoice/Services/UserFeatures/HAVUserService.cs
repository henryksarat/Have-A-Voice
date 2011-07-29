using System;
using System.Collections.Generic;
using System.Web.Mvc;
using HaveAVoice.Helpers;
using HaveAVoice.Models;
using HaveAVoice.Models.View;
using HaveAVoice.Repositories.UserFeatures;
using HaveAVoice.Services.Helpers;
using Social.Email;
using Social.Email.Exceptions;
using Social.Generic.Constants;
using Social.Generic.Helpers;
using Social.User.Repositories;
using Social.User.Services;
using Social.Validation;

namespace HaveAVoice.Services.UserFeatures {
    public class HAVUserService : IHAVUserService {
        private const string ACTIVATION_SUBJECT = "have a voice | account activation";
        private const string ACTIVATION_BODY = "Hello, <br /><br />To finalize completion of your have a voice account, please click following link or copy and paste it into your browser: ";
        private const string INVALID_EMAIL = "That is not a valid email.";

        private IValidationDictionary theValidationDictionary;
        private IUserRetrievalService<User> theUserRetrievalService;
        private IHAVAuthorityVerificationService theAuthorityVerificationService;
        private IHAVAuthenticationService theAuthService;
        private IUserRepository<User, Role, UserRole> theUserRepo;
        private IEmail theEmailService;

        public HAVUserService(IValidationDictionary theValidationDictionary)
            : this(theValidationDictionary, new UserRetrievalService<User>(new EntityHAVUserRetrievalRepository()), new HAVAuthorityVerificationService(theValidationDictionary), new HAVAuthenticationService(), 
                    new EntityHAVUserRepository(), new SocialEmail()) { }

        public HAVUserService(IValidationDictionary aValidationDictionary, IUserRetrievalService<User> aUserRetrievalService,
                                         IHAVAuthorityVerificationService anAuthorityVerificationService, IHAVAuthenticationService anAuthService,
                                         IUserRepository<User, Role, UserRole> aUserRepo, IEmail aEmailService) {
            theValidationDictionary = aValidationDictionary;
            theUserRetrievalService = aUserRetrievalService;
            theAuthorityVerificationService = anAuthorityVerificationService;
            theAuthService = anAuthService;
            theUserRepo = aUserRepo;
            theEmailService = aEmailService;
        }

        public bool CreateUserAuthority(User aUserToCreate, string aToken, string anAuthorityType, bool anAgreement, string anIpAddress) {
            if (!ValidateNewUser(aUserToCreate)) {
                return false;
            }

            if (!ValidateToken(aUserToCreate.Email, aToken, anAuthorityType, aUserToCreate.UserPositionId)) {
                return false;
            }

            if (!ValidateAgreement(anAgreement)) {
                return false;
            }

            aUserToCreate = CompleteAddingFieldsToUser(aUserToCreate, anIpAddress);
            aUserToCreate = theUserRepo.CreateUser(aUserToCreate, Constants.NOT_CONFIRMED_USER_ROLE).Model;

            try {
                theAuthService.ActivateAuthority(aUserToCreate.ActivationCode, anAuthorityType);
            } catch (Exception e) {
                theUserRepo.DeleteUser(aUserToCreate);
                throw e;
            }

            return true;
        }

        private User CompleteAddingFieldsToUser(User aUserToCreate, string aIpAddress) {
            aUserToCreate.Password = HashHelper.DoHash(aUserToCreate.Password);
            aUserToCreate.RegistrationDate = DateTime.UtcNow;
            aUserToCreate.RegistrationIp = aIpAddress;
            aUserToCreate.LastLogin = DateTime.UtcNow;

            string aItemToHash = NameHelper.FullName(aUserToCreate) + aUserToCreate.Email + DateTime.Today;
            aUserToCreate.ActivationCode = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(aItemToHash, "SHA1");

            return aUserToCreate;
        }

        public bool EditUser(EditUserModel aUser, string aHashedPassword) {
            if (!ValidateEditedUser(aUser, aUser.OriginalEmail)
                | ((aUser.NewPassword != string.Empty || aUser.RetypedPassword != string.Empty)
                    && !PasswordValidation.ValidPassword(theValidationDictionary, aUser.NewPassword, aUser.RetypedPassword))) {
                return false;
            } 

            User myOriginalUser = theUserRetrievalService.GetUser(aUser.Id);
            myOriginalUser.Email = aUser.Email;
            myOriginalUser.Password = aHashedPassword;
            myOriginalUser.FirstName = aUser.FirstName;
            myOriginalUser.LastName = aUser.LastName;
            myOriginalUser.DateOfBirth = aUser.DateOfBirth;
            myOriginalUser.City = aUser.City;
            myOriginalUser.State = aUser.State;
            myOriginalUser.Zip = aUser.Zip;
            myOriginalUser.Website = aUser.Website;
            myOriginalUser.Gender = aUser.Gender;
            myOriginalUser.AboutMe = aUser.AboutMe;
            if (!string.IsNullOrEmpty(aUser.ShortUrl)) {
                myOriginalUser.ShortUrl = aUser.ShortUrl;
            }

            theUserRepo.UpdateUser(myOriginalUser);

            return true;
        }

        private bool ShouldUploadImage(bool aValidImage, string anImageFile) {
            return !String.IsNullOrEmpty(anImageFile) && aValidImage;
        }

        public EditUserModel GetUserForEdit(User aUser) {
            int myUserId = aUser.Id;
            User myUser = theUserRetrievalService.GetUser(myUserId);
            IEnumerable<SelectListItem> myStates =
                new SelectList(UnitedStates.STATES, myUser.State);
            IEnumerable<SelectListItem> myGenders =
                new SelectList(Constants.GENDERS, myUser.Gender);

            return new EditUserModel(myUser) {
                OriginalEmail = myUser.Email,
                OriginalFullName = NameHelper.FullName(myUser),
                OriginalGender = myUser.Gender,
                OriginalPassword = myUser.Password,
                OriginalWebsite = myUser.Website,
                States = myStates,
                Genders = myGenders,
                ProfilePictureURL= PhotoHelper.ProfilePicture(myUser)
            };
        }

        private void SendActivationCode(User aUser) {
            string myUrl = HAVConstants.BASE_URL + "/Authentication/ActivateAccount/" + aUser.ActivationCode;
            myUrl = "<a href=\"" + myUrl + "\">" + myUrl + "</a>";

            try {
                theEmailService.SendEmail(aUser.Email, ACTIVATION_SUBJECT, ACTIVATION_BODY + myUrl);
            } catch (Exception e) {
                throw new EmailException("Couldn't send aEmail.", e);
            }
        }

        #region Validation"

        private bool ValidateToken(string anEmail, string aToken, string anAuthorityType, string anAuthorityPosition) {
            if (!theAuthorityVerificationService.IsValidToken(anEmail, aToken, anAuthorityType, anAuthorityPosition)) {
                theValidationDictionary.AddError("Token", aToken, "An error occurred while authenticating you as an authority. Please follow the steps sent to your email again or contact henryksarat@haveavoice.com.");
            }

            return theValidationDictionary.isValid;
        }

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
            if (!EmailValidation.IsValidEmail(aUser.Email)) {
                theValidationDictionary.AddError("Email", aUser.Email.Trim(), "E-mail is required.");
            } else if (theUserRepo.EmailRegistered(aUser.Email.Trim())) {
                theValidationDictionary.AddError("Email", aUser.Email.Trim(), "Someone already registered with that email. Please try another one.");
            }
            if (aUser.ShortUrl.Length == 0) {
                theValidationDictionary.AddError("ShortUrl", aUser.ShortUrl, "You must give yourself a www.haveavoice.com url.");
            } else if (theUserRepo.ShortUrlTaken(aUser.ShortUrl)) {
                theValidationDictionary.AddError("ShortUrl", aUser.ShortUrl, "That www.haveavoice.com url is already taken by someone else.");
            }

            return ValidateUser(aUser);
        }

        private bool ValidateEditedUser(EditUserModel aUser, string aOriginalEmail) {
            ValidEmail(aUser.Email, aOriginalEmail);

            DateOfBirthValidation.ValidDateOfBirth(theValidationDictionary, aUser.DateOfBirth);

            if (aUser.FirstName.Trim().Length == 0) {
                theValidationDictionary.AddError("FirstName", aUser.FirstName.Trim(), "First name is required.");
            }
            if (aUser.LastName.Trim().Length == 0) {
                theValidationDictionary.AddError("LastName", aUser.LastName.Trim(), "Last name is required.");
            }
            if (aUser.City.Trim().Length == 0) {
                theValidationDictionary.AddError("City", aUser.City.Trim(), "City is required.");
            }
            if (aUser.Gender.Equals(Constants.SELECT)) {
                theValidationDictionary.AddError("Gender", Constants.SELECT, "Gender is required.");
            }
            if (aUser.State.Trim().Length != 2) {
                theValidationDictionary.AddError("State", aUser.State.Trim(), "State is required.");
            }
            if (aUser.Zip.ToString().Trim().Length != 5) {
                theValidationDictionary.AddError("Zip", aUser.Zip.ToString().Trim(), "Zip code must be 5 digits.");
            }
            if (!EmailValidation.IsValidEmail(aUser.Email)) {
                theValidationDictionary.AddError("Email", aUser.Email, INVALID_EMAIL);
            }
            if (!string.IsNullOrEmpty(aUser.ShortUrl) && theUserRepo.ShortUrlTaken(aUser.ShortUrl)) {
                theValidationDictionary.AddError("ShortUrl", aUser.ShortUrl, "That have a voice URL is already taken by another member.");
            }

            return theValidationDictionary.isValid;
        }

        private bool ValidateUser(User aUser) {
            DateOfBirthValidation.ValidDateOfBirth(theValidationDictionary, aUser.DateOfBirth);

            if (aUser.FirstName.Trim().Length == 0) {
                theValidationDictionary.AddError("FirstName", aUser.FirstName.Trim(), "First name is required.");
            }
            if (aUser.LastName.Trim().Length == 0) {
                theValidationDictionary.AddError("LastName", aUser.LastName.Trim(), "Last name is required.");
            }
            if (aUser.City.Trim().Length == 0) {
                theValidationDictionary.AddError("City", aUser.City.Trim(), "City is required.");
            }
            if (aUser.Gender.Equals(Constants.SELECT)) {
                theValidationDictionary.AddError("Gender", Constants.SELECT, "Gender is required.");
            }
            if (aUser.State.Trim().Length != 2) {
                theValidationDictionary.AddError("State", aUser.State.Trim(), "State is required.");
            }
            if (aUser.Zip.ToString().Trim().Length != 5) {
                theValidationDictionary.AddError("Zip", aUser.Zip.ToString().Trim(), "Zip code must be 5 digits.");
            }
            if (!EmailValidation.IsValidEmail(aUser.Email)) {
                theValidationDictionary.AddError("Email", aUser.Email, INVALID_EMAIL);
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
            if (!EmailValidation.IsValidEmail(anEmail)) {
                theValidationDictionary.AddError("Email", anEmail.Trim(), INVALID_EMAIL);
            } else if (anOriginalEmail != null && (anOriginalEmail != anEmail)
                && (theUserRepo.EmailRegistered(anEmail))) {
                theValidationDictionary.AddError("Email", anEmail, "Someone already registered with that email. Please try another one.");
            }

            return theValidationDictionary.isValid;
        }

        private bool ValidateFileImage(string aImageFile) {
            if (!PhotoValidation.IsValidImageFile(aImageFile)) {
                theValidationDictionary.AddError("ProfilePictureUpload", aImageFile, "Image must be either a .jpg, .jpeg, or .gif.");
            }

            return theValidationDictionary.isValid;
        }

        #endregion
    }
}