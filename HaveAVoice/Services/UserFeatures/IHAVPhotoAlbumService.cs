using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;
using HaveAVoice.Models.View;

namespace HaveAVoice.Services.UserFeatures {
    public interface IHAVPhotoAlbumService {
        bool CreatePhotoAlbum(User aUser, string aName, string aDescription);
        IEnumerable<PhotoAlbum> GetPhotoAlbumsForUser(User aRequestingUser, int aUserIdOfAlbum);
        PhotoAlbum GetPhotoAlbum(UserInformationModel aUserModel, int aSourceUserId, int anAlbumId);
    }
}