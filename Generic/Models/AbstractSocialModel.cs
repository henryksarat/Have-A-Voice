using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Social.Generic.Models {
    public abstract class AbstractSocialModel<T> {
        public T Model { get; set; }
    }
}
