using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Social.Generic.Helpers;
using UniversityOfMe.Models;
using Social.Generic.Models;

namespace UniversityOfMe.Helpers.Privacy {
    public class PrivacyStrategyImpl : IPrivacyStrategy<User> {
        public bool IsAllowed(User aPrivacyUser, PrivacyAction aPrivacyAction, UserInformationModel<User> aViewingUser) {
            return PrivacyHelper.IsAllowed(aPrivacyUser, aPrivacyAction, aViewingUser);
        }
    }
}