using System;
using System.Collections.Generic;
using System.Linq;
using UniversityOfMe.Models;
using UniversityOfMe.Repositories.Helpers;
using UniversityOfMe.Helpers.Badges;

namespace UniversityOfMe.Repositories.Status {
    public class EntityUserStatusRepository : IUserStatusRepository {
        private UniversityOfMeEntities theEntities = new UniversityOfMeEntities();

        public void CreateUserStatus(User aUser, University aCurrentUniversity, string aStatus) {
            UserStatus myUserStatus = UserStatus.CreateUserStatus(0, aUser.Id, aCurrentUniversity.Id, aStatus, DateTime.UtcNow, false);
            theEntities.AddToUserStatuses(myUserStatus);
            theEntities.SaveChanges();
        }

        public UserStatus GetUserStatus(int aStatusId) {
            return (from us in theEntities.UserStatuses
                    where us.Id == aStatusId
                    select us).FirstOrDefault<UserStatus>();
        }

        public void DeleteUserStatus(int aStatusId) {
            UserStatus myUserStatus = GetUserStatus(aStatusId);
            theEntities.DeleteObject(myUserStatus);
            theEntities.SaveChanges();
        }

        public UserStatus GetLatestUserStatusForUser(User aUser) {
            return (from us in theEntities.UserStatuses
                    where us.UserId == aUser.Id
                    select us).OrderByDescending(us2 => us2.DateTimeStamp).FirstOrDefault<UserStatus>();
        }

        public IEnumerable<UserStatus> GetLatestUserStatuses(string aUniversityId, int aLimit) {
            return (from us in theEntities.UserStatuses
                    where us.UniversityId == aUniversityId
                    select us).OrderByDescending(us2 => us2.DateTimeStamp).Take<UserStatus>(aLimit);
        }
    }
}