using System.Web;
using HaveAVoice.Models;
using HaveAVoice.Repositories.UserFeatures;
using HaveAVoice.Services.UserFeatures;
using Social.Admin.Helpers;
using Social.Generic.Helpers;
using Social.Generic.Models;
using Social.Users.Services;

namespace HaveAVoice.Helpers.UserInformation {
    public class UserInformation : IUserInformation {
        private HttpContextBase theHttpContext;
        private IWhoIsOnlineService<User, WhoIsOnline> theWhoIsOnlineService;

        private UserInformation() {
        }

        private UserInformation(HttpContextBase aHttpBaseContext, IWhoIsOnlineService<User, WhoIsOnline> aWhoIsOnlineService) {
            theHttpContext = aHttpBaseContext;
            theWhoIsOnlineService = aWhoIsOnlineService;
        }

        public UserInformationModel<User> GetUserInformaton() {
            //This becomes null sometimes for some reason... investigate furtur
            if (theHttpContext.Session == null) {
                return null;
            }
            UserInformationModel<User> myUserInformationModel = (UserInformationModel<User>)theHttpContext.Session["UserInformation"];
            string myIpAddress = theHttpContext.Request.UserHostAddress;
            if (myUserInformationModel != null) {
                myUserInformationModel = UpdateUserOnlineStatus(myUserInformationModel, myIpAddress);
            }
            return myUserInformationModel;
        }

        private UserInformationModel<User> UpdateUserOnlineStatus(UserInformationModel<User> myUserInformationModel, string ipAddress) {
            try {
                if (!theWhoIsOnlineService.IsOnline(myUserInformationModel.Details, ipAddress)) {
                    theHttpContext.Session.Clear();
                    myUserInformationModel = null;
                }
            } catch {
                myUserInformationModel = null;
            }
            return myUserInformationModel;
        }

        public static UserInformation Instance() {
            return new UserInformation(new HttpContextWrapper(HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityHAVWhoIsOnlineRepository()));
        }

        public static UserInformation Instance(HttpContextBase aHttpBaseContext, IWhoIsOnlineService<User, WhoIsOnline> aWhoIsOnlineService) {
            return new UserInformation(aHttpBaseContext, aWhoIsOnlineService);
        }

        public bool AllowedToPerformAction(SocialPermission aPermission) {
            UserInformationModel<User> myUserInformationModel = (UserInformationModel<User>)theHttpContext.Session["UserInformation"];
            return PermissionHelper<User>.AllowedToPerformAction(myUserInformationModel, aPermission);
        }
    }
}