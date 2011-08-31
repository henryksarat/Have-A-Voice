using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UniversityOfMe.Helpers.Configuration;

namespace UniversityOfMe.Helpers.Constants {
    public class ClubConstants {
        public static string CLUB_PHOTO_PATH = "http://s3.amazonaws.com/" + SiteConfiguration.OrganizationPhotosBucket() + "/";
        public const int CLUB_MAX_SIZE = 120;
        public const string DEFAULT_NEW_MEMBER_TITLE = "Member";
        public const string DEFAULT_CLUB_LEADER_TITLE = "Club Leader";
    }
}