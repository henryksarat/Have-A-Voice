using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Repositories.UserFeatures;
using HaveAVoice.Repositories;
using HaveAVoice.Services.Helpers;
using HaveAVoice.Models;
using HaveAVoice.Exceptions;
using HaveAVoice.Helpers;
using HaveAVoice.Models.View;
using HaveAVoice.Validation;
using System.Text;

namespace HaveAVoice.Services.UserFeatures {
    public class HAVAuthorityVerificationService : HAVBaseService, IHAVAuthorityVerificationService {
        private const string TOKEN_SUBJECT = "have a voice | authority authentication";
        private const string TOKEN_BODY = "Hello! <br/ ><br/ > You are classified as an authority and therefore must sign up in a special way. Please follow this link: <br/ >";
        private const string INVALID_EMAIL = "That is not a valid email.";

        private IValidationDictionary theValidationDictionary;
        private IHAVAuthorityVerificationRepository theAuthenticationRepo;
        private IHAVEmail theEmailService;

        public HAVAuthorityVerificationService(IValidationDictionary aValidationDictionary) 
            : this(aValidationDictionary, new HAVBaseRepository(), new HAVEmail(), new EntityHAVAuthorityVerificationRepository()) { }

        public HAVAuthorityVerificationService(IValidationDictionary aValidationDictionary, IHAVBaseRepository aBaseRepository, IHAVEmail anEmailService, IHAVAuthorityVerificationRepository anAuthorityService)
            : base(aBaseRepository) {
                theValidationDictionary = aValidationDictionary;
            theAuthenticationRepo = anAuthorityService;
            theEmailService = anEmailService;
        }

        public bool RequestTokenForAuthority(UserInformationModel aRequestingUser, string anEmail, string anAuthorityType, string anAuthorityPosition) {
            if (!HAVPermissionHelper.AllowedToPerformAction(aRequestingUser, HAVPermission.Create_Authority_Verification_Token)) {
                throw new CustomException(HAVConstants.NOT_ALLOWED);
            }

            if(!IsValidAuthorityInformation(anEmail, anAuthorityType, anAuthorityPosition)) {
                return false;
            }

            Random myRandom = new Random(DateTime.UtcNow.Millisecond);
            string myToken = myRandom.Next().ToString();
            string myHashedToken = HashHelper.HashAuthorityVerificationToken(myToken);

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
            return theAuthenticationRepo.IsValidEmailWithToken(anEmail, HashHelper.HashAuthorityVerificationToken(aToken), anAuthorityType, anAuthorityPosition);
        }

        public void VerifyAuthority(string anEmail, string aToken, string anAuthorityType, string anAuthorityPosition) {
            theAuthenticationRepo.SetEmailWithTokenToVerified(anEmail, aToken, anAuthorityType, anAuthorityPosition);
        }

        private bool IsValidAuthorityInformation(string anEmail, string anAuthorityType, string anAuthorityPosition) {
            if (!ValidationHelper.IsValidEmail(anEmail)) {
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

            string myTokenLink = "<a href=\"" + myUrl + "\">" + myUrl + "</a>";

            try {
                theEmailService.SendEmail(anEmail, TOKEN_SUBJECT, TOKEN_BODY + myTokenLink);
            } catch (Exception e) {
                throw new EmailException("Couldn't send aEmail.", e);
            }
        }

        public IEnumerable<UserPosition> GetUserPositions() {
            return theAuthenticationRepo.GetUserPositions();
        }
    }
}