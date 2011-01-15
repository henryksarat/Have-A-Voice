using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Repositories;
using HaveAVoice.Repositories.UserFeatures;
using HaveAVoice.Models;
using HaveAVoice.Helpers;
using HaveAVoice.Exceptions;
using HaveAVoice.Validation;
using HaveAVoice.Models.View;

namespace HaveAVoice.Services.UserFeatures {
    public class HAVPhotoAlbumService : HAVBaseService, IHAVPhotoAlbumService {
        private IValidationDictionary theValidationDictionary;
        private IHAVFriendService theFriendService;
        private IHAVPhotoRepository thePhotoRepo;

        public HAVPhotoAlbumService(IValidationDictionary validationDictionary)
            : this(validationDictionary, new HAVFriendService(), new EntityHAVPhotoRepository(), new HAVBaseRepository()) { }

        public HAVPhotoAlbumService(IValidationDictionary aValidationDictionary, IHAVFriendService aFriendService, IHAVPhotoRepository aPhotoRepo, IHAVBaseRepository aBaseRepository)
            : base(aBaseRepository) {
            theValidationDictionary = aValidationDictionary;
            theFriendService = aFriendService;
            thePhotoRepo = aPhotoRepo;
        }

        public bool CreatePhotoAlbum(User aUser, string aName, string aDescription) {
            if (!ValidateAlbumName(aName)) {
                return false;
            }
            thePhotoRepo.CreatePhotoAlbum(aUser, aName, aDescription);

            return true;
        }

        public IEnumerable<PhotoAlbum> GetPhotoAlbumsForUser(User aRequestingUser, int aUserIdOfAlbum) {
            if(theFriendService.IsFriend(aUserIdOfAlbum, aRequestingUser)) {
                return thePhotoRepo.GetPhotoAlbumsForUser(aUserIdOfAlbum);
            }

            throw new NotFriendException();
        }

        public PhotoAlbum GetPhotoAlbum(UserInformationModel aUserModel, int anAlbumId) {
            PhotoAlbum myAlbum = thePhotoRepo.GetPhotoAlbum(anAlbumId);
            if (theFriendService.IsFriend(myAlbum.CreatedByUserId, aUserModel.Details)) {
                return myAlbum;
            }

            throw new NotFriendException();
        }

        public PhotoAlbum GetPhotoAlbumForEdit(UserInformationModel aUserModel, int anAlbumId) {
            PhotoAlbum myAlbum = thePhotoRepo.GetPhotoAlbum(anAlbumId);
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

        public bool EditPhotoAlbum(User aUserEditing, int anAlbumId, string aName, string aDescription) {
            if (!ValidateAlbumName(aName)) {
                return false;
            }

            PhotoAlbum myAlbum = thePhotoRepo.GetPhotoAlbum(anAlbumId);

            if (myAlbum.CreatedByUserId == aUserEditing.Id) {
                thePhotoRepo.EditPhotoAlbum(anAlbumId, aName, aDescription);
                return true;
            }

            throw new CustomException(HAVConstants.NOT_ALLOWED);
        }
    }
}