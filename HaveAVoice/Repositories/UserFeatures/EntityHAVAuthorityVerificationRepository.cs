using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;
using HaveAVoice.Repositories.AdminFeatures;

namespace HaveAVoice.Repositories.UserFeatures {
    public class EntityHAVAuthorityVerificationRepository : IHAVAuthorityVerificationRepository {
        private HaveAVoiceEntities theEntities = new HaveAVoiceEntities();

        public bool IsValidEmailWithToken(string anEmail, string aToken, string anAuthorityType) {
            return GetAuthorityVerification(anEmail, aToken, anAuthorityType) != null ? true : false; 
        }

        public bool TokenForEmailExists(string anEmail, string anAuthorityType) {
            return GetAuthorityVerification(anEmail, anAuthorityType) != null ? true : false; 
        }

        public void CreateTokenForEmail(User aCreatingUser, string anEmail, string aToken, string anAuthorityType) {
            Role myRole = GetRoleByName(anAuthorityType);
            AuthorityVerification myVerification = AuthorityVerification.CreateAuthorityVerification(anEmail, aCreatingUser.Id, myRole.Id, aToken, false, DateTime.UtcNow);
            theEntities.AddToAuthorityVerifications(myVerification);
            theEntities.SaveChanges();
        }

        public void SetEmailWithTokenToVerified(string anEmail, string aToken, string anAuthorityType) {
            AuthorityVerification myVerification = GetAuthorityVerification(anEmail, aToken, anAuthorityType);
            myVerification.Verified = true;

            theEntities.ApplyCurrentValues(myVerification.EntityKey.EntitySetName, myVerification);
            theEntities.SaveChanges();
        }

        public void UpdateTokenForEmail(string anEmail, string aToken, string anAuthorityType) {
            AuthorityVerification myVerification = GetAuthorityVerification(anEmail, anAuthorityType);
            myVerification.Token = aToken;

            theEntities.ApplyCurrentValues(myVerification.EntityKey.EntitySetName, myVerification);
            theEntities.SaveChanges();
        }

        private AuthorityVerification GetAuthorityVerification(string anEmail, string aToken, string anAuthorityType) {
            Role myRole = GetRoleByName(anAuthorityType);
            return (from a in theEntities.AuthorityVerifications
                    where a.Email == anEmail
                    && a.Token == aToken
                    && a.RoleId == myRole.Id
                    select a).FirstOrDefault<AuthorityVerification>();
        }

        private AuthorityVerification GetAuthorityVerification(string anEmail, string anAuthorityType) {
            Role myRole = GetRoleByName(anAuthorityType);
            return (from a in theEntities.AuthorityVerifications
                    where a.Email == anEmail
                    && a.RoleId == myRole.Id
                    select a).FirstOrDefault<AuthorityVerification>();
        }

        private Role GetRoleByName(string aName) {
            IHAVRoleRepository myRoleRepo = new EntityHAVRoleRepository();
            return myRoleRepo.GetRoleByName(aName);
        }
    }
}