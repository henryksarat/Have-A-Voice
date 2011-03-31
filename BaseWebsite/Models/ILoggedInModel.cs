using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Social.BaseWebsite.Models {
    public interface ILoggedInModel<T> {
        T Get();
        void Set(T aModel);
    }
}
