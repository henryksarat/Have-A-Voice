﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HaveAVoice.Models;
using HaveAVoice.Helpers;
using HaveAVoice.Models.View.Builders;

namespace HaveAVoice.Tests.Helpers {
    class PermissionTestHelper {
        public static void AddPermissionToUserInformation(UserInformationModelBuilder aBuilder, HAVPermission aPermission) {
            Permission myPermission = Permission.CreatePermission(0, aPermission.ToString(), string.Empty, false);
            List<Permission> myPermissions = new List<Permission>();
            myPermissions.Add(myPermission);
            aBuilder.AddPermissions(myPermissions);
        }
    }
}
