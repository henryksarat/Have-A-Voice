using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Social.User.Models;

namespace Social.User.Helpers {
    public class NameHelper<U> {
        public static string FullName(AbstractUserModel<U> aUser) {
            return aUser.FirstName + " " + aUser.LastName;
        }
    }
}
