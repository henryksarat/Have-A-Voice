using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UniversityOfMe.Models.View {
    public class PhotoDisplayView {
        public Photo Photo { get; set; }
        public Photo NextPhoto { get; set; }
        public Photo PreviousPhoto { get; set; }
    }
}