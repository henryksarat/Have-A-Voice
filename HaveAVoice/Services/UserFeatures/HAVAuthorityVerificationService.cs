using System;
using System.Collections.Generic;
using System.Text;
using HaveAVoice.Exceptions;
using HaveAVoice.Helpers;
using HaveAVoice.Models;
using HaveAVoice.Repositories.UserFeatures;
using Social.Admin.Helpers;
using Social.Email;
using Social.Email.Exceptions;
using Social.Generic.Helpers;
using Social.Generic.Models;
using Social.Validation;
using Social.Generic.Constants;
using Social.Admin.Exceptions;
using System.Linq;
using Social.Generic;
using HaveAVoice.Helpers.Authority;
using System.Collections;
using HaveAVoice.Services.Email;

namespace HaveAVoice.Services.UserFeatures {
    public class HAVAuthorityVerificationService : IHAVAuthorityVerificationService {
        private const string TOKEN_SUBJECT = "have a voice | authority authentication";
        private const string TOKEN_BODY = "Hello!<br/><br/>You are classified as an authority and therefore must sign up in a special way. Please click this link or copy and paste the URL into your browser: ";
        private const string INVALID_EMAIL = "That is not a valid email.";

        private IValidationDictionary theValidationDictionary;
        private IHAVAuthorityVerificationRepository theAuthenticationRepo;
        private IEmail theEmailService;

        public HAVAuthorityVerificationService(IValidationDictionary aValidationDictionary) 
            : this(aValidationDictionary, new EmailService(), new EntityHAVAuthorityVerificationRepository()) { }

        public HAVAuthorityVerificationService(IValidationDictionary aValidationDictionary, IEmail anEmailService, IHAVAuthorityVerificationRepository anAuthorityService) {
                theValidationDictionary = aValidationDictionary;
            theAuthenticationRepo = anAuthorityService;
            theEmailService = anEmailService;
        }

        public bool AddZipCodesForUser(UserInformationModel<User> anAdminUser, string anEmail, string aZipCodes) {
            if (!PermissionHelper<User>.AllowedToPerformAction(anAdminUser, SocialPermission.Create_Authority_Verification_Token)) {
                throw new PermissionDenied(ErrorKeys.PERMISSION_DENIED);
            }

            if (!IsValidZipCodes(anEmail, aZipCodes)) {
                return false;
            }

            IEnumerable<int> myZipCodes = aZipCodes.Split(',').Select(z => int.Parse(z.Trim()));

            theAuthenticationRepo.AddZipCodesForUser(anAdminUser.Details, anEmail, myZipCodes);

            return true;            
        }

        public IEnumerable<AuthorityViewableZipCode> GetAuthorityViewableZipCodes(UserInformationModel<User> anAdminUser, string anEmail) {
            if (!PermissionHelper<User>.AllowedToPerformAction(anAdminUser, SocialPermission.Create_Authority_Verification_Token)) {
                throw new PermissionDenied(ErrorKeys.PERMISSION_DENIED);
            }

            return theAuthenticationRepo.GetAuthorityViewableZipCodes(anEmail);
        }

        public IEnumerable<RegionSpecific> GetClashingRegions(string aCity, string aState, int aZip) {
            return theAuthenticationRepo.GetClashingRegions(aCity, aState, aZip);
        }

        public UserRegionSpecific GetRegionSpecifcInformationForUser(User aUser, UserPosition aUserPosition) {
            return theAuthenticationRepo.GetRegionSpecifcInformationForUser(aUser, aUserPosition);
        }

        public bool RequestTokenForAuthority(UserInformationModel<User> aRequestingUser, string anEmail, string anExtraInfo, string anAuthorityType, string anAuthorityPosition) {
            if (!PermissionHelper<User>.AllowedToPerformAction(aRequestingUser, SocialPermission.Create_Authority_Verification_Token)) {
                throw new CustomException(HAVConstants.NOT_ALLOWED);
            }

            if(!IsValidAuthorityInformation(anEmail, anAuthorityType, anAuthorityPosition)) {
                return false;
            }

            Random myRandom = new Random(DateTime.UtcNow.Millisecond);
            string myToken = myRandom.Next().ToString();
            string myHashedToken = HashHelper.DoHash(myToken);

            bool myExists = theAuthenticationRepo.TokenForEmailExists(anEmail, anAuthorityType, anAuthorityPosition);

            if (myExists) {
                theAuthenticationRepo.UpdateTokenForEmail(anEmail, myHashedToken, anAuthorityType, anAuthorityPosition);
            } else {
                theAuthenticationRepo.CreateTokenForEmail(aRequestingUser.Details, anEmail, myHashedToken, anAuthorityType, anAuthorityPosition);
            }

            SendTokenEmail(anEmail, anExtraInfo, myToken, anAuthorityType, anAuthorityPosition);

            return true;
        }

        public bool IsValidToken(string anEmail, string aToken, string anAuthorityType, string anAuthorityPosition) {
            return theAuthenticationRepo.IsValidEmailWithToken(anEmail, HashHelper.DoHash(aToken), anAuthorityType, anAuthorityPosition);
        }

        public void VerifyAuthority(string anEmail, string aToken, string anAuthorityType, string anAuthorityPosition) {
            theAuthenticationRepo.SetEmailWithTokenToVerified(anEmail, aToken, anAuthorityType, anAuthorityPosition);
        }

        private bool IsValidZipCodes(string anEmail, string aZipCodes) {
            if (!EmailValidation.IsValidEmail(anEmail)) {
                theValidationDictionary.AddError("Email", anEmail, INVALID_EMAIL);
            }

            foreach (string myZipCode in aZipCodes.Split(',').Select(z => z.Trim())) {
                int myParsed;

                if(!int.TryParse(myZipCode, out myParsed)) {
                    if(myZipCode.Length != 5) {
                        theValidationDictionary.AddError("ZipCodes", aZipCodes, "A zip code is incorrect. Zip Codes must be 5 integers long.");
                    }
                }

            }

            return theValidationDictionary.isValid;
        }


        private bool IsValidAuthorityInformation(string anEmail, string anAuthorityType, string anAuthorityPosition) {
            if (!EmailValidation.IsValidEmail(anEmail)) {
                theValidationDictionary.AddError("Email", anEmail, INVALID_EMAIL);
            }

            if (anAuthorityType.ToUpper().Equals("SELECT")) {
                theValidationDictionary.AddError("AuthorityType", anAuthorityType, "Authority type is required.");
            }

            if (anAuthorityPosition.ToUpper().Equals("SELECT")) {
                theValidationDictionary.AddError("AuthorityPosition", anAuthorityPosition, "Authority position is required.");
            }

            return theValidationDictionary.isValid;
        }

        private void SendTokenEmail(string anEmail, string anExtraInfo, string aToken, string anAuthorityType, string anAuthorityPosition) {
            string myUrl = new StringBuilder()
                .Append(HAVConstants.BASE_URL)
                .Append("/AuthorityVerification/Verify")
                .Append("?token=")
                .Append(aToken)
                .Append("&authorityType=")
                .Append(anAuthorityType)
                .Append("&authorityPosition=")
                .Append(anAuthorityPosition)
                .Append("&extraInfo=")
                .Append(anExtraInfo)
                .ToString();
            myUrl = "<a href=\"" + myUrl + "\">" + myUrl + "</a>";
            try {
                theEmailService.SendEmail("SEND AUTHORITY TOKEN", anEmail, TOKEN_SUBJECT, TOKEN_BODY + myUrl);
            } catch (Exception e) {
                throw new EmailException("Couldn't send aEmail.", e);
            }
        }

        public IEnumerable<UserPosition> GetUserPositions() {
            return theAuthenticationRepo.GetUserPositions();
        }


        public void AddExtraInfoToAuthority(User aUser, string anExtraInfo) {
            theAuthenticationRepo.SetExtraInfoForAuthority(aUser, anExtraInfo);
        }


        public void UpdateUserRegionSpecifics(UserInformationModel<User> aUserInfo, IEnumerable<Pair<AuthorityPosition, int>> aToAddAndUpdate, IEnumerable<Pair<AuthorityPosition, int>> aToRemove) {
            theAuthenticationRepo.UpdateUserRegionSpecifics(aUserInfo.Details, aToAddAndUpdate, aToRemove);
        }
    }
}