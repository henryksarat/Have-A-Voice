using System;
using System.Collections.Generic;
using System.Linq;
using Social.Admin.Exceptions;
using Social.Admin.Helpers;
using Social.Generic.Constants;
using Social.Generic.Helpers;
using Social.Generic.Models;
using Social.Validation;
using UniversityOfMe.Helpers;
using UniversityOfMe.Models;
using UniversityOfMe.Models.View;
using UniversityOfMe.Repositories.Events;
using UniversityOfMe.Repositories.Status;

namespace UniversityOfMe.Services.Status {
    public class UserStatusService : IUserStatusService {
        private IValidationDictionary theValidationDictionary;
        private IUserStatusRepository theUserStatusRepository;

        public UserStatusService(IValidationDictionary aValidationDictionary)
            : this(aValidationDictionary, new EntityUserStatusRepository()) { }

        public UserStatusService(IValidationDictionary aValidationDictionary, IUserStatusRepository aUserStatusRepo) {
            theUserStatusRepository = aUserStatusRepo;
            theValidationDictionary = aValidationDictionary;
        }

        public bool CreateUserStatus(UserInformationModel<User> aUserInfo, string aStatus, bool anEveryone) {
            if (!ValidStatus(aStatus)) {
                return false;
            }

            University myUniversity = UniversityHelper.GetMainUniversity(aUserInfo.Details);

            theUserStatusRepository.CreateUserStatus(aUserInfo.Details, myUniversity, aStatus, anEveryone);

            return true;
        }

        public void DeleteUserStatus(UserInformationModel<User> aUserInfo, int aStatusId) {
            UserStatus myUserStatus = theUserStatusRepository.GetUserStatus(aStatusId);
            if (aUserInfo.UserId == myUserStatus.UserId) {
                theUserStatusRepository.DeleteUserStatus(aStatusId);
            } else {
                throw new PermissionDenied(ErrorKeys.PERMISSION_DENIED);
            }
        }

        public UserStatus GetLatestUserStatusForUser(UserInformationModel<User> aUserInfo) {
            return theUserStatusRepository.GetLatestUserStatusForUser(aUserInfo.Details);
        }

        public IEnumerable<UserStatus> GetLatestUserStatusesWithinUniversity(UserInformationModel<User> aUser,string aUniversityId, int aLimit) {
            return theUserStatusRepository
                .GetLatestUserStatuses(aUniversityId)
                .Where(s => FriendHelper.IsFriend(aUser.Details, s.User) || s.Everyone)
                .Take(aLimit); ;
        }

        private bool ValidStatus(string aStatus) {
            if (string.IsNullOrEmpty(aStatus)) {
                theValidationDictionary.AddError("UserStatus", aStatus.Trim(), "A status is required.");
            }

            return theValidationDictionary.isValid;
        }
    }
}
