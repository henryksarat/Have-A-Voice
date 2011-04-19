using System.Collections.Generic;
using System.Linq;
using Social.Generic.Models;
using Social.Photo.Repositories;
using UniversityOfMe.Models;
using UniversityOfMe.Models.Social;

namespace UniversityOfMe.Repositories.Photos {
    public class EntityPhotoAlbumRepository : IPhotoAlbumRepository<User, PhotoAlbum, Photo> {
        private const string PROFILE_PICTURE_ALBUM = "Profile Pictures";

        private UniversityOfMeEntities theEntities = new UniversityOfMeEntities();

        public PhotoAlbum Create(User aUser, string aName, string aDescription) {
            PhotoAlbum myAlbum = PhotoAlbum.CreatePhotoAlbum(0, aName, aUser.Id);
            myAlbum.Description = aDescription;

            theEntities.AddToPhotoAlbums(myAlbum);
            theEntities.SaveChanges();

            return myAlbum;
        }

        public void Delete(int anAlbumId) {
            theEntities = new UniversityOfMeEntities();
            PhotoAlbum myAlbum = GetPhotoAlbum(anAlbumId);

            theEntities.DeleteObject(myAlbum);
            theEntities.SaveChanges();
        }

        public void Edit(int anAlbumId, string aName, string aDescription) {
            PhotoAlbum myAlbum = GetPhotoAlbum(anAlbumId);

            myAlbum.Name = aName;
            myAlbum.Description = aDescription;

            theEntities.ApplyCurrentValues(myAlbum.EntityKey.EntitySetName, myAlbum);
            theEntities.SaveChanges();
        }

        public AbstractPhotoAlbumModel<PhotoAlbum, Photo> GetAbstractPhotoAlbum(int anAlbumId) {
            return SocialPhotoAlbumModel.Create(GetPhotoAlbum(anAlbumId));
        }

        public IEnumerable<PhotoAlbum> GetPhotoAlbumsForUser(int aUserIdOfAlbum) {
            return (from p in theEntities.PhotoAlbums
                    where p.CreatedByUserId == aUserIdOfAlbum
                    select p).ToList<PhotoAlbum>();
        }

        public AbstractPhotoAlbumModel<PhotoAlbum, Photo> GetAbstractProfilePictureAlbumForUser(User aUser) {
            PhotoAlbum myAlbum = GetPhotoAlbumForUserandAlbumName(aUser, PROFILE_PICTURE_ALBUM);
            if (myAlbum == null) {
                myAlbum = Create(aUser, PROFILE_PICTURE_ALBUM, string.Empty);
            }

            return SocialPhotoAlbumModel.Create(myAlbum);
        }

        private PhotoAlbum GetPhotoAlbumForUserandAlbumName(User aUser, string aPhotoAlbumnName) {
            return (from p in theEntities.PhotoAlbums
                    where p.CreatedByUserId == aUser.Id
                    && p.Name == aPhotoAlbumnName
                    select p).FirstOrDefault<PhotoAlbum>();
        }

        private PhotoAlbum GetPhotoAlbum(int anAlbumId) {
            return (from p in theEntities.PhotoAlbums
                    where p.Id == anAlbumId
                    select p).FirstOrDefault<PhotoAlbum>();
        }
    }
}