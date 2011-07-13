using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Social.Generic.Models;

namespace UniversityOfMe.Models.SocialModels {
    public class SocialRoleModel : AbstractRoleModel<Role> {
        public static SocialRoleModel Create(Role anExternal) {
            if (anExternal != null) {
                return new SocialRoleModel(anExternal);
            }
            return null;
        }

        public override Role FromModel() {
            return Role.CreateRole(Id, Name, Description, DefaultRole, Deleted);
        }

        private SocialRoleModel(Role anExternal) {
            Id = anExternal.Id;
            Name = anExternal.Name;
            Description = anExternal.Description;
            DefaultRole = anExternal.DefaultRole;
            Deleted = anExternal.Deleted;
        }
    }
}