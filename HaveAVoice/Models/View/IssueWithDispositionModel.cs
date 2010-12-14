using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaveAVoice.Models.View {
    public class IssueWithDispositionModel {
        public Issue Issue { get; set; }
        public bool HasDisposition { get; set; }
    }
}