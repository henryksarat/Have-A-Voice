using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using HaveAVoice.Exceptions;
using HaveAVoice.Helpers;
using HaveAVoice.Models;
using HaveAVoice.Models.SocialWrappers;
using HaveAVoice.Repositories.UserFeatures;
using HaveAVoice.Services.Helpers;
using Social.Friend.Exceptions;
using Social.Friend.Services;
using Social.Generic.Models;
using Social.Validation;

namespace HaveAVoice.Services.UserFeatures {
    public class HAVPhotoService : IHAVPhotoService {
        private const int MAX_SIZE = 840;
        private const string UNAUTHORIZED_UPLOAD = "You are not allowed to upload to that album.";

        private IFriendService<User, Friend> theFriendService;
        private IHAVPhotoAlbumRepository thePhotoAlbumRepo;
        private IHAVPhotoRepository thePhotoRepo;
 
        public HAVPhotoService()
            : this(new FriendService<User, Friend>(new EntityHAVFriendRepository()), new EntityHAVPhotoAlbumRepository(), new EntityHAVPhotoRepository()) { }

        public HAVPhotoService(IFriendService<User, Friend> aFriendService, IHAVPhotoAlbumRepository aPhotoAlbumRepo, IHAVPhotoRepository aPhotoRepo) {
            thePhotoAlbumRepo = aPhotoAlbumRepo;
            theFriendService = aFriendService;
            thePhotoRepo = aPhotoRepo;
        }

        public IEnumerable<Photo> GetPhotos(User aViewingUser, int anAlbumId, int aUserId) {
            AbstractUserModel<User> myAbstractUser = SocialUserModel.Create(aViewingUser);

            if (theFriendService.IsFriend(aUserId, myAbstractUser)) {
                return thePhotoRepo.GetPhotos(aUserId, anAlbumId);
            }

            throw new NotFriendException();
        }

        public void DeletePhoto(User aUserDeleting, int aPhotoId) {
            Photo myPhoto = GetPhoto(aUserDeleting, aPhotoId);
            if (myPhoto.UploadedByUserId == aUserDeleting.Id) {
                PhysicallyDeletePhoto(myPhoto.ImageName);

                thePhotoRepo.DeletePhoto(aPhotoId);
            } else {
                throw new CustomException(HAVConstants.NOT_ALLOWED);
            }
        }

        public Photo GetPhoto(User aViewingUser,  int aPhotoId) {
            Photo myPhotoId = thePhotoRepo.GetPhoto(aPhotoId);
            AbstractUserModel<User> myAbstractUserModel = SocialUserModel.Create(aViewingUser);
            if (theFriendService.IsFriend(myPhotoId.UploadedByUserId, myAbstractUserModel)) {
                return myPhotoId;
            }

            throw new NotFriendException();
        }

        public void SetToProfilePicture(User aUser, int aPhotoId) {
            Photo myCurrentProfile = thePhotoRepo.GetProfilePicture(aUser.Id);
            if (myCurrentProfile != null) {
                if (myCurrentProfile.UploadedByUserId == aUser.Id) {
                    thePhotoRepo.DeletePhoto(myCurrentProfile.Id);
                    PhysicallyDeletePhoto(myCurrentProfile.ImageName);
                } else {
                    throw new CustomException(HAVConstants.NOT_ALLOWED);
                }
            }

            Photo myNewProfilePhoto = thePhotoRepo.GetPhoto(aPhotoId);

            if (myNewProfilePhoto.UploadedByUserId == aUser.Id) {
                string[] myNewProfileSplit = myNewProfilePhoto.ImageName.Split('.');
                string myNewProfilePictureImageName = myNewProfileSplit[0] + "-profile." + myNewProfileSplit[1];
                ResizeImage(myNewProfilePhoto.ImageName, myNewProfilePictureImageName, 120);
                thePhotoRepo.AddReferenceToImage(aUser, myNewProfilePhoto.PhotoAlbumId, myNewProfilePictureImageName, true);
            } else {
                throw new CustomException(HAVConstants.NOT_ALLOWED);
            }
        }

        public void UploadProfilePicture(User aUserToUploadFor, HttpPostedFileBase aImageFile) {
            PhotoAlbum myProfilePictureAlbum = thePhotoAlbumRepo.GetProfilePictureAlbumForUser(aUserToUploadFor);
            string myImageName = UploadImage(aUserToUploadFor, aImageFile);
            Photo myPhoto = thePhotoRepo.AddReferenceToImage(aUserToUploadFor, myProfilePictureAlbum.Id, myImageName, true);
        }

        public Photo GetProfilePicutre(User aUser) {
            return GetProfilePicture(aUser.Id);
        }

        public Photo GetProfilePicture(int aUserId) {
            return thePhotoRepo.GetProfilePicture(aUserId);
        }

        public void UploadImageWithDatabaseReference(UserInformationModel<User> aUserToUploadFor, int anAlbumId, HttpPostedFileBase aImageFile) {
            PhotoAlbum myAlbum = thePhotoAlbumRepo.GetPhotoAlbum(anAlbumId);
             if (myAlbum.CreatedByUserId == aUserToUploadFor.Details.Id) {
                 string myImageName = UploadImage(aUserToUploadFor.Details, aImageFile);
                 thePhotoRepo.AddReferenceToImage(aUserToUploadFor.Details, anAlbumId, myImageName, false);
             }

             new CustomException(UNAUTHORIZED_UPLOAD);
         }

        public void SetPhotoAsAlbumCover(User myEditingUser, int aPhotoId) {
            Photo myPhoto = thePhotoRepo.GetPhoto(aPhotoId);

            if (myPhoto.UploadedByUserId == myEditingUser.Id) {
                thePhotoRepo.SetPhotoAsAlbumCover(aPhotoId);
            } else {
                throw new CustomException(HAVConstants.NOT_ALLOWED);
            }
        }

        private string UploadImage(User aUserToUploadFor, HttpPostedFileBase aImageFile) {
            if(!PhotoValidation.IsValidImageFile(aImageFile.FileName)) {
                    throw new CustomException("Please specify a proper image file that ends in .gif, .jpg, or .jpeg.");
            }
            string[] mySplitOnPeriod = aImageFile.FileName.Split(new char[] { '.' });
            string myFileExtension = mySplitOnPeriod[mySplitOnPeriod.Length - 1];
            string myFileNamePrefix = aUserToUploadFor.Id + "_" + DateTime.UtcNow.GetHashCode();
            string myOriginalFile = myFileNamePrefix + "-original." + myFileExtension;
            string myNewFile = myFileNamePrefix + "." + myFileExtension;
            string myOriginalFilePath = HttpContext.Current.Server.MapPath(PhotoHelper.ConstructUrl(myOriginalFile));
            aImageFile.SaveAs(myOriginalFilePath);
            
            ResizeImage(myOriginalFile, myNewFile, MAX_SIZE);

            PhysicallyDeletePhoto(myOriginalFile);
            return myNewFile;
        }

        private void ResizeImage(string anOriginalImageName, string aNewImageName, int aSize) {
            string myOriginalFilePath = HttpContext.Current.Server.MapPath(PhotoHelper.ConstructUrl(anOriginalImageName));
            string myNewFilePath = HttpContext.Current.Server.MapPath(PhotoHelper.ConstructUrl(aNewImageName));

            Image myOriginal = Image.FromFile(myOriginalFilePath);

            Image myActual = ScaleBySize(myOriginal, aSize);
            myActual.Save(myNewFilePath);
            myActual.Dispose();

            myOriginal.Dispose();
        }

        private Image ScaleBySize(Image myPhoto, int aSize) {
            float mySourceWidth = myPhoto.Width;
            float mySourceHeight = myPhoto.Height;
            float myDesiredHeight = mySourceHeight;
            float myDesiredWidth = mySourceWidth;
            
            if (mySourceWidth > mySourceHeight && mySourceWidth > aSize) {
                myDesiredWidth = aSize;
                myDesiredHeight = (float)(mySourceHeight * aSize / mySourceWidth);
            } else if (mySourceHeight > mySourceWidth && mySourceHeight > aSize) {
                myDesiredHeight = aSize;
                myDesiredWidth = (float)(mySourceWidth * aSize / mySourceHeight);
            } else if(mySourceWidth > aSize && mySourceHeight > aSize) {
                myDesiredWidth = aSize;
                myDesiredHeight = aSize;
            }

            Bitmap myBitmap = new Bitmap((int)myDesiredWidth, (int)myDesiredHeight,
                                        PixelFormat.Format32bppPArgb);
            myBitmap.SetResolution(myPhoto.HorizontalResolution, myPhoto.VerticalResolution);

            Graphics myGraphicPhoto = Graphics.FromImage(myBitmap);
            myGraphicPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

            myGraphicPhoto.DrawImage(myPhoto,
                new Rectangle(0, 0, (int)myDesiredWidth, (int)myDesiredHeight),
                new Rectangle(0, 0, (int)mySourceWidth, (int)mySourceHeight),
                GraphicsUnit.Pixel);

            myGraphicPhoto.Dispose();

            return myBitmap;
        }

        private void PhysicallyDeletePhoto(string anImageName) {
            FileInfo myFile = new FileInfo(HttpContext.Current.Server.MapPath(PhotoHelper.ConstructUrl(anImageName)));
            if (myFile.Exists) {
                myFile.Delete();
            } else {
                throw new FileNotFoundException();
            }
        }
    }
}