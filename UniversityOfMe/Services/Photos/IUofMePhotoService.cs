using Social.Photo.Services;
using UniversityOfMe.Models;
using UniversityOfMe.Models.View;
using Social.Generic.Models;
using System.Web;
using Amazon.S3;

namespace UniversityOfMe.Services.Photos {
    public interface IUofMePhotoService : IPhotoService<User, PhotoAlbum, Photo, Friend> {
        PhotoDisplayView GetPhotoDisplayView(User aUserInformation, int aPhotoId);
        bool HasProfilePhoto(User aUser);
        void UploadProfilePicture(UserInformationModel<User> aUser, HttpPostedFileBase anImageFile, AmazonS3 anAmazonS3Client, string aBucketName, int aMaxSize);
    }
}