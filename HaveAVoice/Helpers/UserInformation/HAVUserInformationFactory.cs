using System.Web;
using HaveAVoice.Models;
using HaveAVoice.Repositories.UserFeatures;
using Social.Authentication;
using Social.Generic.Models;
using Social.Users.Services;
using Social.Authentication.Helpers;

namespace HaveAVoice.Helpers.UserInformation {
    public class HAVUserInformationFactory {
        private static IUserInformation<User, WhoIsOnline> theFactory;
        
        private HAVUserInformationFactory() { }

        public static UserInformationModel<User> GetUserInformation() {
            SetDefaultInstance();
            UserInformationModel<User> myUserInfo = theFactory.GetUserInformaton();

            if (myUserInfo == null) {
                int myUserId = CookieHelper.GetUserIdFromCookie();
                myUserInfo = theFactory.GetUserInformaton(myUserId);
            }

            return myUserInfo;
        }

        public static bool IsLoggedIn() {
            SetDefaultInstance();
            //HUGE FUCKIGN HACK.. DID THIS BECAUSE FORMS AUTH DIDN't LAST FOR MORE THAN 5 minutes
            if (CookieHelper.GetUserIdFromCookie() == 0) {
                return theFactory.IsLoggedIn();
            } else {
                return true;
            }
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
