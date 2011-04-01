using System;
using System.Collections.Generic;
using Social.Friend.Exceptions;
using Social.Friend.Services;
using Social.Generic.Constants;
using Social.Generic.Exceptions;
using Social.Generic.Models;
using Social.Photo.Repositories;
using Social.Validation;

namespace Social.Photo.Services {
    //T = User
    //U = PhotoAlbum
    //V = Photo
    //W = Friend
    public class PhotoAlbumService<T, U, V, W> : IPhotoAlbumService<T, U, V, W> {
        private IValidationDictionary theValidationDictionary;
        private IFriendService<T, W> theFriendService;
        private IPhotoAlbumRepository<T, U, V> thePhotoAlbumRepo;
        private IPhotoService<T, U, V, W> thePhotoService;

        public PhotoAlbumService(IValidationDictionary aValidationDictionary, IPhotoService<T, U, V, W> aPhotoService, IFriendService<T, W> aFriendService, IPhotoAlbumRepository<T, U, V> aPhotoAlbumRepo) {
            theValidationDictionary = aValidationDictionary;
            thePhotoService = aPhotoService;
            theFriendService = aFriendService;
            thePhotoAlbumRepo = aPhotoAlbumRepo;
        }

        public bool CreatePhotoAlbum(T aUser, string aName, string aDescription) {
            if (!ValidateAlbumName(aName)) {
                return false;
            }
            thePhotoAlbumRepo.Create(aUser, aName, aDescription);

            return true;
        }

        public IEnumerable<U> GetPhotoAlbumsForUser(AbstractUserModel<T> aRequestingUser, int aUserIdOfAlbum) {
            if (theFriendService.IsFriend(aUserIdOfAlbum, aRequestingUser)) {
                return thePhotoAlbumRepo.GetPhotoAlbumsForUser(aUserIdOfAlbum);
            }

            throw new NotFriendException();
        }

        public U GetPhotoAlbum(AbstractUserModel<T> aUserModel, int anAlbumId) {
            AbstractPhotoAlbumModel<U, V> myAlbum = thePhotoAlbumRepo.GetAbstractPhotoAlbum(anAlbumId);
            if (theFriendService.IsFriend(myAlbum.CreatedByUserId, aUserModel)) {
                return myAlbum.Model;
            }

            throw new NotFriendException();
        }

        public U GetPhotoAlbumForEdit(AbstractUserModel<T> aUserModel, int anAlbumId) {
            AbstractPhotoAlbumModel<U, V> myAlbum = thePhotoAlbumRepo.GetAbstractPhotoAlbum(anAlbumId);
            if (myAlbum.CreatedByUserId == aUserModel.Id) {
                return myAlbum.Model;
            }

            throw new CustomException(ErrorKeys.PERMISSION_DENIED);
        }

        private bool ValidateAlbumName(string aName) {
            if (String.IsNullOrEmpty(aName)) {
                theValidationDictionary.AddError("Name", aName, "A name for the album must be specified.");
            }
            return theValidationDictionary.isValid;
        }

        public bool EditPhotoAlbum(AbstractUserModel<T> anEditUser, int anAlbumId, string aName, string aDescription) {
            if (!ValidateAlbumName(aName)) {
                return false;
            }

            AbstractPhotoAlbumModel<U, V> myAlbum = thePhotoAlbumRepo.GetAbstractPhotoAlbum(anAlbumId);

            if (myAlbum.CreatedByUserId == anEditUser.Id) {
                thePhotoAlbumRepo.Edit(anAlbumId, aName, aDescription);
                return true;
            }

            throw new CustomException(ErrorKeys.PERMISSION_DENIED);
        }

        public void DeletePhotoAlbum(AbstractUserModel<T> aUserDeletingModel, int anAlbumId) {
            AbstractPhotoAlbumModel<U, V> myAlbum = thePhotoAlbumRepo.GetAbstractPhotoAlbum(anAlbumId);

            if (myAlbum.CreatedByUserId == aUserDeletingModel.Id) {
                foreach (AbstractPhotoModel<V> myPhoto in myAlbum.Photos) {
                    thePhotoService.DeletePhoto(aUserDeletingModel, myPhoto.Id);
                }

                thePhotoAlbumRepo.Delete(anAlbumId);
            } else {
                throw new CustomException(ErrorKeys.PERMISSION_DENIED);
            }
        }

        public AbstractPhotoAlbumModel<U, V> GetProfilePictureAlbumForUser(T aUsersPhotoAlbum) {
            return thePhotoAlbumRepo.GetAbstractProfilePictureAlbumForUser(aUsersPhotoAlbum);
        }
    }
}