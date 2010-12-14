using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaveAVoice.Models.View {
    public class MessageModel {
        public SiteSectionsEnum Origin { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
    }
}
