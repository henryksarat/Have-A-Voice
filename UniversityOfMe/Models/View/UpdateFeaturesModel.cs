using System.Collections.Generic;
using Social.Generic;

namespace UniversityOfMe.Models.View {
    public class UpdateFeaturesModel {
        public IEnumerable<Pair<Feature, bool>> Features { get; set; }
    }
}