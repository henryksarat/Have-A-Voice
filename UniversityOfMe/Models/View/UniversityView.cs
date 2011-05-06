using System.Collections.Generic;

namespace UniversityOfMe.Models.View {
    public class UniversityView {
        public University University { get; set; }
        public User DatingMember { get; set; }
        public DatingLog DatingMatchMember { get; set; }
        public IEnumerable<Professor> Professors { get; set; }
        public IEnumerable<Class> Classes { get; set; }
        public IEnumerable<Event> Events { get; set; }
        public IEnumerable<TextBook> TextBooks { get; set; }
        public IEnumerable<Club> Organizations { get; set; }
        public IEnumerable<GeneralPosting> GeneralPostings { get; set; }
        public IEnumerable<User> NewestUsers { get; set; }
        public IEnumerable<NotificationModel> Notifications { get; set; }


        public bool HasDatingMatch() {
            return DatingMatchMember != null;
        }

        public bool HasDatingMember() {
            return DatingMember != null;
        }
    }
}