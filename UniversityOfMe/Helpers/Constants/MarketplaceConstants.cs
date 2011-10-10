using UniversityOfMe.Helpers.Configuration;
namespace UniversityOfMe.Helpers.Constants {
    public class MarketplaceConstants {
        public static string MARKETPLACE_PHOTO_PATH = "http://s3.amazonaws.com/" + SiteConfiguration.MarketplacePhotosBucket() + "/";
        public const int ITEM_MAX_SIZE = 400;
    }
}