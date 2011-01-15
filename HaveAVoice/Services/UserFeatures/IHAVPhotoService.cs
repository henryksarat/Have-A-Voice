﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Services.UserFeatures {
    public interface IHAVPhotoService {
        Photo GetProfilePicture(int aUserId);
        IEnumerable<Photo> GetPhotos(User aViewingUser, int anAlbumId, int aUserId);
        void SetToProfilePicture(User aUser, int aPhotoId);
        void DeletePhoto(User aUserDeleting, int aPhotoId);
        Photo GetPhoto(User aViewingUser, int aPhotoId);
        bool IsValidImage(string anImageFile);
        void UploadProfilePicture(User aUserToUploadFor, HttpPostedFileBase anImage);
        void UploadImageWithDatabaseReference(User aUserToUploadFor, int anAlbumId, HttpPostedFileBase aImageFile);
    }
}