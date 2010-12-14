using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaveAVoice.Models.View {
    public class StringWrapper {
        public string Value { get ; private set; }

        public StringWrapper(string aString) {
            Value = aString;
        }
    }
}