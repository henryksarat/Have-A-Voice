using Social.Generic.Models;

namespace UniversityOfMe.Models.SocialModels {
    public class SocialPermissionModel : AbstractPermissionModel<Permission> {
        public static SocialPermissionModel Create(Permission anExternal) {
            if (anExternal != null) {
                return new SocialPermissionModel(anExternal);
            }
            return null;
        }

        public SocialPermissionModel() { }

        public override Permission FromModel() {
            return Permission.CreatePermission(Id, Name, Description, Deleted);
        }

        private SocialPermissionModel(Permission anExternal) {
            Id = anExternal.Id;
            Name = anExternal.Name;
            Description = anExternal.Description;
            Deleted = anExternal.Deleted;
        }
    }
}