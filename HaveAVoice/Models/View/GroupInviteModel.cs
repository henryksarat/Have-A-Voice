using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaveAVoice.Models.View {
    public class GroupInviteModel {
        public Group Group { get; set; }
        public IEnumerable<User> Users { get; set; }
        public int[] SelectedUsers { get; set; }

        public GroupInviteModel() {
            SelectedUsers = new int[0];
        }
    }
}