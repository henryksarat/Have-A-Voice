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

namespace HaveAVoice.Services.UserFeatures {
    public class HAVAuthorityVerificationService : IHAVAuthorityVerificationService {
        private const string TOKEN_SUBJECT = "have a voice | authority authentication";
        private const string TOKEN_BODY = "Hello!\nYou are classified as an authority and therefore must sign up in a special way. Please follow this link: ";
        private const string INVALID_EMAIL = "That is not a valid email.";

        private IValidationDictionary theValidationDictionary;
        private IHAVAuthorityVerificationRepository theAuthenticationRepo;
        private IEmail theEmailService;

        public HAVAuthorityVerificationService(IValidationDictionary aValidationDictionary) 
            : this(aValidationDictionary, new SocialEmail(), new EntityHAVAuthorityVerificationRepository()) { }

        public HAVAuthorityVerificationService(IValidationDictionary aValidationDictionary, IEmail anEmailService, IHAVAuthorityVerificationRepository anAuthorityService) {
                theValidationDictionary = aValidationDictionary;
            theAuthenticationRepo = anAuthorityService;
            theEmailService = anEmailService;
        }

        public bool RequestTokenForAuthority(UserInformationModel<User> aRequestingUser, string anEmail, string anAuthorityType, string anAuthorityPosition) {
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

            SendTokenEmail(anEmail, myToken, anAuthorityType, anAuthorityPosition);

            return true;
        }

        public bool IsValidToken(string anEmail, string aToken, string anAuthorityType, string anAuthorityPosition) {
            return theAuthenticationRepo.IsValidEmailWithToken(anEmail, HashHelper.DoHash(aToken), anAuthorityType, anAuthorityPosition);
        }

        public void VerifyAuthority(string anEmail, string aToken, string anAuthorityType, string anAuthorityPosition) {
            theAuthenticationRepo.SetEmailWithTokenToVerified(anEmail, aToken, anAuthorityType, anAuthorityPosition);
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

        private void SendTokenEmail(string anEmail, string aToken, string anAuthorityType, string anAuthorityPosition) {
            string myUrl = new StringBuilder()
                .Append(HAVConstants.BASE_URL)
                .Append("/AuthorityVerification/Verify")
                .Append("?token=")
                .Append(aToken)
                .Append("&authorityType=")
                .Append(anAuthorityType)
                .Append("&authorityPosition=")
                .Append(anAuthorityPosition)
                .ToString();
            try {
                theEmailService.SendEmail(anEmail, TOKEN_SUBJECT, TOKEN_BODY + myUrl);
            } catch (Exception e) {
                throw new EmailException("Couldn't send aEmail.", e);
            }
        }

        public IEnumerable<UserPosition> GetUserPositions() {
            return theAuthenticationRepo.GetUserPositions();
        }
    }
}