using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;
using System.Collections;
using HaveAVoice.Helpers.Enums;
using HaveAVoice.Helpers;
using HaveAVoice.Models.View;
using Social.Generic.Models;

namespace HaveAVoice.Helpers.UserInformation {
    public class HAVUserInformationFactory {
        private static IUserInformation theFactory;
        
        private HAVUserInformationFactory() { }

        public static UserInformationModel<User> GetUserInformation() {
            SetDefaultInstance();
            return theFactory.GetUserInformaton();
        }

        public static bool IsLoggedIn() {
            return GetUserInformation() != null;
        }

        public static void Dispose() {
            Dispose(UserInformation.Instance());
        }

        public static void SetInstance(IUserInformation userInformation) {
            theFactory = userInformation;
        }

        public static void Dispose(IUserInformation userInformation) {
            theFactory = userInformation;
        }

        private static void SetDefaultInstance() {
            if (theFactory == null) {
                theFactory = UserInformation.Instance();
            }
        }
    }
}
