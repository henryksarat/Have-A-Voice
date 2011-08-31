using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UniversityOfMe.Helpers.Configuration;

namespace UniversityOfMe.Helpers.Constants {
    public class PhotoConstants {
        public static string NO_PROFILE_PICTURE = "http://s3.amazonaws.com/" + SiteConfiguration.UserPhotosBucket() + "/no_profile_picture.jpg";
        public static string PHOTO_BASE_URL = "http://s3.amazonaws.com/" + SiteConfiguration.UserPhotosBucket() + "/";
    }
}