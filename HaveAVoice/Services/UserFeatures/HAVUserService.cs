using System;
using System.Web;
using HaveAVoice.Validation;
using System.Collections.Generic;
using HaveAVoice.Repositories;
using System.Text.RegularExpressions;
using HaveAVoice.Helpers;
using HaveAVoice.Exceptions;
using HaveAVoice.Models.View;
using HaveAVoice.Repositories.UserFeatures;
using System.Web.Mvc;
using HaveAVoice.Repositories.AdminFeatures;
using HaveAVoice.Models;


namespace HaveAVoice.Services.UserFeatures {
    public class HAVUserService : HAVBaseService, IHAVUserService {
        public const string REMEMBER_ME_COOKIE = "HaveAVoiceRememberMeCookie";
        public const int REMEMBER_ME_COOKIE_HOURS = 4;
        public const double FORGOT_PASSWORD_MAX_DAYS = 15;

        private IValidationDictionary theValidationDictionary;
        private IHAVUserRepository theRepository;
        private IHAVRoleRepository theRoleRepository;
        private IHAVEmail theEmailService;

        public HAVUserService() :
            this(null) { }

        public HAVUserService(IValidationDictionary theValidationDictionary)
            : this(theValidationDictionary, new EntityHAVUserRepository(), 
            new EntityHAVRoleRepository(), new HAVEmail(), new HAVBaseRepository()) { }
        
        public HAVUserService(IValidationDictionary aValidationDictionary, IHAVUserRepository aRepository, IHAVRoleRepository aRoleRepository,
            IHAVEmail aEmailService, IHAVBaseRepository baseRepository) : base(baseRepository) {
            theValidationDictionary = aValidationDictionary;
            theRepository = aRepository;
            theRoleRepository = aRoleRepository;
            theEmailService = aEmailService;
        }

        #region "Validation"
        private bool ValidateAgreement(bool aAgreement) {
            if(aAgreement == false) {
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
            } else if (theRepository.UsernameRegistered(aUser.Username)) {
                theValidationDictionary.AddError("Username", aUser.Username.Trim(), "Someone already registered with that aUsername. Please try another one.");    
            }
            if (aUser.Email.Trim().Length == 0) {
                theValidationDictionary.AddError("Email", aUser.Email.Trim(), "E-mail is required.");
            } else if (theRepository.EmailRegistered(aUser.Email.Trim())) {
                theValidationDictionary.AddError("Email", aUser.Email.Trim(), "Someone already registered with that myException-mail. Please try another one.");
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
                && (theRepository.UsernameRegistered(aUser.Username))) {
                theValidationDictionary.AddError("Username", aUser.Username, "Someone already registered with that aUsername. Please try another one.");
            }
        }

        private bool ValidateUser(User aUser) {
            if(aUser.FirstName.Trim().Length == 0) {
                theValidationDictionary.AddError("FirstName", aUser.FirstName.Trim(), "First name is required.");
            }
            if(aUser.LastName.Trim().Length == 0) {
                theValidationDictionary.AddError("LastName", aUser.LastName.Trim(), "Last name is required.");
            }
            if(aUser.City.Trim().Length == 0) {
                theValidationDictionary.AddError("City", aUser.City.Trim(), "City is required.");
            }
            if(aUser.State.Trim().Length != 2) {
                theValidationDictionary.AddError("State", aUser.State.Trim(), "State is required.");
            }
            if (aUser.Email.Length > 0 && !Regex.IsMatch(aUser.Email, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$")) {
                theValidationDictionary.AddError("Email", aUser.Email, "Invalid aEmail address.");
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
                && (theRepository.EmailRegistered(anEmail))) {
                    theValidationDictionary.AddError("Email", anEmail, "Someone already registered with that myException-mail. Please try another one.");
            }

            return theValidationDictionary.isValid;
        }

        #endregion

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
            aUserToCreate = theRepository.CreateUser(aUserToCreate);

            EmailException myEmailException = null;
            try {
                SendActivationCode(aUserToCreate);
            } catch (EmailException e) {
                myEmailException = e;
            }

            if (myEmailException != null) {
                try {
                    ActivateUser(aUserToCreate.ActivationCode);
                } catch (Exception e) {
                    theRepository.DeleteUser(aUserToCreate);
                    throw e;
                }
                throw new EmailException("Couldn't send aEmail.", myEmailException);
            }

            return true;
        }

        private User ActivateUser(string anActivationCode) {
            User myUser = theRepository.FindUserByActivationCode(anActivationCode);

            if (myUser == null) {
                throw new NullUserException("There is no user matching that activation code.");
            }

            
            Role myNotConfirmedRole = theRoleRepository.GetNotConfirmedUserRole();
            if (myNotConfirmedRole == null) {
                throw new NullRoleException("There is no \"Not confirmed\" role");
            }
            Role myDefaultRole = theRoleRepository.GetDefaultRole();
            if (myDefaultRole == null) {
                throw new NullReferenceException("There is no default role.");
            }

            theRepository.RemoveUserFromRole(myUser, myNotConfirmedRole);
            theRepository.AddUserToRole(myUser, myDefaultRole);
            theRepository.AddDefaultUserPrivacySettings(myUser);
            return myUser;
        }

        private void RemoveUserFromNotConfirmedRole(User myUser) {

        }

        private User CompleteAddingFieldsToUser(User aUserToCreate, string aIpAddress) {
            TimeZone myTimezone = TimeZone.CurrentTimeZone;
            aUserToCreate.UTCOffset = myTimezone.GetUtcOffset(DateTime.Now).ToString();

            aUserToCreate.Password = HashPassword(aUserToCreate.Password);
            aUserToCreate.RegistrationDate = DateTime.UtcNow;
            aUserToCreate.RegistrationIp = aIpAddress;
            aUserToCreate.LastLogin = DateTime.UtcNow;

            string aItemToHash = aUserToCreate.Username + aUserToCreate.Email + DateTime.Today;
            aUserToCreate.ActivationCode = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(aItemToHash, "SHA1");

            return aUserToCreate;
        }

        private void SendActivationCode(User aUser) {
            try {
                theEmailService.SendEmail(aUser.Email, "Have A Voice Account Activation", "Click this link to activate your account.... " + aUser.ActivationCode);
            } catch (Exception e) {
                throw new EmailException("Couldn't send aEmail.", e);
            }
        }

       

        private string HashPassword(string aPassword) {
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(aPassword, "SHA1");
        }
        
        public UserInformationModel AuthenticateUser(string anEmail, string aPassword) {
            aPassword = HashPassword(aPassword);
            User myUser = theRepository.GetUser(anEmail, aPassword);
            
            if (myUser == null) {
                return null;
            }

            IEnumerable<Permission> myPermissions = theRepository.GetPermissionsForUser(myUser);
            Restriction myRestriction = theRepository.GetRestrictionsForUser(myUser);

            if (myRestriction == null) {
                throw new Exception("The user has no restriction.");
            }
            
            UserPrivacySetting myPrivacySettings = theRepository.GetUserPrivacySettingsForUser(myUser);
            
            if (myPrivacySettings == null) {
                throw new Exception("The user has no privacy settings.");
            }

            return new UserInformationModel(myUser, myPermissions, myRestriction, myPrivacySettings);
        }

        public void CreateRememberMeCredentials(User aUser) {
            string myCookieHash = CreateCookieHash(aUser);
            HttpCookie aCookie = new HttpCookie(REMEMBER_ME_COOKIE);
            aCookie["UserId"] = aUser.Id.ToString();
            aCookie["CookieHash"] = myCookieHash;
            aCookie.Expires = DateTime.Today.AddHours(REMEMBER_ME_COOKIE_HOURS);
            HttpContext.Current.Response.Cookies.Add(aCookie);
        }

        private string CreateCookieHash(User aUser) {
            string myTime = DateTime.Now.ToString();
            string myCookieHash =
                System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(aUser.Id + DateTime.Now.ToString(), "SHA1");
            aUser.CookieHash = myCookieHash;
            aUser.CookieCreationDate = DateTime.Now;
            theRepository.UpdateUser(aUser);
            return myCookieHash;
        }


        public User ReadRememberMeCredentials() {
            HttpCookie myCookie = HttpContext.Current.Request.Cookies.Get(REMEMBER_ME_COOKIE);
            if (myCookie != null) {
                int userId = Int32.Parse(myCookie["UserId"]);
                string cookieHash = myCookie["CookieHash"];
                User myUser = theRepository.GetUserFromCookieHash(userId, cookieHash);
                myCookie.Expires = DateTime.Now.AddDays(REMEMBER_ME_COOKIE_HOURS);
                theRepository.UpdateCookieHashCreationDate(myUser);
                return myUser;
            } else {
                return null;
            }
        }

        public UserInformationModel ActivateNewUser(string activationCode) {
            User myUser = ActivateUser(activationCode);
            IEnumerable<Permission> permissions = theRepository.GetPermissionsForUser(myUser);
            Restriction myRestriction = theRepository.GetRestrictionsForUser(myUser);
            UserPrivacySetting myPrivacySettings = theRepository.GetUserPrivacySettingsForUser(myUser);
            return new UserInformationModel(myUser, permissions, myRestriction, myPrivacySettings);
        }

        public User GetUser(int aUserId) {
            try {
                return theRepository.GetUser(aUserId);
            } catch (Exception exception) {
                string details = "Unable to get userToListenTo.";
                LogError(exception, details);
                throw exception;
            }
        }

        public IEnumerable<UserDetailsModel> GetUserList(User aExcludedUser) {
            return theRepository.GetUserList(aExcludedUser);
        }


        public IEnumerable<Timezone> GetTimezones() {
            return theRepository.GetTimezones();
        }

        public bool EditUser(EditUserModel aUser) {
            bool isValidFileImage = ValidateFileImage(aUser.ImageFile.FileName);

            if (!isValidFileImage || !ValidateEditedUser(aUser.UserInformation, aUser.OriginalUsername, aUser.OriginalEmail)) {
                return false;
            }

            string password = aUser.NewPassword;

            if (password.Trim() == string.Empty) {
                //aUser.UserInformation.Password = aUser.OriginalPassword;
            } else if (!ValidatePassword(password, aUser.RetypedPassword)) {
                return false;
            } else {
                aUser.UserInformation.Password = HashPassword(password);
            }

            if (ShouldUploadImage(isValidFileImage, aUser.ImageFile.FileName)) {
                string imageName = UploadImage(aUser.ImageFile, aUser.UserInformation);
                theRepository.AddProfilePicture(imageName, aUser.UserInformation);
            }

            theRepository.UpdateUser(aUser.UserInformation);

            return true;
            
        }


        private bool ValidateFileImage(string aImageFile) {
            if(!(String.IsNullOrEmpty(aImageFile)) && !(aImageFile.EndsWith(".jpg")) && !(aImageFile.EndsWith(".jpeg")) && !(aImageFile.EndsWith(".gif"))) {
                theValidationDictionary.AddError("ProfilePictureUpload", aImageFile, "Image must be either a .jpg, .jpeg, or .gif.");
            }

            return theValidationDictionary.isValid;
        }

        private bool ShouldUploadImage(bool aValidImage, string anImageFile) {
            return !String.IsNullOrEmpty(anImageFile) && aValidImage;
        }

        private string UploadImage(HttpPostedFileBase aImageFile, User aUser) {
            string[] splitOnPeriod = aImageFile.FileName.Split(new char[] { '.' });
            string fileExtension = splitOnPeriod[splitOnPeriod.Length - 1];
            string fileName = aUser.Id + "_" + DateTime.UtcNow.GetHashCode() + "." + fileExtension;
            string filePath = HttpContext.Current.Server.MapPath(HAVConstants.USER_PICTURE_LOCATION) + "\\" + fileName;
            aImageFile.SaveAs(filePath);
            return fileName;
        }

        public string GetProfilePictureURL(User aUser) {
            UserPicture profilePicture = theRepository.GetProfilePicture(aUser.Id);
            string profilePictureImageName;

            if (profilePicture == null) {
                profilePictureImageName = HAVConstants.NO_PROFILE_PICTURE_IMAGE;
            } else {
                profilePictureImageName = profilePicture.ImageName;
            }

            string filePath = HAVConstants.USER_PICTURE_LOCATION + "/" + profilePictureImageName;
            return filePath;
        }

        public IEnumerable<UserPicture> GetUserPictures(int aUserId) {
            return theRepository.GetUserPictures(aUserId);
        }

        public UserPicture GetProfilePicture(int aUserId) {
            return theRepository.GetProfilePicture(aUserId);
        }

        public void SetToProfilePicture(int aUserPictureId, User aUser) {
            theRepository.SetToProfilePicture(aUserPictureId, aUser);
        }

        public void DeleteUserPictures(List<int> aUserPictureIds) {
            foreach(int userPictureId in aUserPictureIds) {
                theRepository.DeleteUserPicture(userPictureId);
            }
        }

        public IEnumerable<Permission> GetPermissionsForUser(User aUser) {
            return theRepository.GetPermissionsForUser(aUser);
        }


        public void AddToWhoIsOnline(User aCurrentUser, string aCurrentIpAddress) {
            theRepository.RemoveFromWhoIsOnline(aCurrentUser, aCurrentIpAddress);
            theRepository.AddToWhoIsOnline(aCurrentUser, aCurrentIpAddress);
            theRepository.MarkForceLogOutOfOtherUsers(aCurrentUser, aCurrentIpAddress);
        }


        public bool IsOnline(User aCurrentUser, string aCurrentIpAddress) {
            WhoIsOnline onlineUser = theRepository.GetWhoIsOnlineEntry(aCurrentUser, aCurrentIpAddress);
            DateTime expiryTime = DateTime.UtcNow.AddSeconds(-1 * HAVConstants.SECONDS_BEFORE_USER_TIMEOUT);

            bool isOnline = true;
            if (onlineUser.DateTimeStamp.CompareTo(expiryTime) < 0 || onlineUser.ForceLogOut) {
                theRepository.RemoveFromWhoIsOnline(aCurrentUser, aCurrentIpAddress);
                isOnline = false;
            } else {
                theRepository.UpdateTimeOfWhoIsOnline(aCurrentUser, aCurrentIpAddress);
            }

            return isOnline;
        }


        public void RemoveFromWhoIsOnline(User aCurrentUser, string aCurrentIpAddress) {
            theRepository.RemoveFromWhoIsOnline(aCurrentUser, aCurrentIpAddress);
        }


        public UserPicture GetUserPicture(int aUserPictureId) {
            return theRepository.GetUserPicture(aUserPictureId);
        }

        public UserPrivacySetting GetUserPrivacySettings(User aUser) {
            return theRepository.GetUserPrivacySettingsForUser(aUser);
        }


        public void UpdatePrivacySettings(User aUser, UserPrivacySetting aUserPrivacySetting) {
            theRepository.UpdatePrivacySettings(aUser, aUserPrivacySetting);
        }

        public bool ForgotPasswordRequest(string anEmail) {
            if (!ValidEmail(anEmail, null)) {
                return false;
            }

            User myUser = theRepository.GetUser(anEmail);

            if (myUser == null) {
                theValidationDictionary.AddError("Email", anEmail, "That is not a valid email.");
                return false;
            }

            string myForgotPasswordHash =
                System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(anEmail + DateTime.UtcNow.ToString(), "SHA1");
            theRepository.UpdateUserForgotPasswordHash(anEmail, myForgotPasswordHash);

            SendActivationCode(anEmail, myForgotPasswordHash);

            return true;
        }


        private void SendActivationCode(string anEmail, string aPasswordHash) {
            try {
                theEmailService.SendEmail(anEmail, "have a voice | forgot password", "Click this link to change your password.... " + aPasswordHash);
            } catch (Exception e) {
                throw new EmailException("Couldn't send aEmail.", e);
            }
        }


        public bool ForgotPasswordProcess(string anEmail, string aForgotPasswordHash) {
            User myUser = theRepository.GetUserByEmailAndForgotPasswordHash(anEmail, aForgotPasswordHash);
            DateTime myDateTime = (DateTime)myUser.ForgotPasswordHashDateTimeStamp;
            
            TimeSpan myDifference = DateTime.UtcNow - myDateTime;
            if(myDifference.Days > FORGOT_PASSWORD_MAX_DAYS) {
                return false;
            }

            return true;
        }


        public bool ChangePassword(string anEmail, string aForgotPasswordHash, string aPassword, string aRetypedPassword) {
            User myUser = theRepository.GetUserByEmailAndForgotPasswordHash(anEmail, aForgotPasswordHash);
            if (!ValidateUser(myUser, anEmail)) {
                return false;
            }
            
            if(!ValidatePassword(aPassword, aRetypedPassword)) {
                return false;
            }

            if (!ValidForgotPasswordHash(aForgotPasswordHash)) {
                return false;
            }

            string myPassword = HashPassword(aPassword);
            theRepository.ChangePassword(myUser.Id, myPassword);

            return true;
        }


        public EditUserModel GetUserForEdit(User aUser) {
            int myUserId = aUser.Id;
            IEnumerable<SelectListItem> timezones =
                new SelectList(TimezoneHelper.GetTimeZones(), TimezoneHelper.GetTimezone(aUser.UTCOffset));
            IEnumerable<SelectListItem> states =
                new SelectList(HAVConstants.STATES, aUser.State);
            User user = theRepository.GetUser(myUserId);
            string profilePictureURL = GetProfilePictureURL(user);

            return new EditUserModel.Builder(user)
            .setTimezones(timezones)
            .setStates(states)
            .setProfilePictureUrl(profilePictureURL)
            .Build();
        }

        public void AddFan(User aUser, int aSourceUserId) {
            theRepository.AddFan(aUser, aSourceUserId);
        }

    }
}