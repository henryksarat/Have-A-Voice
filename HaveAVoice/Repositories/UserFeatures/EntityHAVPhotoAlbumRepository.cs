using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.UserFeatures {
    public class EntityHAVPhotoAlbumRepository : HAVBaseRepository, IHAVPhotoAlbumRepository {
        private const string PROFILE_PICTURE_ALBUM = "Profile Pictures";

        private HaveAVoiceEntities theEntities = new HaveAVoiceEntities();

        public PhotoAlbum GetProfilePictureAlbumForUser(User aUser) {
            PhotoAlbum myAlbum = GetPhotoAlbumForUserandAlbumName(aUser, PROFILE_PICTURE_ALBUM);
            if (myAlbum == null) {
                myAlbum = Create(aUser, PROFILE_PICTURE_ALBUM, string.Empty);
            }

            return myAlbum;
        }

        public PhotoAlbum GetPhotoAlbum(int anAlbumId) {
            return (from p in theEntities.PhotoAlbums
                    where p.Id == anAlbumId
                    select p).FirstOrDefault<PhotoAlbum>();
        }

        public IEnumerable<PhotoAlbum> GetPhotoAlbumsForUser(int aUserIdOfAlbum) {
            return (from p in theEntities.PhotoAlbums
                    where p.CreatedByUserId == aUserIdOfAlbum
                    select p).ToList<PhotoAlbum>();
        }

        public PhotoAlbum Create(User aUser, string aName, string aDescription) {
            PhotoAlbum myAlbum = PhotoAlbum.CreatePhotoAlbum(0, aName, aUser.Id);
            myAlbum.Description = aDescription;

            theEntities.AddToPhotoAlbums(myAlbum);
            theEntities.SaveChanges();

            return myAlbum;
        }

        public void Edit(int anAlbumId, string aName, string aDescription) {
            PhotoAlbum myAlbum = GetPhotoAlbum(anAlbumId);

            myAlbum.Name = aName;
            myAlbum.Description = aDescription;

            theEntities.ApplyCurrentValues(myAlbum.EntityKey.EntitySetName, myAlbum);
            theEntities.SaveChanges();
        }

        public void Delete(int anAlbumId) {
            theEntities = new HaveAVoiceEntities();
            PhotoAlbum myAlbum = GetPhotoAlbum(anAlbumId);

            theEntities.DeleteObject(myAlbum);
            theEntities.SaveChanges();
        }

        private PhotoAlbum GetPhotoAlbumForUserandAlbumName(User aUser, string aPhotoAlbumnName) {
            return (from p in theEntities.PhotoAlbums
                    where p.CreatedByUserId == aUser.Id
                    && p.Name == aPhotoAlbumnName
                    select p).FirstOrDefault<PhotoAlbum>();
        }
    }
}