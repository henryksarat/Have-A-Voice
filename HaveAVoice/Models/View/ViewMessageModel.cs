using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaveAVoice.Models.View {
    public class ViewMessageModel {
        public Message Message { get; set; }
        public string Reply { get; set; }

        public ViewMessageModel(Message message) {
            this.Message = message;
            this.Reply = string.Empty;
        }
    }
}
