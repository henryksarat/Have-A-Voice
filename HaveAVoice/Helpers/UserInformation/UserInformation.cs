using System.Web;
using HaveAVoice.Models;
using System.Collections;
using HaveAVoice.Models.View;
using System;
using HaveAVoice.Helpers.Enums;
using System.Collections.Generic;
using HaveAVoice.Services.UserFeatures;

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

        public UserInformationModel GetUserInformaton() {
            UserInformationModel myUserInformationModel = (UserInformationModel)theHttpContext.Session["UserInformation"];
            string myIpAddress = theHttpContext.Request.UserHostAddress;
            if (myUserInformationModel != null) {
                myUserInformationModel = UpdateUserOnlineStatus(myUserInformationModel, myIpAddress);
            }
            return myUserInformationModel;
        }

        private UserInformationModel UpdateUserOnlineStatus(UserInformationModel myUserInformationModel, string ipAddress) {
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

        public bool AllowedToPerformAction(HAVPermission aPermission) {
            UserInformationModel myUserInformationModel = (UserInformationModel)theHttpContext.Session["UserInformation"];
            return HAVPermissionHelper.AllowedToPerformAction(myUserInformationModel, aPermission);
        }

        public bool HasPrivacySettingEnabled(PrivacySetting aPrivacySetting) {
            UserInformationModel myUserInformationModel = (UserInformationModel)theHttpContext.Session["UserInformation"];
            return myUserInformationModel != null && (bool)myUserInformationModel.PrivacySettings[aPrivacySetting];
        }
    }
}