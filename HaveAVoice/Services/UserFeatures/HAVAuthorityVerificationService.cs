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

namespace HaveAVoice.Services.UserFeatures {
    public class HAVAuthorityVerificationService : HAVBaseService, IHAVAuthorityVerificationService {
        private const string TOKEN_SUBJECT = "have a voice | authority authentication";
        private const string TOKEN_BODY = "Hello! <br/ ><br/ > You are classified as an authority and therefore must sign up in a special way. Please follow this link: <br/ >";

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

        public bool RequestTokenForAuthority(UserInformationModel aRequestingUser, string anEmail) {
            if (!HAVPermissionHelper.AllowedToPerformAction(aRequestingUser, HAVPermission.Create_Authority_Verification_Token)) {
                throw new CustomException(HAVConstants.NOT_ALLOWED);
            }

            if(!IsValidEmail(anEmail)) {
                return false;
            }

            Random myRandom = new Random(DateTime.UtcNow.Millisecond);
            string myToken = myRandom.Next().ToString();
            string myHashedToken = HashHelper.HashAuthorityVerificationToken(myToken);

            bool myExists = theAuthenticationRepo.TokenForEmailExists(anEmail);

            if (myExists) {
                theAuthenticationRepo.UpdateTokenForEmail(anEmail, myHashedToken);
            } else {
                theAuthenticationRepo.CreateTokenForEmail(aRequestingUser.Details, anEmail, myHashedToken);
            }

            EmailException myEmailException = null;
            try {
                SendTokenEmail(anEmail, myToken);
            } catch (EmailException e) {
                myEmailException = e;
            }

            return true;
        }

        public bool IsValidToken(string anEmail, string aToken) {
            return theAuthenticationRepo.IsValidEmailWithToken(anEmail, HashHelper.HashAuthorityVerificationToken(aToken));
        }

        public void VerifyAuthority(string anEmail, string aToken) {
            theAuthenticationRepo.SetEmailWithTokenToVerified(anEmail, aToken);
        }

        private bool IsValidEmail(string anEmail) {
            if (!ValidationHelper.IsValidEmail(anEmail)) {
                theValidationDictionary.AddError("Email", anEmail, "Email is required.");
            }

            return theValidationDictionary.isValid;
        }

        private void SendTokenEmail(string anEmail, string aToken) {
            string myUrl = HAVConstants.BASE_URL + "/AuthorityAuthentication/ActivateAccount/" + aToken;

            string myTokenLink = "<a href=\"" + myUrl + "\">" + myUrl + "</a>";

            try {
                theEmailService.SendEmail(anEmail, TOKEN_SUBJECT, TOKEN_BODY + myTokenLink);
            } catch (Exception e) {
                throw new EmailException("Couldn't send aEmail.", e);
            }
        }
    }
}