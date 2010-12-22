using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.UserFeatures {
    public class EntityHAVUserPictureRepository : HAVBaseRepository, IHAVUserPictureRepository {

        public void AddProfilePicture(User aUser, string anImageURL) {
            UserPicture myUserPicture = UserPicture.CreateUserPicture(0, anImageURL, true, DateTime.UtcNow);
            myUserPicture.User = GetUser(aUser.Id);

            UnSetCurrentUserPicture(aUser);

            GetEntities().AddToUserPictures(myUserPicture);
            GetEntities().SaveChanges();
        }

        public void SetToProfilePicture(User aUser, int aUserPictureId) {
            UserPicture newProfilePicture = GetUserPicture(aUserPictureId);

            if (newProfilePicture == null) {
                return;
            }

            newProfilePicture.ProfilePicture = true;
            UnSetCurrentUserPicture(aUser);

            GetEntities().ApplyCurrentValues(newProfilePicture.EntityKey.EntitySetName, newProfilePicture);

            GetEntities().SaveChanges();
        }

        private void UnSetCurrentUserPicture(User aUser) {
            UserPicture currentProfilePicture = GetProfilePicture(aUser.Id);
            if (currentProfilePicture != null) {
                currentProfilePicture.ProfilePicture = false;
                GetEntities().ApplyCurrentValues(currentProfilePicture.EntityKey.EntitySetName, currentProfilePicture);
            }
        }

        public UserPicture GetProfilePicture(int aUserId) {
            return (from up in GetEntities().UserPictures
                    where up.User.Id == aUserId
                    && up.ProfilePicture == true
                    select up).FirstOrDefault();
        }

        public UserPicture GetUserPicture(int aUserPictureId) {
            return (from up in GetEntities().UserPictures
                    where up.Id == aUserPictureId
                    select up).FirstOrDefault();
        }

        public IEnumerable<UserPicture> GetUserPictures(int aUserId) {
            return (from up in GetEntities().UserPictures
                    where up.User.Id == aUserId
                    && up.ProfilePicture == false
                    orderby up.DateTimeStamp descending
                    select up).ToList<UserPicture>();
        }

        public void DeleteUserPicture(int aUserPictureId) {
            UserPicture userPicture = GetUserPicture(aUserPictureId);
            GetEntities().DeleteObject(userPicture);
            GetEntities().SaveChanges();
        }

        private User GetUser(int anId) {
            IHAVUserRetrievalRepository myUserRetrieval = new EntityHAVUserRetrievalRepository();
            return myUserRetrieval.GetUser(anId);
        }
    }
}