using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.UserFeatures {
    public interface IHAVPhotoAlbumRepository {
        PhotoAlbum GetProfilePictureAlbumForUser(User aUser);
        IEnumerable<PhotoAlbum> GetPhotoAlbumsForUser(int aUserIdOfAlbum);
        PhotoAlbum GetPhotoAlbum(int anAlbumId);

        PhotoAlbum Create(User aUser, string aName, string aDescription);
        void Edit(int anAlbumId, string aName, string aDescription);
        void Delete(int anAlbumId);
    }
}