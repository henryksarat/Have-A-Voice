using System;
using System.Collections.Generic;
using System.Web.Mvc;
using HaveAVoice.Helpers.UserInformation;
using HaveAVoice.Services.UserFeatures;
using HaveAVoice.Repositories.UserFeatures;
using Social.Friend.Services;
using Social.Photo.Services;
using HaveAVoice.Models.SocialWrappers;

namespace HaveAVoice.Models.View {
    public class PhotosModelBinder : IModelBinder {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext) {
            IPhotoService<User, PhotoAlbum, Photo, Friend> myPhotoService = new PhotoService<User, PhotoAlbum, Photo, Friend>(new FriendService<User, Friend>(new EntityHAVFriendRepository()), new EntityHAVPhotoAlbumRepository(), new EntityHAVPhotoRepository());
            User myUserInfo = HAVUserInformationFactory.GetUserInformation().Details;

            int myUserId = Int32.Parse(BinderHelper.GetA(bindingContext, "UserId"));
            int myAlbumId = Int32.Parse(BinderHelper.GetA(bindingContext, "AlbumId"));
            string myProfilePictureURL = BinderHelper.GetA(bindingContext, "ProfilePictureURL");
            string selectedProfilePictureIds = BinderHelper.GetA(bindingContext, "SelectedProfilePictureId").Trim();

            IEnumerable<Photo> myPhotos = myPhotoService.GetPhotos(SocialUserModel.Create(myUserInfo), myAlbumId, myUserId);

            string[] splitIds = selectedProfilePictureIds.Split(',');
            List<int> selectedProfilePictures = new List<int>();

            foreach (string id in splitIds) {
                if (id != string.Empty) {
                    selectedProfilePictures.Add(Int32.Parse(id));
                }
            }

            return new PhotosModel() {
                UserId = myUserId,
                ProfilePictureURL = myProfilePictureURL,
                Photos = myPhotos,
                SelectedPhotos = selectedProfilePictures
            };
        }
    }
}
