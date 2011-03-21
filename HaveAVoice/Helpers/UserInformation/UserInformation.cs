using System.Web;
using HaveAVoice.Models;
using System.Collections;
using HaveAVoice.Models.View;
using System;
using HaveAVoice.Helpers.Enums;
using System.Collections.Generic;
using HaveAVoice.Services.UserFeatures;
using Social.Generic.Models;
using Social.Generic.Helpers;
using Social.Admin.Helpers;

namespace HaveAVoice.Helpers.UserInformation {
    public class UserInformation : IUserInformation {
        private HttpContextBase theHttpContext;
        private IHAVWhoIsOnlineService theWhoIsOnlineService;

        private UserInformation() {
        }

        private UserInformation(HttpContextBase aHttpBaseContext, IHAVWhoIsOnlineService aWhoIsOnlineService) {
            theHttpContext = aHttpBaseContext;
            theWhoIsOnlineService = aWhoIsOnlineService;
        }

        public UserInformationModel<User> GetUserInformaton() {
            //This becomes null sometimes for some reason... investigate furthur
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
            return new UserInformation(new HttpContextWrapper(HttpContext.Current), new HAVWhoIsOnlineService());
        }

        public static UserInformation Instance(HttpContextBase aHttpBaseContext, IHAVWhoIsOnlineService aWhoIsOnlineService) {
            return new UserInformation(aHttpBaseContext, aWhoIsOnlineService);
        }

        public bool AllowedToPerformAction(SocialPermission aPermission) {
            UserInformationModel<User> myUserInformationModel = (UserInformationModel<User>)theHttpContext.Session["UserInformation"];
            return PermissionHelper<User>.AllowedToPerformAction(myUserInformationModel, aPermission);
        }
    }
}