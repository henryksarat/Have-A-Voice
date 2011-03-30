using System.Collections.Generic;
using Social.Generic;

namespace BaseWebsite.Models {
    public abstract class AbstractRoleViewModel<T, U> {
        public T Role { get; set; }
        public List<int> SelectedPermissionsIds { get; set; }
        public IEnumerable<U> AllPermissions { get; set; }

        public AbstractRoleViewModel(T role) {
            Role = role;
            SelectedPermissionsIds = new List<int>();
            AllPermissions = new List<U>();            
        }

        public abstract List<Pair<U, bool>> PermissionSelection();
    }
}
