using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UniversityOfMe.Helpers.Format {
    public static class MoneyFormatHelper {
        public static string Format(double anAmount) {
            return String.Format("{0:C}", anAmount);
        }
    }
}