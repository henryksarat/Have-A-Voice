using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.UserFeatures {
    public class EntityHAVUserPictureRepository : HAVBaseRepository, IHAVUserPictureRepository {
        private HaveAVoiceEntities theEntities = new HaveAVoiceEntities();

        public UserPicture AddReferenceToImage(User aUser, string anImageName) {
            UserPicture myUserPicture = UserPicture.CreateUserPicture(0, aUser.Id, anImageName, false, DateTime.UtcNow);
            
            theEntities.AddToUserPictures(myUserPicture);
            theEntities.SaveChanges();

            return myUserPicture;
        }

        public void SetToProfilePicture(User aUser, int aUserPictureId) {
            UserPicture newProfilePicture = GetUserPicture(aUserPictureId);

            if (newProfilePicture == null) {
                return;
            }

            newProfilePicture.ProfilePicture = true;
            UnSetCurrentUserPicture(aUser);

            theEntities.ApplyCurrentValues(newProfilePicture.EntityKey.EntitySetName, newProfilePicture);

            theEntities.SaveChanges();
        }

        private void UnSetCurrentUserPicture(User aUser) {
            UserPicture currentProfilePicture = GetProfilePicture(aUser.Id);
            if (currentProfilePicture != null) {
                currentProfilePicture.ProfilePicture = false;
                theEntities.ApplyCurrentValues(currentProfilePicture.EntityKey.EntitySetName, currentProfilePicture);
            }
        }

        public UserPicture GetProfilePicture(int aUserId) {
            return (from up in theEntities.UserPictures
                    where up.User.Id == aUserId
                    && up.ProfilePicture == true
                    select up).FirstOrDefault();
        }

        public UserPicture GetUserPicture(int aUserPictureId) {
            return (from up in theEntities.UserPictures
                    where up.Id == aUserPictureId
                    select up).FirstOrDefault();
        }

        public IEnumerable<UserPicture> GetUserPictures(int aUserId) {
            return (from up in theEntities.UserPictures
                    where up.User.Id == aUserId
                    && up.ProfilePicture == false
                    orderby up.DateTimeStamp descending
                    select up).ToList<UserPicture>();
        }

        public void DeleteUserPicture(int aUserPictureId) {
            UserPicture userPicture = GetUserPicture(aUserPictureId);
            theEntities.DeleteObject(userPicture);
            theEntities.SaveChanges();
        }
    }
}