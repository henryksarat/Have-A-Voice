using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.UserFeatures {
    public class EntityHAVAuthorityVerificationRepository : IHAVAuthorityVerificationRepository {
        private HaveAVoiceEntities theEntities = new HaveAVoiceEntities();

        public bool IsValidEmailWithToken(string anEmail, string aToken) {
            return GetAuthorityVerification(anEmail, aToken) != null ? true : false; 
        }

        public bool TokenForEmailExists(string anEmail) {
            return GetAuthorityVerification(anEmail) != null ? true : false; 
        }

        public void CreateTokenForEmail(User aCreatingUser, string anEmail, string aToken) {
            AuthorityVerification myVerification = AuthorityVerification.CreateAuthorityVerification(anEmail, aCreatingUser.Id, aToken, false, DateTime.UtcNow);
            theEntities.AddToAuthorityVerifications(myVerification);
            theEntities.SaveChanges();
        }

        public void SetEmailWithTokenToVerified(string anEmail, string aToken) {
            AuthorityVerification myVerification = GetAuthorityVerification(anEmail, aToken);
            myVerification.Verified = true;

            theEntities.ApplyCurrentValues(myVerification.EntityKey.EntitySetName, myVerification);
            theEntities.SaveChanges();
        }

        public void UpdateTokenForEmail(string anEmail, string aToken) {
            AuthorityVerification myVerification = GetAuthorityVerification(anEmail);
            myVerification.Token = aToken;

            theEntities.ApplyCurrentValues(myVerification.EntityKey.EntitySetName, myVerification);
            theEntities.SaveChanges();
        }

        private AuthorityVerification GetAuthorityVerification(string anEmail, string aToken) {
            return (from a in theEntities.AuthorityVerifications
                    where a.Email == anEmail
                    && a.Token == aToken
                    select a).FirstOrDefault<AuthorityVerification>();
        }

        private AuthorityVerification GetAuthorityVerification(string anEmail) {
            return (from a in theEntities.AuthorityVerifications
                    where a.Email == anEmail
                    select a).FirstOrDefault<AuthorityVerification>();
        }
    }
}