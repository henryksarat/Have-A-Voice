using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaveAVoice.Models.View {
    public class NotLoggedInModel {
        public IEnumerable<Issue> MostPopular { get; set; }
        public IEnumerable<Issue> Newest { get; set; }

        public NotLoggedInModel() {
            MostPopular = new List<Issue>();
            Newest = new List<Issue>();
        }
    }
}