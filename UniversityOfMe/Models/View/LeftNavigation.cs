using System.Collections.Generic;

namespace UniversityOfMe.Models.View {
    public class LeftNavigation {
        public User User { get; set; }
        public IEnumerable<NotificationModel> Notifications { get; set; }
        public IEnumerable<User> NewestUsersInUniversity { get; set; }
        public User DatingMember { get; set; }
        public DatingLog DatingMatchMember { get; set; }

        public bool HasDatingMatch() {
            return DatingMatchMember != null;
        }

        public bool HasDatingMember() {
            return DatingMember != null;
        }
    }
}