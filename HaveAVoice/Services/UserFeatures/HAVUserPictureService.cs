using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Repositories;
using HaveAVoice.Repositories.UserFeatures;
using HaveAVoice.Models;
using HaveAVoice.Helpers;
using HaveAVoice.Exceptions;

namespace HaveAVoice.Services.UserFeatures {
    public class HAVUserPictureService : HAVBaseService, IHAVUserPictureService {
        private IHAVFanService theFanService;
        private IHAVUserPictureRepository theUserPictureRepo;
 
        public HAVUserPictureService()
            : this(new HAVFanService(), new EntityHAVUserPictureRepository(), new HAVBaseRepository()) { }

        public HAVUserPictureService(IHAVFanService aFanService, IHAVUserPictureRepository aUserPictureRepo, IHAVBaseRepository aBaseRepository) : base(aBaseRepository) {
            theFanService = aFanService;
            theUserPictureRepo = aUserPictureRepo;
        }

        public void AddProfilePicture(User aUser, string anImageURL) {
            theUserPictureRepo.AddProfilePicture(aUser, anImageURL);
        }

        public IEnumerable<UserPicture> GetUserPictures(User aViewingUser, int aUserId) {
            if (aViewingUser.Id == aUserId || theFanService.IsFan(aUserId, aViewingUser)) {
                return theUserPictureRepo.GetUserPictures(aUserId);
            }

            throw new NotFanException();
        }

        public UserPicture GetProfilePicture(int aUserId) {
            return theUserPictureRepo.GetProfilePicture(aUserId);
        }

        public void DeleteUserPictures(List<int> aUserPictureIds) {
            foreach (int userPictureId in aUserPictureIds) {
                theUserPictureRepo.DeleteUserPicture(userPictureId);
            }
        }

        public UserPicture GetUserPicture(int aUserPictureId) {
            return theUserPictureRepo.GetUserPicture(aUserPictureId);
        }

        public void SetToProfilePicture(User aUser, int aUserPictureId) {
            theUserPictureRepo.SetToProfilePicture(aUser, aUserPictureId);
        }
    }
}