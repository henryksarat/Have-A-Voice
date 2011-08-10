﻿using System;
using System.Collections.Generic;
using System.Linq;
using HaveAVoice.Models;
using HaveAVoice.Repositories.AdminFeatures;
using Social.Admin.Repositories;
using HaveAVoice.Helpers.Authority;
using Social.Generic;

namespace HaveAVoice.Repositories.UserFeatures {
    public class EntityHAVAuthorityVerificationRepository : IHAVAuthorityVerificationRepository {
        private HaveAVoiceEntities theEntities = new HaveAVoiceEntities();

        public void AddZipCodesForUser(User anAdminUser, string anEmail, IEnumerable<int> aZipCodes) {
            User myAuthority = GetUser(anEmail);

            foreach(int myZipCode in aZipCodes) {
                AuthorityViewableZipCode myAuthorityZipCode = AuthorityViewableZipCode.CreateAuthorityViewableZipCode(0, myAuthority.Id, myZipCode, anAdminUser.Id, DateTime.UtcNow);
                theEntities.AddToAuthorityViewableZipCodes(myAuthorityZipCode);
            }

            theEntities.SaveChanges();
        }

        public IEnumerable<AuthorityViewableZipCode> GetAuthorityViewableZipCodes(string anEmail) {
            return (from z in theEntities.AuthorityViewableZipCodes
                    where z.AuthorityUser.Email == anEmail
                    select z);
        }

        public IEnumerable<RegionSpecific> GetClashingRegions(string aCity, string aState, int aZip) {
            return (from r in theEntities.RegionSpecifics
                    where r.City == aCity
                    && r.State == aState
                    && r.ZipCode == aZip
                    select r);
        }

        public UserRegionSpecific GetRegionSpecifcInformationForUser(User aUser, UserPosition aUserPosition) {
            return (from urs in theEntities.UserRegionSpecifics
                    join rs in theEntities.RegionSpecifics on urs.RegionSpecificId equals rs.Id
                    where urs.UserId == aUser.Id
                    && rs.UserPositionId == aUserPosition.Position
                    select urs).FirstOrDefault<UserRegionSpecific>();
        }

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
            if (myRole == null) {
                throw new Exception("Role doesn't exist for " + anAuthorityType);
            }
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


        public void SetExtraInfoForAuthority(User aUser, string anExtraInfo) {
            User myUser = GetUser(aUser.Id);
            myUser.UserPositionExtraInfo = anExtraInfo;

            theEntities.ApplyCurrentValues(myUser.EntityKey.EntitySetName, myUser);
            theEntities.SaveChanges();
        }

        public User GetUser(int anId) {
            return (from u in theEntities.Users
                    where u.Id == anId
                    select u).FirstOrDefault<User>();
        }

        public User GetUser(string anEmail) {
            return (from u in theEntities.Users
                    where u.Email == anEmail
                    select u).FirstOrDefault<User>();
        }


        public void UpdateUserRegionSpecifics(User aUser, IEnumerable<Pair<AuthorityPosition, int>> aRegionSpecificsToAddAndUpdate, IEnumerable<Pair<AuthorityPosition, int>> aToRemove) {
            bool myDoSave = false;
            //We look up which UserRegionSpecific it is for the UserPosition(First Value) then we change it to the second value in the Pair
            foreach (Pair<AuthorityPosition, int> myToAdd in aRegionSpecificsToAddAndUpdate) {
                UserRegionSpecific myUserRegionSpecific = GetUserRegionSpecificForUserBasedOnUserPosition(aUser, myToAdd.First.ToString());
                if(myUserRegionSpecific == null) {
                    myUserRegionSpecific = UserRegionSpecific.CreateUserRegionSpecific(0, aUser.Id, myToAdd.Second);
                    theEntities.AddToUserRegionSpecifics(myUserRegionSpecific);
                } else {
                    myUserRegionSpecific.RegionSpecificId = myToAdd.Second;
                    theEntities.ApplyCurrentValues(myUserRegionSpecific.EntityKey.EntitySetName, myUserRegionSpecific);
                }

                myDoSave = true;
            }

            foreach (Pair<AuthorityPosition, int> myToRemove in aToRemove) {
                UserRegionSpecific myUserRegionSpecific = GetUserRegionSpecificForUserBasedOnUserPosition(aUser, myToRemove.First.ToString());
                if (myUserRegionSpecific != null) {
                    theEntities.DeleteObject(myUserRegionSpecific);
                    myDoSave = true;
                }
            }

            if (myDoSave) {
                theEntities.SaveChanges();
            }
        }

        private UserRegionSpecific GetUserRegionSpecificForUserBasedOnUserPosition(User aUser, string aPosition) {
            return (from u in theEntities.UserRegionSpecifics
                    join rs in theEntities.RegionSpecifics on u.RegionSpecificId equals rs.Id
                    where u.UserId == aUser.Id
                    && rs.UserPositionId == aPosition
                    select u).FirstOrDefault<UserRegionSpecific>();
        }

    }
}