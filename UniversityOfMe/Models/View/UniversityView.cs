using System.Collections.Generic;

namespace UniversityOfMe.Models.View {
    public class UniversityView {
        public University University { get; set; }
        public IEnumerable<TextBook> TextBooks { get; set; }
        public IEnumerable<MarketplaceItem> MarketplaceItems { get; set;}
    }
}