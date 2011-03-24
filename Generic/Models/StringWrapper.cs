using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Social.Generic.Models {
    public class StringModel {
        public string Value { get ; private set; }

        public StringModel(string aString) {
            Value = aString;
        }
    }
}