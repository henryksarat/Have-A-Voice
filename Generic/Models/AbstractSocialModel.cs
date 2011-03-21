using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Social.Generic.Models {
    public interface AbstractSocialModel<T> {
        T FromModel();
    }
}
