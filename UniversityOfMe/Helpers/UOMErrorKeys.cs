using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UniversityOfMe.Helpers {
    public static class UOMErrorKeys {
        public const string NOT_FROM_UNVIERSITY = "You are not associated with this university and therefore can't view that.";
        public const string ACADEMIC_TERMS_GET_ERROR = "Error retrieving the academic terms. Please refresh the page.";
    }
}