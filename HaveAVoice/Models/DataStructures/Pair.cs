using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaveAVoice.Models.DataStructures {
    public class Pair<T> {
        public T Item { get; set; }
        public bool Selected { get; set; }
    }
}