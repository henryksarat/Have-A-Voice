using System.Web;
using Social.Authentication;
using Social.Generic.Models;
using Social.Users.Services;
using UniversityOfMe.Models;
using UniversityOfMe.Repositories;
using UniversityOfMe.Helpers;

namespace UniversityOfMe.UserInformation {
    public class UserInformationFactory {
        private static IUserInformation<User, WhoIsOnline> theFactory;

        private UserInformationFactory() { }

        public static IUserInformation<User, WhoIsOnline> GetUserInformationInstance() {
            return theFactory;
        }

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

        private static void SetDefaultInstance() {
            if (theFactory == null) {
                theFactory = UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository()), new GetUserStrategy());
            }
        }
    }
}
