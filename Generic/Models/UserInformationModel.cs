using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using Social.Generic.Models;
using Social.Generic.Helpers;

namespace Social.Generic.Models {
    [Serializable]
    public class UserInformationModel<T> {
        public int UserId { get; set; }
        public T Details { get; set; }
        public IEnumerable<SocialPermission> Permissions { get; set; }
        public IEnumerable<SocialPrivacySetting> PrivacySettings { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string FullName { get; set; }

        public UserInformationModel(T aUser, int aUserId) {
            this.Details = aUser;
            UserId = aUserId;
            PrivacySettings = new List<SocialPrivacySetting>();
            Permissions = new List<SocialPermission>();
        }
    }
}
