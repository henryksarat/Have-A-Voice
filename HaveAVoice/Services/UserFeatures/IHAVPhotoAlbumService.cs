﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;
using HaveAVoice.Models.View;

namespace HaveAVoice.Services.UserFeatures {
    public interface IHAVPhotoAlbumService {
        bool CreatePhotoAlbum(User aUser, string aName, string aDescription);
        IEnumerable<PhotoAlbum> GetPhotoAlbumsForUser(User aRequestingUser, int aUserIdOfAlbum);
        PhotoAlbum GetPhotoAlbum(UserInformationModel aUserModel, int anAlbumId);
        PhotoAlbum GetPhotoAlbumForEdit(UserInformationModel aUserModel, int anAlbumId);
        bool EditPhotoAlbum(UserInformationModel aUserEditingModel, int anAlbumId, string aName, string aDescription);
        void DeletePhotoAlbum(UserInformationModel aUserDeletingModel, int anAlbumId);
        PhotoAlbum GetProfilePictureAlbumForUser(User aUsersPhotoAlbum);
    }
}