using System.Web;
using Social.Authentication.Helpers;
using Social.Generic.Models;
using Social.Users.Services;
using System;
using System.Web.Security;

namespace Social.Authentication {
    public class UserInformation<T, U> : IUserInformation<T, U> {
        private HttpContextBase theHttpContext;
        private IWhoIsOnlineService<T, U> theWhoIsOnlineService;
        private IGetUserStrategy<T> theGetUserStrategy;
        private UserInformationModel<T> theUserInformationModel;

        private UserInformation() {
        }

        private UserInformation(HttpContextBase aHttpBaseContext, IWhoIsOnlineService<T, U> aWhoIsOnlineService, IGetUserStrategy<T> aGetUserStrategy) {
            theHttpContext = aHttpBaseContext;
            theWhoIsOnlineService = aWhoIsOnlineService;
            theGetUserStrategy = aGetUserStrategy;
        }

        public bool IsLoggedIn() {
            return theHttpContext.User.Identity.IsAuthenticated;
        }

        public UserInformationModel<T> GetUserInformaton() {
            int myResult;
            int.TryParse(theHttpContext.User.Identity.Name, out myResult);
            if (theUserInformationModel == null) {
                 theUserInformationModel = theGetUserStrategy.GetUserInformationModel(myResult);
            }
            /*
            string myIpAddress = theHttpContext.Request.UserHostAddress;
            if (theUserInformationModel != null && !theAlreadyLoggedTime) {
                theUserInformationModel = UpdateUserOnlineStatus(theUserInformationModel, myIpAddress);
                theAlreadyLoggedTime = true;
            }*/
            return theUserInformationModel;
        }

        public UserInformationModel<T> GetUserInformaton(int anId) {
            if (theUserInformationModel == null) {
                theUserInformationModel = theGetUserStrategy.GetUserInformationModel(anId);
            }

            return theUserInformationModel;
        }



        private UserInformationModel<T> UpdateUserOnlineStatus(UserInformationModel<T> myUserInformationModel, string ipAddress) {
            try {
                if (!theWhoIsOnlineService.IsOnline(myUserInformationModel.Details, ipAddress)) {
                    theHttpContext.Session.Clear();
                    //myUserInformationModel = null;
                    FormsAuthentication.SignOut();
                }
            } catch {
                myUserInformationModel = null;
            }
            return myUserInformationModel;
        }

        public static UserInformation<T, U> Instance(HttpContextBase aHttpBaseContext, IWhoIsOnlineService<T, U> aWhoIsOnlineService, IGetUserStrategy<T> aGetUserStrategy) {
            return new UserInformation<T, U>(aHttpBaseContext, aWhoIsOnlineService, aGetUserStrategy);
        }


        public void ForceUserInformationClear() {
            theUserInformationModel = null;
        }


        public string GetIdentityName() {
            int myResult;
            int.TryParse(theHttpContext.User.Identity.Name, out myResult);
            return myResult.ToString();
        }
    }
}