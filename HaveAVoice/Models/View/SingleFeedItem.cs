using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Helpers.Enums;

namespace HaveAVoice.Models.View {
    public class SingleFeedItem {
        public FeedItem FeedItem { get; set; }
        public Object Item { get; set; }
    }
}