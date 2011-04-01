using System.Collections.Generic;
using System.Web;
using Social.Generic.Models;

namespace Social.Photo.Services {
    //T = User
    //U = PhotoAlbum
    //V = Photo
    //W = Friend
    public interface IPhotoService<T, U, V, W> {
        V GetProfilePicture(int aUserId);
        IEnumerable<V> GetPhotos(AbstractUserModel<T> aViewingUser, int anAlbumId, int aUserId);
        void SetToProfilePicture(AbstractUserModel<T> aUser, int aPhotoId);
        void DeletePhoto(AbstractUserModel<T> aUserDeleting, int aPhotoId);
        AbstractPhotoModel<V> GetPhoto(AbstractUserModel<T> aViewingUser, int aPhotoId);
        void UploadProfilePicture(AbstractUserModel<T> aUserToUploadFor, HttpPostedFileBase anImage);
        void UploadImageWithDatabaseReference(AbstractUserModel<T> aUserToUploadFor, int anAlbumId, HttpPostedFileBase aImageFile);
        void SetPhotoAsAlbumCover(AbstractUserModel<T> myEditingUser, int aPhotoId);
    }
}