using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UniversityOfMe.Models.View {
    public class FriendListModel {
        public IEnumerable<Friend> Friends { get; set; }
        public User User { get; set; }
    }
}