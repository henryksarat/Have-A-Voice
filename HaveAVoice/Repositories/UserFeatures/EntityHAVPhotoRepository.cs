using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.UserFeatures {
    public class EntityHAVPhotoRepository : HAVBaseRepository, IHAVPhotoRepository {
        private HaveAVoiceEntities theEntities = new HaveAVoiceEntities();
        private const string PROFILE_PICTURE_ALBUM = "Profile Pictures";

        public IEnumerable<PhotoAlbum> GetPhotoAlbumsForUser(int aUserIdOfAlbum) {
            return (from p in theEntities.PhotoAlbums
                    where p.CreatedByUserId == aUserIdOfAlbum
                    select p).ToList<PhotoAlbum>();
        }

        public PhotoAlbum GetProfilePictureAlbumForUser(User aUser) {
            PhotoAlbum myAlbum = GetPhotoAlbumForUserandAlbumName(aUser, PROFILE_PICTURE_ALBUM);
            if (myAlbum == null) {
                myAlbum = CreatePhotoAlbum(aUser, PROFILE_PICTURE_ALBUM, string.Empty);
            }

            return myAlbum;
        }

        public PhotoAlbum CreatePhotoAlbum(User aUser, string aName, string aDescription) {
            PhotoAlbum myAlbum = PhotoAlbum.CreatePhotoAlbum(0, aName, aUser.Id);
            myAlbum.Description = aDescription;

            theEntities.AddToPhotoAlbums(myAlbum);
            theEntities.SaveChanges();

            return myAlbum;
        }

        public Photo AddReferenceToImage(User aUser, int anAlbumId, string anImageName) {
            Photo myPhoto = Photo.CreatePhoto(0, aUser.Id, anAlbumId, anImageName, false, DateTime.UtcNow, 0);

            theEntities.AddToPhotos(myPhoto);
            theEntities.SaveChanges();

            return myPhoto;
        }

        public void SetToProfilePicture(User aUser, int aPhotoId) {
            Photo newProfilePicture = GetPhoto(aPhotoId);

            if (newProfilePicture == null) {
                return;
            }

            newProfilePicture.ProfilePicture = true;
            UnSetCurrentPhoto(aUser);

            theEntities.ApplyCurrentValues(newProfilePicture.EntityKey.EntitySetName, newProfilePicture);

            theEntities.SaveChanges();
        }

        public Photo GetProfilePicture(int aUserId) {
            return (from up in theEntities.Photos
                    where up.User.Id == aUserId
                    && up.ProfilePicture == true
                    select up).FirstOrDefault();
        }

        public Photo GetPhoto(int aPhotoId) {
            return (from up in theEntities.Photos
                    where up.Id == aPhotoId
                    select up).FirstOrDefault();
        }

        public IEnumerable<Photo> GetPhotos(int aUserId, int anAlbumId) {
            return (from p in theEntities.Photos
                    where p.User.Id == aUserId
                    && p.ProfilePicture == false
                    && p.PhotoAlbumId == anAlbumId
                    orderby p.DateTimeStamp descending
                    select p).ToList<Photo>();
        }

        public void DeletePhoto(int aPhotoId) {
            Photo myPhoto = GetPhoto(aPhotoId);
            theEntities.DeleteObject(myPhoto);
            theEntities.SaveChanges();
        }

        private void UnSetCurrentPhoto(User aUser) {
            Photo currentProfilePicture = GetProfilePicture(aUser.Id);
            if (currentProfilePicture != null) {
                currentProfilePicture.ProfilePicture = false;
                theEntities.ApplyCurrentValues(currentProfilePicture.EntityKey.EntitySetName, currentProfilePicture);
            }
        }

        private PhotoAlbum GetPhotoAlbumForUserandAlbumName(User aUser, string aPhotoAlbumnName) {
            return (from p in theEntities.PhotoAlbums
                    where p.CreatedByUserId == aUser.Id
                    && p.Name == aPhotoAlbumnName
                    select p).FirstOrDefault<PhotoAlbum>();
        }

        public PhotoAlbum GetPhotoAlbum(int aPhotoAlbumOfUserId, int anAlbumId) {
            return (from p in theEntities.PhotoAlbums
                    where p.CreatedByUserId == aPhotoAlbumOfUserId
                    && p.Id == anAlbumId
                    select p).FirstOrDefault<PhotoAlbum>();
        }
    }
}