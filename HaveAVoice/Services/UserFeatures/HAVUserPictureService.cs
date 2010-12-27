﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Repositories;
using HaveAVoice.Repositories.UserFeatures;
using HaveAVoice.Models;
using HaveAVoice.Helpers;

namespace HaveAVoice.Services.UserFeatures {
    public class HAVUserPictureService : HAVBaseService, IHAVUserPictureService {
        private IHAVUserPictureRepository theUserPictureRepo;
 
        public HAVUserPictureService()
            : this(new EntityHAVUserPictureRepository(), new HAVBaseRepository()) { }

        public HAVUserPictureService(IHAVUserPictureRepository aUserPictureRepo, IHAVBaseRepository aBaseRepository) : base(aBaseRepository) {
            theUserPictureRepo = aUserPictureRepo;
        }

        public void AddProfilePicture(User aUser, string anImageURL) {
            theUserPictureRepo.AddProfilePicture(aUser, anImageURL);
        }

        public string GetProfilePictureURL(User aUser) {
            UserPicture profilePicture = theUserPictureRepo.GetProfilePicture(aUser.Id);
            string profilePictureImageName;

            if (profilePicture == null) {
                profilePictureImageName = HAVConstants.NO_PROFILE_PICTURE_IMAGE;
            } else {
                profilePictureImageName = profilePicture.ImageName;
            }

            string filePath = HAVConstants.USER_PICTURE_LOCATION_FROM_VIEW + profilePictureImageName;
            return filePath;
        }

        public IEnumerable<UserPicture> GetUserPictures(int aUserId) {
            return theUserPictureRepo.GetUserPictures(aUserId);
        }

        public UserPicture GetProfilePicture(int aUserId) {
            return theUserPictureRepo.GetProfilePicture(aUserId);
        }

        public void DeleteUserPictures(List<int> aUserPictureIds) {
            foreach (int userPictureId in aUserPictureIds) {
                theUserPictureRepo.DeleteUserPicture(userPictureId);
            }
        }

        public UserPicture GetUserPicture(int aUserPictureId) {
            return theUserPictureRepo.GetUserPicture(aUserPictureId);
        }

        public void SetToProfilePicture(User aUser, int aUserPictureId) {
            theUserPictureRepo.SetToProfilePicture(aUser, aUserPictureId);
        }
    }
}