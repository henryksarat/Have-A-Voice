using System.Collections.Generic;

namespace UniversityOfMe.Models.View {
    public class LeftNavigation {
        public User User { get; set; }
        public IEnumerable<ItemType> ItemTypes { get; set; }
        public bool HasProfilePicture { get; set; }
        public bool IsLoggedIn { get; set; }
    }
}