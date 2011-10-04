using System.Collections;
using System.Collections.Generic;

namespace UniversityOfMe.Models.View {
    public class ClassDetailsModel {
        public Class Class { get; set; }
        public IEnumerable<Professor> Professors { get; set; }
    }
}