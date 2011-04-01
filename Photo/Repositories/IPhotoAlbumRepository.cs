using System.Collections.Generic;
using Social.Generic.Models;

namespace Social.Photo.Repositories {
    public interface IPhotoAlbumRepository<T, U, V> {
        U Create(T aUser, string aName, string aDescription);
        void Delete(int anAlbumId);
        void Edit(int anAlbumId, string aName, string aDescription);
        AbstractPhotoAlbumModel<U, V> GetAbstractPhotoAlbum(int anAlbumId);
        IEnumerable<U> GetPhotoAlbumsForUser(int aUserIdOfAlbum);
        AbstractPhotoAlbumModel<U, V> GetAbstractProfilePictureAlbumForUser(T aUser);        
    }
}