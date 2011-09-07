using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UniversityOfMe.Models.View {
    public class SomethingListWithUser<T> {
        public User TargetUser { get; set; }
        public IEnumerable<T> ListedItems { get; set;}
    }
}