using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaveAVoice.Models.View {
    public class AuthorityVerificationModel {
        public string Token { get; set; }
        public string AuthorityType { get; set; }
        public string AuthorityPosition { get; set; }
    }
}