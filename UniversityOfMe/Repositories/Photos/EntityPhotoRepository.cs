using System;
using System.Collections.Generic;
using System.Linq;
using Social.Generic.Models;
using Social.Photo.Repositories;
using UniversityOfMe.Models;
using UniversityOfMe.Models.Social;

namespace UniversityOfMe.Repositories.Photos {
    public class EntityPhotoRepository : IPhotoRepository<User, Photo> {
        private UniversityOfMeEntities theEntities = new UniversityOfMeEntities();

        public Photo AddReferenceToImage(User aUser, int anAlbumId, string anImageName, bool aProfilePicture) {
            Photo myPhoto = Photo.CreatePhoto(0, aUser.Id, anAlbumId, anImageName, aProfilePicture, DateTime.UtcNow, false);

            theEntities.AddToPhotos(myPhoto);
            theEntities.SaveChanges();

            return myPhoto;
        }

        public void DeletePhoto(int aPhotoId) {
            Photo myPhoto = GetPhoto(aPhotoId);
            theEntities.DeleteObject(myPhoto);
            theEntities.SaveChanges();
        }

        public AbstractPhotoModel<Photo> GetAbstractPhoto(int aPhotoId) {
            return SocialPhotoModel.Create(GetPhoto(aPhotoId));
        }

        public IEnumerable<Photo> GetPhotos(int aUserId, int anAlbumId) {
            return (from p in theEntities.Photos
                    where p.User.Id == aUserId
                    && p.ProfilePicture == false
                    && p.PhotoAlbumId == anAlbumId
                    orderby p.DateTimeStamp descending
                    select p).ToList<Photo>();
        }

        public AbstractPhotoModel<Photo> GetAbstractProfilePicture(int aUserId) {
            return SocialPhotoModel.Create(GetProfilePicture(aUserId));
        }

        public void SetPhotoAsAlbumCover(int aPhotoId) {
            Photo myNewCover = GetPhoto(aPhotoId);
            myNewCover.AlbumCover = true;

            Photo myCurrentCover = GetPhotoAlbumCoverPhoto(myNewCover.PhotoAlbumId);

            theEntities.ApplyCurrentValues(myNewCover.EntityKey.EntitySetName, myNewCover);

            if (myCurrentCover != null) {
                myCurrentCover.AlbumCover = false;
                theEntities.ApplyCurrentValues(myCurrentCover.EntityKey.EntitySetName, myCurrentCover);
            }

            theEntities.SaveChanges();
        }

        private Photo GetPhoto(int aPhotoId) {
            return (from up in theEntities.Photos
                    where up.Id == aPhotoId
                    select up).FirstOrDefault();
        }

        private Photo GetProfilePicture(int aUserId) {
            return (from up in theEntities.Photos
                    where up.User.Id == aUserId
                    && up.ProfilePicture == true
                    select up).FirstOrDefault();
        }

        private void UnSetCurrentPhoto(User aUser) {
            Photo currentProfilePicture = GetProfilePicture(aUser.Id);
            if (currentProfilePicture != null) {
                currentProfilePicture.ProfilePicture = false;
                theEntities.ApplyCurrentValues(currentProfilePicture.EntityKey.EntitySetName, currentProfilePicture);
            }
        }
    
        private Photo GetPhotoAlbumCoverPhoto(int anAlbumId) {
            return (from p in theEntities.Photos
                    where p.PhotoAlbumId == anAlbumId 
                    && p.AlbumCover == true
                    select p).FirstOrDefault<Photo>();
        }
    }
}