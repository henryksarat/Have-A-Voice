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

        public IEnumerable<PhotoAlbum> GetPhotoAlbumsForUser(User aUser) {
            return thePhotoRepo.GetPhotoAlbumsForUser(aUser);
        }

        public PhotoAlbum GetPhotoAlbum(UserInformationModel aUserModel, int anAlbumId) {
            return thePhotoRepo.GetPhotoAlbum(aUserModel.Details, anAlbumId);
        }

        private bool ValidateAlbumName(string aName) {
            if (String.IsNullOrEmpty(aName)) {
                theValidationDictionary.AddError("Name", aName, "A name for the album must be specified.");
            }
            return theValidationDictionary.isValid;
        }
    }
}