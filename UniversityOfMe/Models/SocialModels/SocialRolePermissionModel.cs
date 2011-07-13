using Social.Generic.Models;

namespace UniversityOfMe.Models.SocialModels {
    public class SocialRolePermissionModel : AbstractRolePermissionModel<RolePermission> {
        public static SocialRolePermissionModel Create(RolePermission anExternal) {
            return new SocialRolePermissionModel(anExternal);
        }

        public override RolePermission FromModel() {
            return RolePermission.CreateRolePermission(Id, RoleId, PermissionId);
        }

        private SocialRolePermissionModel(RolePermission anExternal) {
            Id = anExternal.Id;
            RoleId = anExternal.RoleId;
            PermissionId = anExternal.PermissionId;
        }
    }
}