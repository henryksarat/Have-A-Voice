namespace HaveAVoice.Helpers.Configuration {
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

        public static string NewAccountsEmail() {
            return GetKey("NewAccountsEmail");
        }

        public static string NotificationsEmail() {
            return GetKey("NotificationsEmail");
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