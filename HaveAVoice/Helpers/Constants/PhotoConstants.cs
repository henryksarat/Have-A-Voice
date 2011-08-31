using HaveAVoice.Helpers.Configuration;

namespace HaveAVoice.Helpers.Constants {
    public class PhotoConstants {
        public static string NO_PROFILE_PICTURE = "http://s3.amazonaws.com/" + SiteConfiguration.UserPhotosBucket() + "/no_profile_picture.jpg";
        public static string PHOTO_BASE_URL = "http://s3.amazonaws.com/" + SiteConfiguration.UserPhotosBucket() + "/";
    }
}