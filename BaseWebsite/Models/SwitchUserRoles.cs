using System.Collections.Generic;
using System.Web.Mvc;
using Social.Generic;

namespace BaseWebsite.Models {
    public class SwitchUserRoles<T, U> {
        public IEnumerable<Pair<T, bool>> Users { get; set; }
        public SelectList CurrentRoles { get; set; }
        public SelectList MoveToRoles { get; set; }
        public int SelectedCurrentRoleId { get; set; }
        public int SelectedMoveToRoleId { get; set; }

        public SwitchUserRoles() {
            Users = new List<Pair<T, bool>>();
        }
    }
}