using System;
using System.Collections.Generic;
using System.Linq;
using HaveAVoice.Models;
using HaveAVoice.Repositories.AdminFeatures;
using Social.Admin.Repositories;

namespace HaveAVoice.Repositories.UserFeatures {
    public class EntityHAVAuthorityVerificationRepository : IHAVAuthorityVerificationRepository {
        private HaveAVoiceEntities theEntities = new HaveAVoiceEntities();

        public bool IsValidEmailWithToken(string anEmail, string aToken, string anAuthorityType, string anAuthorityPosition) {
            return GetAuthorityVerification(anEmail, aToken, anAuthorityType, anAuthorityPosition) != null ? true : false; 
        }

        public bool TokenForEmailExists(string anEmail, string anAuthorityType, string anAuthorityPosition) {
            return GetAuthorityVerification(anEmail, anAuthorityType, anAuthorityPosition) != null ? true : false; 
        }

        public void CreateTokenForEmail(User aCreatingUser, string anEmail, string aToken, string anAuthorityType, string anAuthorityPosition) {
            Role myRole = GetRoleByName(anAuthorityType);
            AuthorityVerification myVerification = AuthorityVerification.CreateAuthorityVerification(anEmail, aCreatingUser.Id, myRole.Id, aToken, false, DateTime.UtcNow, anAuthorityPosition);
            theEntities.AddToAuthorityVerifications(myVerification);
            theEntities.SaveChanges();
        }

        public void SetEmailWithTokenToVerified(string anEmail, string aToken, string anAuthorityType, string anAuthorityPosition) {
            AuthorityVerification myVerification = GetAuthorityVerification(anEmail, aToken, anAuthorityType, anAuthorityPosition);
            myVerification.Verified = true;

            theEntities.ApplyCurrentValues(myVerification.EntityKey.EntitySetName, myVerification);
            theEntities.SaveChanges();
        }

        public void UpdateTokenForEmail(string anEmail, string aToken, string anAuthorityType, string anAuthorityPosition) {
            AuthorityVerification myVerification = GetAuthorityVerification(anEmail, anAuthorityType, anAuthorityPosition);
            myVerification.Token = aToken;

            theEntities.ApplyCurrentValues(myVerification.EntityKey.EntitySetName, myVerification);
            theEntities.SaveChanges();
        }

        public IEnumerable<UserPosition> GetUserPositions() {
            return (from u in theEntities.UserPositions
                    select u).ToList<UserPosition>();
        }

        private AuthorityVerification GetAuthorityVerification(string anEmail, string aToken, string anAuthorityType, string anAuthorityPosition) {
            Role myRole = GetRoleByName(anAuthorityType);
            return (from a in theEntities.AuthorityVerifications
                    where a.Email == anEmail
                    && a.Token == aToken
                    && a.RoleId == myRole.Id
                    && a.UserPositionId == anAuthorityPosition
                    select a).FirstOrDefault<AuthorityVerification>();
        }

        private AuthorityVerification GetAuthorityVerification(string anEmail, string anAuthorityType, string anAuthorityPosition) {
            Role myRole = GetRoleByName(anAuthorityType);
            return (from a in theEntities.AuthorityVerifications
                    where a.Email == anEmail
                    && a.RoleId == myRole.Id
                    && a.UserPositionId == anAuthorityPosition
                    select a).FirstOrDefault<AuthorityVerification>();
        }

        private Role GetRoleByName(string aName) {
            IRoleRepository<User, Role, RolePermission> myRoleRepo = new EntityHAVRoleRepository();
            return myRoleRepo.GetRoleByName(aName);
        }
    }
}