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
using System.Linq;
using Social.Generic;
using Social.Generic.Models;
using HaveAVoice.Helpers.Authority;
using System.Text.RegularExpressions;
using HaveAVoice.Services.Email;

namespace HaveAVoice.Services.UserFeatures {
    public class HAVUserService : IHAVUserService {
        private const string ACTIVATION_SUBJECT = "have a voice | account activation";
        private const string ACTIVATION_BODY = "Hello, <br /><br />To finalize completion of your have a voice account, please click following link or copy and paste it into your browser: ";
        private const string INVALID_EMAIL = "That is not a valid email.";

        private IValidationDictionary theValidationDictionary;
        private IUserRetrievalService<User> theUserRetrievalService;
        private IHAVAuthorityVerificationService theAuthorityVerificationService;
        private IHAVAuthenticationService theAuthService;
        private IHAVUserRepository theUserRepo;
        private IEmail theEmailService;

        public HAVUserService(IValidationDictionary theValidationDictionary)
            : this(theValidationDictionary, new UserRetrievalService<User>(new EntityHAVUserRetrievalRepository()), new HAVAuthorityVerificationService(theValidationDictionary), new HAVAuthenticationService(), 
                    new EntityHAVUserRepository(), new EmailService()) { }

        public HAVUserService(IValidationDictionary aValidationDictionary, IUserRetrievalService<User> aUserRetrievalService,
                                         IHAVAuthorityVerificationService anAuthorityVerificationService, IHAVAuthenticationService anAuthService,
                                         IHAVUserRepository aUserRepo, IEmail aEmailService) {
            theValidationDictionary = aValidationDictionary;
            theUserRetrievalService = aUserRetrievalService;
            theAuthorityVerificationService = anAuthorityVerificationService;
            theAuthService = anAuthService;
            theUserRepo = aUserRepo;
            theEmailService = aEmailService;
        }

        public bool CreateUserAuthority(User aUserToCreate, string anExtraInfo, string aToken, string anAuthorityType, string anIpAddress) {
            if (!ValidateNewUser(aUserToCreate)) {
                return false;
            }

            if (!ValidateToken(aUserToCreate.Email, aToken, anAuthorityType, aUserToCreate.UserPositionId)) {
                return false;
            }

            aUserToCreate = CompleteAddingFieldsToUser(aUserToCreate, anIpAddress);
            aUserToCreate = theUserRepo.CreateUser(aUserToCreate, Constants.NOT_CONFIRMED_USER_ROLE).Model;
            theAuthorityVerificationService.AddExtraInfoToAuthority(aUserToCreate, anExtraInfo);

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
            if (!ValidateEditedUser(aUser, aUser.OriginalEmail) | ((aUser.NewPassword != string.Empty || aUser.RetypedPassword != string.Empty)
                    && !PasswordValidation.ValidPassword(theValidationDictionary, aUser.NewPassword, aUser.RetypedPassword))) {
                return false;
            } 

            User myOriginalUser = theUserRetrievalService.GetUser(aUser.Id);
            myOriginalUser.Email = aUser.Email.Trim();
            myOriginalUser.Password = aHashedPassword;
            myOriginalUser.FirstName = aUser.FirstName.Trim();
            myOriginalUser.LastName = aUser.LastName.Trim();
            myOriginalUser.DateOfBirth = aUser.DateOfBirth;
            myOriginalUser.City = aUser.City.Trim();
            myOriginalUser.State = aUser.State;
            myOriginalUser.Zip = int.Parse(aUser.Zip);
            myOriginalUser.Website = aUser.Website.Trim();
            myOriginalUser.Gender = aUser.Gender;
            myOriginalUser.AboutMe = aUser.AboutMe.Trim();
            myOriginalUser.UseUsername = aUser.UseUsername;

            if (!string.IsNullOrEmpty(aUser.ShortUrl)) {
                myOriginalUser.ShortUrl = aUser.ShortUrl.Trim();
            }

            if (!string.IsNullOrEmpty(aUser.Username)) {
                myOriginalUser.Username = aUser.Username.Trim();
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
                ProfilePictureURL= PhotoHelper.ProfilePicture(myUser),
            };
        }

        public EditUserSpecificRegionModel GetUserSpecificRegionForEdit(User aUser) {
            User myUser = theUserRetrievalService.GetUser(aUser.Id);

            IEnumerable<RegionSpecific> myClashingRegions =
                theAuthorityVerificationService.GetClashingRegions(myUser.City, myUser.State, myUser.Zip);
            IEnumerable<UserPosition> myAuthorityPositionsWithClash = (from a in myClashingRegions
                                                                       select a.UserPosition).Distinct();


            List<Pair<UserPosition, SelectList>> myRegionClashes = new List<Pair<UserPosition, SelectList>>();
            foreach (UserPosition myUserPosition in myAuthorityPositionsWithClash) {
                IDictionary<string, string> myRegionSpecificDictionary = new Dictionary<string, string>();
                myRegionSpecificDictionary.Add(Constants.SELECT, Constants.SELECT);

                foreach (RegionSpecific myRegionSpecific in myClashingRegions) {
                    myRegionSpecificDictionary.Add(myRegionSpecific.Id.ToString(), myRegionSpecific.AlgorithmSpecificInformation + " " + myRegionSpecific.AlgorithmText);
                }

                UserRegionSpecific myUserRegionSpecific = theAuthorityVerificationService.GetRegionSpecifcInformationForUser(aUser, myUserPosition);

                SelectList mySelectList = new SelectList(myRegionSpecificDictionary, "Key", "Value");

                if (myUserRegionSpecific != null) {
                    mySelectList = new SelectList(myRegionSpecificDictionary, "Key", "Value", myUserRegionSpecific.RegionSpecificId);
                }

                Pair<UserPosition, SelectList> myPair = new Pair<UserPosition, SelectList>() {
                    First = myUserPosition,
                    Second = mySelectList
                };


                myRegionClashes.Add(myPair);
            }

            return new EditUserSpecificRegionModel() {
                RegionClashes = myRegionClashes
            };
        }

        public void EditUserSpecificRegions(UserInformationModel<User> aUserInfo, EditUserSpecificRegionModel aModel) {
            IEnumerable<Pair<AuthorityPosition, int>> myToUpdateOrAdd = aModel.Responses.Where(r => r.Second != 0).Select(r => r);
            IEnumerable<Pair<AuthorityPosition, int>> myToDelete = aModel.Responses.Where(r => r.Second == 0).Select(r => r);
            
            theAuthorityVerificationService.UpdateUserRegionSpecifics(aUserInfo, myToUpdateOrAdd, myToDelete);
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

            return ValidateUser(aUser);
        }

        private bool ValidateEditedUser(EditUserModel aUser, string aOriginalEmail) {
            ValidEmail(aUser.Email, aOriginalEmail);

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
            if (!string.IsNullOrEmpty(aUser.ShortUrl)) {
                if (theUserRepo.ShortUrlTaken(aUser.ShortUrl)) {
                    theValidationDictionary.AddError("ShortUrl", aUser.ShortUrl, "That have a voice URL is already taken by another member.");
                } else {
                    Regex myRegex = new Regex(RegexHelper.OnlyCharactersAndNumbersAndPeriods(), RegexOptions.IgnoreCase);
                    Match myMatch = myRegex.Match(aUser.ShortUrl);
                    if (!myMatch.Success) {
                        theValidationDictionary.AddError("ShortUrl", aUser.ShortUrl, "That have a voice URL can only consist of letters, numbers, and periods.");
                    }
                }
            }

            if (!string.IsNullOrEmpty(aUser.Username) && theUserRepo.IsUserNameTaken(aUser.Username.Trim())) {
                theValidationDictionary.AddError("Username", aUser.Username, "That username is already taken. Please choose another.");
            } else if (!string.IsNullOrEmpty(aUser.Username) && !VarCharValidation.Valid15Length(aUser.Username)) {
                theValidationDictionary.AddError("Username", aUser.Username, "A username can only have a max of 15 characters..");
            }

            if (aUser.UseUsername && (string.IsNullOrEmpty(aUser.Username) && string.IsNullOrEmpty(aUser.OriginalUsername))) {
                theValidationDictionary.AddError("UseUsername", aUser.UseUsername.ToString(), "To use a username you first have to specify one.");
            }

            return theValidationDictionary.isValid;
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