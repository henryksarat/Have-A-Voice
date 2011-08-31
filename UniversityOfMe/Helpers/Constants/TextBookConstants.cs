using UniversityOfMe.Helpers.Configuration;
namespace UniversityOfMe.Helpers.Constants {
    public class TextBookConstants {
        public static string TEXTBOOK_PHOTO_PATH = "http://s3.amazonaws.com/" + SiteConfiguration.TextbookPhotosBucket() + "/";
        public const int BOOK_MAX_SIZE = 207;
    }
}