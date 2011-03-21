using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Social.Generic.Models {
    public abstract class AbstractPermissionModel<T> : AbstractSocialModel<T> {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Deleted { get; set; }

        public abstract T FromModel();
    }
}
