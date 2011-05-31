using System.Web;
using Social.Generic.Models;
using Social.Users.Services;

namespace Social.Authentication {
    public class UserInformation<T, U> : IUserInformation<T, U> {
        private HttpContextBase theHttpContext;
        private IWhoIsOnlineService<T, U> theWhoIsOnlineService;

        private UserInformation() {
        }

        private UserInformation(HttpContextBase aHttpBaseContext, IWhoIsOnlineService<T, U> aWhoIsOnlineService) {
            theHttpContext = aHttpBaseContext;
            theWhoIsOnlineService = aWhoIsOnlineService;
        }

        public bool IsLoggedIn() {
            return GetUserInformaton() != null;
        }

        public UserInformationModel<T> GetUserInformaton() {
            //This becomes null sometimes for some reason... investigate furthur
            if (theHttpContext.Session == null) {
                return null;
            }
            
            UserInformationModel<T> myUserInformationModel = (UserInformationModel<T>)theHttpContext.Session["UserInformation"];
            theHttpContext.Session["UserInformation"] = myUserInformationModel;
            string myIpAddress = theHttpContext.Request.UserHostAddress;
            if (myUserInformationModel != null) {
                myUserInformationModel = UpdateUserOnlineStatus(myUserInformationModel, myIpAddress);
            }
            return myUserInformationModel;
        }

        private UserInformationModel<T> UpdateUserOnlineStatus(UserInformationModel<T> myUserInformationModel, string ipAddress) {
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

        public static UserInformation<T, U> Instance(HttpContextBase aHttpBaseContext, IWhoIsOnlineService<T, U> aWhoIsOnlineService) {
            return new UserInformation<T, U>(aHttpBaseContext, aWhoIsOnlineService);
        }
    }
}