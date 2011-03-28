using System.Web;
using HaveAVoice.Models;
using HaveAVoice.Repositories.UserFeatures;
using HaveAVoice.Services.UserFeatures;
using Social.Authentication;
using Social.Generic.Models;

namespace HaveAVoice.Helpers.UserInformation {
    public class HAVUserInformationFactory {
        private static IUserInformation<User, WhoIsOnline> theFactory;
        
        private HAVUserInformationFactory() { }

        public static UserInformationModel<User> GetUserInformation() {
            SetDefaultInstance();
            return theFactory.GetUserInformaton();
        }

        public static bool IsLoggedIn() {
            return GetUserInformation() != null;
        }

        public static void SetInstance(IUserInformation<User, WhoIsOnline> userInformation) {
            theFactory = userInformation;
        }

        private static void SetDefaultInstance() {
            if (theFactory == null) {
                theFactory = UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityHAVWhoIsOnlineRepository()));
            }
        }
    }
}
