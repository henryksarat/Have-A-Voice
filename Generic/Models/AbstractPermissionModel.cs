using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Social.Generic.Models {
    public abstract class AbstractPermissionModel<T> : AbstractSocialModel<T> {
        public int Id { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Name { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Description { get; set; }
        public bool Deleted { get; set; }

        public abstract T FromModel();
    }
}
