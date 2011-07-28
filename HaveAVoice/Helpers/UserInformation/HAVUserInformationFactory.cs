using System.Web;
using HaveAVoice.Models;
using HaveAVoice.Repositories.UserFeatures;
using Social.Authentication;
using Social.Generic.Models;
using Social.Users.Services;

namespace HaveAVoice.Helpers.UserInformation {
    public class HAVUserInformationFactory {
        private static IUserInformation<User, WhoIsOnline> theFactory;
        
        private HAVUserInformationFactory() { }

        public static UserInformationModel<User> GetUserInformation() {
            SetDefaultInstance();
            return theFactory.GetUserInformaton();
        }

        public static bool IsLoggedIn() {
            SetDefaultInstance();
            return theFactory.IsLoggedIn();
        }

        public static void SetInstance(IUserInformation<User, WhoIsOnline> userInformation) {
            theFactory = userInformation;
        }

        public static IUserInformation<User, WhoIsOnline> GetInstance() {
            SetDefaultInstance();
            return theFactory;
        }

        public static string GetIdentityName() {
            return theFactory.GetIdentityName();
        }

        private static void SetDefaultInstance() {
            if (theFactory == null) {
                theFactory = UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityHAVWhoIsOnlineRepository()), new GetUserStrategy());
            }
        }
    }
}
