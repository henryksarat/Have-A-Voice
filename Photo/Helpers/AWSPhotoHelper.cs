using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using Social.Generic.Constants;
using Amazon.S3;
using Amazon.S3.Model;

namespace Social.Photo.Helpers {
    public static class AWSPhotoHelper {
        public static S3Response PhysicallyDeletePhoto(AmazonS3 anS3Client, string aBucketName, string aFileName) {
            DeleteObjectRequest myDeleteRequest = new DeleteObjectRequest();
            myDeleteRequest.WithBucketName(aBucketName).WithKey(aFileName);

            return anS3Client.DeleteObject(myDeleteRequest);
        }

        public static string TakeImageAndResizeAndUpload(HttpPostedFileBase anImageFile, AmazonS3 anS3Client, string aBucketName, string anImageNamePrefix, int aMaxSize) {
            string myFileNamePrefix = anImageNamePrefix + "_" + DateTime.UtcNow.GetHashCode() + ".jpg";
            ResizeImageAndUpload(anImageFile, anS3Client, aBucketName, myFileNamePrefix, aMaxSize);
            return myFileNamePrefix;
        }

        public static void ResizeImageAndUpload(AmazonS3 anAmazonS3Client, string aBucketName, string aCurrentPhotoName, string aNewImageName, int aSize) {
            GetObjectRequest myGetRequest = new GetObjectRequest().WithBucketName(aBucketName).WithKey(aCurrentPhotoName);
            GetObjectResponse myResponse = anAmazonS3Client.GetObject(myGetRequest);
            Stream myStream = myResponse.ResponseStream;
            ResizeAndUpload(myStream, anAmazonS3Client, aBucketName, aNewImageName, aSize);
        }

        public static void ResizeImageAndUpload(HttpPostedFileBase anImageFile, AmazonS3 anAmazonS3Client, string aBucketName, string aNewImageName, int aSize) {
            ResizeAndUpload(anImageFile.InputStream, anAmazonS3Client, aBucketName, aNewImageName, aSize);
        }

        private static void ResizeAndUpload(Stream aStream, AmazonS3 anS3Client, string aBucketName, string aNewImageName, int aSize) {
            Image myOriginal = Image.FromStream(aStream);
            Image myActual = ScaleBySize(myOriginal, aSize);

            MemoryStream myMemoryStream = imageToMemoryStream(myActual);

            PutObjectRequest myRequest = new PutObjectRequest();

            myRequest.WithBucketName(aBucketName)
                .WithCannedACL(S3CannedACL.PublicRead)
                .WithKey(aNewImageName)
                .InputStream = myMemoryStream;

            S3Response myResponse = anS3Client.PutObject(myRequest);

            myActual.Dispose();
            myMemoryStream.Dispose();
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

        private static MemoryStream imageToMemoryStream(Image imageIn) {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, ImageFormat.Jpeg);
            return ms;
        }
    }
}
