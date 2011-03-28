using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Social.Generic.Models {
    public abstract class AbstractRolePermissionModel<T> : AbstractSocialModel<T> {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int PermissionId { get; set; }

        public abstract T FromModel();
    }
}
