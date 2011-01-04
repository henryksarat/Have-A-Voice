using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.UserFeatures {
    public interface IHAVUserPictureRepository {
        UserPicture AddReferenceToImage(User aUser, string anImageName);
        void SetToProfilePicture(User aUser, int aUserPictureId);
        UserPicture GetProfilePicture(int aUserId);
        IEnumerable<UserPicture> GetUserPictures(int aUserId);
        UserPicture GetUserPicture(int userPictureId);
        void DeleteUserPicture(int aUserPictureId);
    }
}