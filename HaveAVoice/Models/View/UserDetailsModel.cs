using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaveAVoice.Models.View {
    public class UserDetailsModel {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public bool CanListen { get; set; }
        public bool CanMessage { get; set; }
    }
}
