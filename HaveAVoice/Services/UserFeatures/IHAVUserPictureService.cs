using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Services.UserFeatures {
    public interface IHAVUserPictureService {
        UserPicture GetProfilePicture(int aUserId);
        IEnumerable<UserPicture> GetUserPictures(User aViewingUser, int aUserId);
        void SetToProfilePicture(User aUser, int aUserPictureId);
        void DeleteUserPictures(List<int> aUserPictureIds);
        UserPicture GetUserPicture(int aUserPictureId);
        bool IsValidImage(string anImageFile);
        void UploadProfilePicture(User aUserToUploadFor, HttpPostedFileBase anImage);
        void UploadImageWithDatabaseReference(User aUserToUploadFor, HttpPostedFileBase aImageFile);
    }
}