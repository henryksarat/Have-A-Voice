namespace UniversityOfMe.Helpers.Configuration {
    public class SiteConfiguration {
        public static string AWSAccessKey() {
            return GetKey("AWSAccessKey");
        }

        public static string AWSSecretKey() {
            return GetKey("AWSSecretKey");
        }

        public static string UserPhotosBucket() {
            return GetKey("UserPhotosBucket");
        }

        public static string ProfessorPhotosBucket() {
            return GetKey("ProfessorPhotosBucket");
        }

        public static string OrganizationPhotosBucket() {
            return GetKey("OrganizationPhotosBucket");
        }

        public static string TextbookPhotosBucket() {
            return GetKey("TextbookPhotosBucket");
        }

        public static string MarketplacePhotosBucket() {
            return GetKey("MarketplacePhotosBucket");
        }

        public static string NewAccountsEmail() {
            return GetKey("NewAccountsEmail");
        }

        public static string NotificationsEmail() {
            return GetKey("NotificationsEmail");
        }

        public static int ResultsPerPage() {
            return int.Parse(GetKey("ResultsPerPage"));
        }

        public static int PagesPadding() {
            return int.Parse(GetKey("PagesPadding"));
        }

        public static string[] FlirtAdjectives() {
            return GetKey("FlirtAdjectives").Split(',');
        }

        public static string[] FlirtDeliciousTreats() {
            return GetKey("FlirtDeliciousTreats").Split(',');
        }

        public static string[] FlirtAnimals() {
            return GetKey("FlirtAnimals").Split(',');
        }

        private static string GetKey(string aKey) {
            string myValue = string.Empty;
            System.Configuration.Configuration rootWebConfig1 =
                System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("/");
            if (rootWebConfig1.AppSettings.Settings.Count > 0) {
                System.Configuration.KeyValueConfigurationElement customSetting =
                    rootWebConfig1.AppSettings.Settings[aKey];
                if (customSetting != null) {
                    myValue = customSetting.Value;
                }
            }

            return myValue;
        }
    }
}