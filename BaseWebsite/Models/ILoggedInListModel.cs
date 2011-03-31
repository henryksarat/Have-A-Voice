using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Social.BaseWebsite.Models {
    public interface ILoggedInListModel<T> {
        IEnumerable<T> Get();
        void Set(IEnumerable<T> aModel);
    }
}
