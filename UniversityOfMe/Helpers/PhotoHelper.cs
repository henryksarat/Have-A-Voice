using System.Linq;
using HaveAVoice.Services.UserFeatures;
using Social.Friend.Services;
using Social.Photo.Services;
using UniversityOfMe.Models;
using UniversityOfMe.Repositories.Friends;
using UniversityOfMe.Repositories.Photos;
using UniversityOfMe.Helpers.Constants;

namespace UniversityOfMe.Helpers {
    public class PhotoHelper {
        public static string ProfilePicture(User aUser) {
            string myProfileUrl = Social.Generic.Constants.Constants.NO_PROFILE_PICTURE_URL;

            Photo myProfilePicture = (from u in aUser.Photos where u.ProfilePicture == true select u).FirstOrDefault<Photo>();
            if (myProfilePicture != null) {
                myProfileUrl = Social.Generic.Constants.Constants.PHOTO_LOCATION_FROM_VIEW + myProfilePicture.ImageName;
            } else {
                IPhotoService<User, PhotoAlbum, Photo, Friend> myPhotoService = new PhotoService<User, PhotoAlbum, Photo, Friend>(new FriendService<User, Friend>(new EntityFriendRepository()), new EntityPhotoAlbumRepository(), new EntityPhotoRepository());
                Photo myPhoto = myPhotoService.GetProfilePicture(aUser.Id);

                if (myPhoto != null) {
                    myProfileUrl = Social.Generic.Constants.Constants.PHOTO_LOCATION_FROM_VIEW + myPhoto.ImageName;
                }
            }

            return myProfileUrl;
        }

        public static string ConstructUrl(string anImageName) {
            return Social.Photo.Helpers.SocialPhotoHelper.ConstructUrl(anImageName);
        }
        
        public static string ConstructProfessorUrl(Professor aProfessor) {
            string myProfessorImage = ConstructProfessorUrl(ProfessorConstants.NO_PROFESOR_IMAGE);
            if (aProfessor.ProfessorSuggestedPhotoId != null) {
                myProfessorImage = ConstructProfessorUrl(aProfessor.ProfessorSuggestedPhoto.ImageName);
            } else if (!string.IsNullOrEmpty(aProfessor.ProfessorImage)) {
                myProfessorImage = ConstructProfessorUrl(aProfessor.ProfessorImage);
            }
            return myProfessorImage;
        }

        public static string ConstructProfessorUrl(string anImageName) {
            return ProfessorConstants.PROFESSOR_PHOTO_PATH + anImageName;
        }

        public static string PhotoAlbumCover(PhotoAlbum myPhotoAlbum) {
            Photo myDefaultPhoto = (from p in myPhotoAlbum.Photos
                                    where p.AlbumCover == true
                                    select p).FirstOrDefault<Photo>();
            string myPhotoAlbumCoverImageName = myDefaultPhoto != null ? myDefaultPhoto.ImageName : "nophotos.jpg";
            return ConstructUrl(myPhotoAlbumCoverImageName);
        }
    }
}