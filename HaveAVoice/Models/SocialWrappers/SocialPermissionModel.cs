using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Social.Generic.Models;

namespace HaveAVoice.Models.SocialWrappers {
    public class SocialPermissionModel : AbstractPermissionModel<Permission> {
        public SocialPermissionModel() { }

        public SocialPermissionModel(Permission anExternal) {
            Id = anExternal.Id;
            Name = anExternal.Name;
            Description = anExternal.Description;
            Deleted = anExternal.Deleted;
        }

        public override Permission FromModel() {
            return Permission.CreatePermission(Id, Name, Description, Deleted);
        }
    }
}