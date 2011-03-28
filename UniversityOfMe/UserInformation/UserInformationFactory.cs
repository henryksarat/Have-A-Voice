using System.Web;
using HaveAVoice.Services.UserFeatures;
using Social.Authentication;
using Social.Generic.Models;
using UniversityOfMe.Models;
using UniversityOfMe.Repositories;

namespace HaveAVoice.Helpers.UserInformation {
    public class UserInformationFactory {
        private static IUserInformation<User, WhoIsOnline> theFactory;

        private UserInformationFactory() { }

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
                theFactory = UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository()));
            }
        }
    }
}
