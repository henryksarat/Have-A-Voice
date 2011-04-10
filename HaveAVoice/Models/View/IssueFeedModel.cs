using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Helpers.Enums;

namespace HaveAVoice.Models.View {
    public class IssueFeedModel : FeedModel {
        public Issue Issue { get; set; }
        public PersonFilter PersonFilter { get; set; }

        public IssueFeedModel(User aUser) : base(aUser) { }
    }
}