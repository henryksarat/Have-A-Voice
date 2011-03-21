using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Social.Generic.Models {
    public abstract class AbstractRoleModel<T> : AbstractSocialModel<T> {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool DefaultRole { get; set; }
        public int RestrictionId { get; set; }
        public bool Deleted { get; set; }

        public abstract T FromModel();
    }
}
