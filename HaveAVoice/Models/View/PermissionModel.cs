namespace HaveAVoice.Models.View {
    public class PermissionModel {
        public Permission Permission { get; private set; }

        public PermissionModel(Permission permission) {
            this.Permission = permission;
        }
    }
}