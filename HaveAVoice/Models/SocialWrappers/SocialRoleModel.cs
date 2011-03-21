using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Social.Generic.Models;

namespace HaveAVoice.Models.SocialWrappers {
    public class SocialRoleModel : AbstractRoleModel<Role> {
        public SocialRoleModel(Role anExternal) {
            Id = anExternal.Id;
            Name = anExternal.Name;
            Description = anExternal.Description;
            DefaultRole = anExternal.DefaultRole;
            RestrictionId = anExternal.RestrictionId;
            Deleted = anExternal.Deleted;
        }

        public override Role FromModel() {
            return Role.CreateRole(Id, Name, Description, DefaultRole, RestrictionId, Deleted);
        }
    }
}