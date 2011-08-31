using System.Collections.Generic;
using System.Web;
using Social.Generic.Models;
using Amazon.S3;

namespace Social.Photo.Services {
    //T = User
    //U = PhotoAlbum
    //V = Photo
    //W = Friend
    public interface IPhotoService<T, U, V, W> {
        V GetProfilePicture(int aUserId);
        IEnumerable<V> GetPhotos(AbstractUserModel<T> aViewingUser, int anAlbumId, int aUserId);
        void SetToProfilePicture(AbstractUserModel<T> aUser, int aPhotoId);
        void SetToProfilePicture(AbstractUserModel<T> aUser, int aPhotoId, AmazonS3 anAmazonS3Client, string aBucketName, int aMaxSize);
        void DeletePhoto(AbstractUserModel<T> aUserDeleting, int aPhotoId);
        AbstractPhotoModel<V> GetPhoto(AbstractUserModel<T> aViewingUser, int aPhotoId);
        V UploadImageWithDatabaseReference(AbstractUserModel<T> aUserToUploadFor, int anAlbumId, HttpPostedFileBase aImageFile);
        V UploadImageWithDatabaseReference(AbstractUserModel<T> aUserToUploadFor, int anAlbumId, HttpPostedFileBase aImageFile, AmazonS3 anAmazonS3Client, string aBucketName, int aMaxSize);
        void SetPhotoAsAlbumCover(AbstractUserModel<T> myEditingUser, int aPhotoId);
    }
}