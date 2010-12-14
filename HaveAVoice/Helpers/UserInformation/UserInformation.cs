using System.Web;
using HaveAVoice.Models;
using System.Collections;
using HaveAVoice.Models.View;
using System;
using HaveAVoice.Helpers.Enums;
using System.Collections.Generic;
using HaveAVoice.Models.Services.UserFeatures;

namespace HaveAVoice.Helpers.UserInformation {
    public class UserInformation : IUserInformation {
        private HttpContextBase theHttpContext;
        private IHAVUserService theUserService;

        private UserInformation() {
        }

        private UserInformation(HttpContextBase httpContext, IHAVUserService userService) {
            theHttpContext = httpContext;
            theUserService = userService;
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
                if (!theUserService.IsOnline(myUserInformationModel.Details, ipAddress)) {
                    theHttpContext.Session.Clear();
                    myUserInformationModel = null;
                }
            } catch {
                myUserInformationModel = null;
            }
            return myUserInformationModel;
        }

        public static UserInformation Instance() {
            return new UserInformation(new HttpContextWrapper(HttpContext.Current), new HAVUserService());
        }

        public static UserInformation Instance(HttpContextBase httpBaseContext, IHAVUserService userService) {
            return new UserInformation(httpBaseContext, userService);
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