using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Social.Generic.Models;

namespace Social.Generic.Helpers {
    public interface IPrivacyStrategy<T> {
        bool IsAllowed(T aPrivacyUser, PrivacyAction aPrivacyAction, UserInformationModel<T> aViewingUser);
    }
}
