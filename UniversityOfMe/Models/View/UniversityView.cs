using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UniversityOfMe.Models.View {
    public class UniversityView {
        public University University { get; set; }
        public IEnumerable<Professor> Professors { get; set; }
        public IEnumerable<Class> Classes { get; set; }
        public IEnumerable<Event> Events { get; set; }
        public IEnumerable<TextBook> TextBooks { get; set; }
        public IEnumerable<Club> Organizations { get; set; }
        public IEnumerable<GeneralPosting> GeneralPostings { get; set; }
        public IEnumerable<User> NewestUsers { get; set; }
    }
}