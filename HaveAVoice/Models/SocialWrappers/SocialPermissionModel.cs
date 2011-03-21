using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Social.Generic.Models;

namespace HaveAVoice.Models.SocialWrappers {
    public class SocialPermissionModel : AbstractPermissionModel<Permission> {
        public SocialPermissionModel(Permission anExternal) {
            Name = anExternal.Name;
            Description = anExternal.Description;
            Deleted = anExternal.Deleted;
        }

        public override Permission FromModel() {
            return Permission.CreatePermission(0, Name, Description, Deleted);
        }
    }
}