using System;
using System.Collections.Generic;
using HaveAVoice.Exceptions;
using HaveAVoice.Helpers;
using HaveAVoice.Models;
using HaveAVoice.Models.SocialWrappers;
using HaveAVoice.Repositories;
using HaveAVoice.Repositories.UserFeatures;
using Social.Friend.Exceptions;
using Social.Friend.Services;
using Social.Generic.Models;
using Social.User.Models;
using Social.Validation;
using Social.Generic.Repositories;

namespace HaveAVoice.Services.UserFeatures {
    public class HAVPhotoAlbumService : IHAVPhotoAlbumService {
        private IValidationDictionary theValidationDictionary;
        private IFriendService<User, Friend> theFriendService;
        private IHAVPhotoAlbumRepository thePhotoAlbumRepo;
        private IHAVPhotoService thePhotoService;

        public HAVPhotoAlbumService(IValidationDictionary validationDictionary)
            : this(validationDictionary, new HAVPhotoService(), new FriendService<User, Friend>(new EntityHAVFriendRepository()), new EntityHAVPhotoAlbumRepository()) { }

        public HAVPhotoAlbumService(IValidationDictionary aValidationDictionary, IHAVPhotoService aPhotoService, IFriendService<User, Friend> aFriendService, IHAVPhotoAlbumRepository aPhotoAlbumRepo) {
            theValidationDictionary = aValidationDictionary;
            thePhotoService = aPhotoService;
            theFriendService = aFriendService;
            thePhotoAlbumRepo = aPhotoAlbumRepo;
        }

        public bool CreatePhotoAlbum(User aUser, string aName, string aDescription) {
            if (!ValidateAlbumName(aName)) {
                return false;
            }
            thePhotoAlbumRepo.Create(aUser, aName, aDescription);

            return true;
        }

        public IEnumerable<PhotoAlbum> GetPhotoAlbumsForUser(User aRequestingUser, int aUserIdOfAlbum) {
            AbstractUserModel<User> myAbstractUserModel = SocialUserModel.Create(aRequestingUser);
            if (theFriendService.IsFriend(aUserIdOfAlbum, myAbstractUserModel)) {
                return thePhotoAlbumRepo.GetPhotoAlbumsForUser(aUserIdOfAlbum);
            }

            throw new NotFriendException();
        }

        public PhotoAlbum GetPhotoAlbum(UserInformationModel<User> aUserModel, int anAlbumId) {
            PhotoAlbum myAlbum = thePhotoAlbumRepo.GetPhotoAlbum(anAlbumId);
            AbstractUserModel<User> myAbstractUserModel = SocialUserModel.Create(aUserModel.Details);
            if (theFriendService.IsFriend(myAlbum.CreatedByUserId, myAbstractUserModel)) {
                return myAlbum;
            }

            throw new NotFriendException();
        }

        public PhotoAlbum GetPhotoAlbumForEdit(UserInformationModel<User> aUserModel, int anAlbumId) {
            PhotoAlbum myAlbum = thePhotoAlbumRepo.GetPhotoAlbum(anAlbumId);
            if (myAlbum.CreatedByUserId == aUserModel.Details.Id) {
                return myAlbum;
            }

            throw new CustomException(HAVConstants.NOT_ALLOWED);
        }

        private bool ValidateAlbumName(string aName) {
            if (String.IsNullOrEmpty(aName)) {
                theValidationDictionary.AddError("Name", aName, "A name for the album must be specified.");
            }
            return theValidationDictionary.isValid;
        }

        public bool EditPhotoAlbum(UserInformationModel<User> aUserEditingModel, int anAlbumId, string aName, string aDescription) {
            if (!ValidateAlbumName(aName)) {
                return false;
            }

            PhotoAlbum myAlbum = thePhotoAlbumRepo.GetPhotoAlbum(anAlbumId);

            if (myAlbum.CreatedByUserId == aUserEditingModel.Details.Id) {
                thePhotoAlbumRepo.Edit(anAlbumId, aName, aDescription);
                return true;
            }

            throw new CustomException(HAVConstants.NOT_ALLOWED);
        }

        public void DeletePhotoAlbum(UserInformationModel<User> aUserDeletingModel, int anAlbumId) {
            PhotoAlbum myAlbum = thePhotoAlbumRepo.GetPhotoAlbum(anAlbumId);

            if (myAlbum.CreatedByUserId == aUserDeletingModel.Details.Id) {
                foreach (Photo myPhoto in myAlbum.Photos) {
                    thePhotoService.DeletePhoto(aUserDeletingModel.Details, myPhoto.Id);
                }

                thePhotoAlbumRepo.Delete(anAlbumId);
            } else {
                throw new CustomException(HAVConstants.NOT_ALLOWED);
            }
        }

        public PhotoAlbum GetProfilePictureAlbumForUser(User aUsersPhotoAlbum) {
            return thePhotoAlbumRepo.GetProfilePictureAlbumForUser(aUsersPhotoAlbum);
        }
    }
}