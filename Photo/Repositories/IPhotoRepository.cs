using System.Collections.Generic;
using Social.Generic.Models;

namespace Social.Photo.Repositories {
    public interface IPhotoRepository<T, U> {
        U AddReferenceToImage(T aUser, int anAlbumId, string anImageName, bool aProfilePicture);
        void DeletePhoto(int aPhotoId);
        AbstractPhotoModel<U> GetAbstractPhoto(int aPhotoId);
        IEnumerable<U> GetPhotos(int aUserId, int anAlbumId);
        AbstractPhotoModel<U> GetAbstractProfilePicture(int aUserId);
        void SetPhotoAsAlbumCover(int aPhotoId);
    }
}