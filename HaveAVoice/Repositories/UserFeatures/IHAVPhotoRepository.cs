using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.UserFeatures {
    public interface IHAVPhotoRepository {
        IEnumerable<PhotoAlbum> GetPhotoAlbumsForUser(int aUserIdOfAlbum);
        PhotoAlbum CreatePhotoAlbum(User aUser, string aName, string aDescription);
        PhotoAlbum GetPhotoAlbum(int aPhotoAlbumOfUserId, int anAlbumId);
        PhotoAlbum GetProfilePictureAlbumForUser(User aUser);
        Photo AddReferenceToImage(User aUser, int anAlbumId, string anImageName);
        void SetToProfilePicture(User aUser, int aPhotoId);
        Photo GetProfilePicture(int aUserId);
        IEnumerable<Photo> GetPhotos(int aUserId, int anAlbumId);
        Photo GetPhoto(int aPhotoId);
        void DeletePhoto(int aPhotoId);
    }
}