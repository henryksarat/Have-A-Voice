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

namespace HaveAVoice.Services.UserFeatures {
    public class HAVAuthorityVerificationService : HAVBaseService, IHAVAuthorityVerificationService {
        private const string TOKEN_SUBJECT = "have a voice | authority authentication";
        private const string TOKEN_BODY = "Hello! <br/ ><br/ > You are classified as an authority and therefore must sign up in a special way. Please follow this link: <br/ >";

        private IHAVAuthorityVerificationRepository theAuthenticationRepo;
        private IHAVEmail theEmailService;

        public HAVAuthorityVerificationService() 
            : this(new HAVBaseRepository(), new HAVEmail(), new EntityHAVAuthorityVerificationRepository()) { }

        public HAVAuthorityVerificationService(IHAVBaseRepository aBaseRepository, IHAVEmail anEmailService, IHAVAuthorityVerificationRepository anAuthorityService) : base(aBaseRepository) {
            theAuthenticationRepo = anAuthorityService;
            theEmailService = anEmailService;
        }

        public void RequestNewTokenForAuthority(User aRequestingUser, string anEmail) {
            Random myRandom = new Random(DateTime.UtcNow.Millisecond);
            string myToken = myRandom.Next().ToString();
            string myHashedToken = HashHelper.HashAuthorityVerificationToken(myToken);

            EmailException myEmailException = null;
            try {
                SendTokenEmail(anEmail, myToken);
            } catch (EmailException e) {
                myEmailException = e;
            }

            theAuthenticationRepo.CreateTokenForEmail(aRequestingUser, anEmail, myHashedToken);
        }

        public bool IsValidToken(string anEmail, string aToken) {
            return theAuthenticationRepo.IsValidEmailWithToken(anEmail, aToken);
        }

        public void VerifyAuthority(string anEmail, string aToken) {
            theAuthenticationRepo.SetEmailWithTokenToVerified(anEmail, aToken);
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