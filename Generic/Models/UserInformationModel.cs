﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using Social.Generic.Models;
using Social.Generic.Helpers;

namespace Social.Generic.Models {
    public class UserInformationModel<T> {
        public T Details { get; set; }
        public IEnumerable<SocialPermission> Permissions { get; set; }
        public IEnumerable<SocialPrivacySetting> PrivacySettings { get; set; }
        public Hashtable PermissionToRestriction { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string FullName { get; set; }

        public UserInformationModel(T aUser) {
            this.Details = aUser;
            PrivacySettings = new List<SocialPrivacySetting>();
            Permissions = new List<SocialPermission>();
            PermissionToRestriction = new Hashtable();
        }
    }
}
