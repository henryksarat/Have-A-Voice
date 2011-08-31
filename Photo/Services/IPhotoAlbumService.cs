using System.Collections.Generic;
using Amazon.S3;
using Social.Generic.Models;

namespace Social.Photo.Services {
    //T = User
    //U = PhotoAlbum
    //V = Photo
    //W = Friend
    public interface IPhotoAlbumService<T, U, V, W> {
        bool CreatePhotoAlbum(T aUser, string aName, string aDescription);
        IEnumerable<U> GetPhotoAlbumsForUser(AbstractUserModel<T> aRequestingUser, int aUserIdOfAlbum);
        U GetPhotoAlbum(AbstractUserModel<T> aUserModel, int anAlbumId);
        U GetPhotoAlbumForEdit(AbstractUserModel<T> aUserModel, int anAlbumId);
        bool EditPhotoAlbum(AbstractUserModel<T> aUserEditingModel, int anAlbumId, string aName, string aDescription);
        void DeletePhotoAlbum(AbstractUserModel<T> aUserDeletingModel, AmazonS3 anAmazonS3Client, string aBucketName, int anAlbumId);
        AbstractPhotoAlbumModel<U, V> GetProfilePictureAlbumForUser(T aUsersPhotoAlbum);
    }
}