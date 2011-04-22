using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using Social.Generic.Constants;

namespace Social.Photo.Helpers {
    public static class SocialPhotoHelper {
        public static string ConstructUrl(string anImageName) {
            return Constants.PHOTO_LOCATION_FROM_VIEW + anImageName;
        }

        public static void PhysicallyDeletePhoto(string aPhotoPath) {
            FileInfo myFile = new FileInfo(aPhotoPath);
            if (myFile.Exists) {
                myFile.Delete();
            } else {
                throw new FileNotFoundException();
            }
        }

        public static string TakeImageAndResizeAndUpload(string aPhotoStoragePath, string anImageNamePrefix, HttpPostedFileBase anImageFile, int aMaxSize) {
            string[] mySplitOnPeriod = anImageFile.FileName.Split(new char[] { '.' });
            string myFileExtension = mySplitOnPeriod[mySplitOnPeriod.Length - 1];
            string myFileNamePrefix = anImageNamePrefix + "_" + DateTime.UtcNow.GetHashCode();
            string myOriginalFile = myFileNamePrefix + "-original." + myFileExtension;
            string myNewFile = myFileNamePrefix + "." + myFileExtension;
            string myOriginalFilePath = HttpContext.Current.Server.MapPath(aPhotoStoragePath + myOriginalFile);
            anImageFile.SaveAs(myOriginalFilePath);

            ResizeImageAndUpload(aPhotoStoragePath, myOriginalFile, myNewFile, aMaxSize);

            SocialPhotoHelper.PhysicallyDeletePhoto(HttpContext.Current.Server.MapPath(aPhotoStoragePath + myOriginalFile));
            return myNewFile;
        }

        public static void ResizeImageAndUpload(string aPhotoStoragePath, string anOriginalImageName, string aNewImageName, int aSize) {
            string myOriginalFilePath = HttpContext.Current.Server.MapPath(aPhotoStoragePath + anOriginalImageName);
            string myNewFilePath = HttpContext.Current.Server.MapPath(aPhotoStoragePath + aNewImageName);

            Image myOriginal = Image.FromFile(myOriginalFilePath);

            Image myActual = ScaleBySize(myOriginal, aSize);
            myActual.Save(myNewFilePath);
            myActual.Dispose();

            myOriginal.Dispose();
        }

        private static Image ScaleBySize(Image myPhoto, int aSize) {
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
            } else if (mySourceWidth > aSize && mySourceHeight > aSize) {
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
    }
}
