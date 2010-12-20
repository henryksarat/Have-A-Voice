﻿using System;
using System.Collections.Generic;
using System.Web.Mvc;
using HaveAVoice.Validation;
using HaveAVoice.Helpers;
using HaveAVoice.Services.UserFeatures;

namespace HaveAVoice.Models.View {
    public class UserPicturesModelBinder : IModelBinder {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext) {
            IHAVUserPictureService myUserPictureService = new HAVUserPictureService();

            int userId = Int32.Parse(BinderHelper.GetA(bindingContext, "userId"));
            string selectedProfilePictureIds = BinderHelper.GetA(bindingContext, "SelectedProfilePictureId").Trim();

            IEnumerable<UserPicture> userPictures = myUserPictureService.GetUserPictures(userId);
            UserPicture profilePicture = myUserPictureService.GetProfilePicture(userId);

            string[] splitIds = selectedProfilePictureIds.Split(',');
            List<int> selectedProfilePictures = new List<int>();

            foreach (string id in splitIds) {
                if (id != string.Empty) {
                    selectedProfilePictures.Add(Int32.Parse(id));
                }
            }

            return new UserPicturesModel(profilePicture, userPictures, selectedProfilePictures);
        }
    }
}
