﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.UserFeatures {
    public interface IHAVPhotoRepository {
        Photo AddReferenceToImage(User aUser, int anAlbumId, string anImageName);
        void SetToProfilePicture(User aUser, int aPhotoId);
        Photo GetProfilePicture(int aUserId);
        IEnumerable<Photo> GetPhotos(int aUserId, int anAlbumId);
        Photo GetPhoto(int aPhotoId);
        void DeletePhoto(int aPhotoId);
    }
}