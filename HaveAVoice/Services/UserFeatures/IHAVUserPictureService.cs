using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Services.UserFeatures {
    public interface IHAVUserPictureService {
        void AddProfilePicture(User aUser, string anImageURL);
        UserPicture GetProfilePicture(int aUserId);
        string GetProfilePictureURL(User aUser);
        IEnumerable<UserPicture> GetUserPictures(User aViewingUser, int aUserId);
        void SetToProfilePicture(User aUser, int aUserPictureId);
        void DeleteUserPictures(List<int> aUserPictureIds);
        UserPicture GetUserPicture(int aUserPictureId);
    }
}