using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaveAVoice.Models.View {
    public class SelectionModel {
        public Object Item { get; set; }
        public bool Selected { get; set; }

        public SelectionModel(Object item, bool selected) {
            this.Item = item;
            this.Selected = selected;
        }
    }
}
