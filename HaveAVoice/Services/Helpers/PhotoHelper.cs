using System.Linq;
using HaveAVoice.Helpers;
using HaveAVoice.Helpers.Constants;
using HaveAVoice.Models;
using HaveAVoice.Repositories.UserFeatures;
using HaveAVoice.Services.UserFeatures;
using Social.Friend.Services;
using Social.Photo.Services;

namespace HaveAVoice.Services.Helpers {
    public class PhotoHelper {
        public static string ProfilePicture(User aUser) {
            string myProfileUrl = PhotoConstants.NO_PROFILE_PICTURE;

            if(!aUser.Equals(ProfileHelper.GetAnonymousProfile())) {
                Photo myProfilePicture = (from u in aUser.Photos where u.ProfilePicture == true select u).FirstOrDefault<Photo>();
                if (myProfilePicture != null) {
                    myProfileUrl = PhotoConstants.PHOTO_BASE_URL + myProfilePicture.ImageName;
                } else {
                    IPhotoService<User, PhotoAlbum, Photo, Friend> myPhotoService = new PhotoService<User, PhotoAlbum, Photo, Friend>(new FriendService<User, Friend>(new EntityHAVFriendRepository()), new EntityHAVPhotoAlbumRepository(), new EntityHAVPhotoRepository());
                    Photo myPhoto = myPhotoService.GetProfilePicture(aUser.Id);

                    if (myPhoto != null) {
                        myProfileUrl = PhotoConstants.PHOTO_BASE_URL + myPhoto.ImageName;
                    }
                }
            }

            return myProfileUrl;
        }

        public static string ConstructUrl(string anImageName) {
            return PhotoConstants.PHOTO_BASE_URL + anImageName;
        }

        public static string RetrievePhotoAlbumCoverUrl(PhotoAlbum anAlbum) {
            string myCover = "/Content/images/album.png";
            string myNewCover = (from p in anAlbum.Photos
                                 where p.AlbumCover == true
                                 select p.ImageName).FirstOrDefault<string>();
            if (myNewCover != null) {
                myCover = ConstructUrl(myNewCover);
            }

            return myCover;
        }
    }
}