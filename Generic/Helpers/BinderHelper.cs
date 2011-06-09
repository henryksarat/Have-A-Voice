using System;
using System.Web.Mvc;

namespace Social.Generic.Helpers {
    public class BinderHelper {
        public static string GetA(ModelBindingContext bindingContext, string key) {
            if (String.IsNullOrEmpty(key)) return null;
            ValueProviderResult valueResult;
            //Try it with the prefix...
            valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName + "." + key);
            //Didn't work? Try without the prefix if needed...
            if (valueResult == null && bindingContext.FallbackToEmptyPrefix == true) {
                valueResult = bindingContext.ValueProvider.GetValue(key);
            }

            if (valueResult == null) {
                return "";
            }
            return valueResult.AttemptedValue;
        }

        public static long GetALong(ModelBindingContext bindingContext, string key) {
            string value = GetA(bindingContext, key);
            if (String.IsNullOrEmpty(value)) return 0L;
            long longValue;
            bool result = Int64.TryParse(value, out longValue);

            if (result) {
                return longValue;
            } else {
                return 0L;
            }
        }

        public static int GetAInt(ModelBindingContext bindingContext, string key) {
            string value = GetA(bindingContext, key);
            if (String.IsNullOrEmpty(value)) return 0;
            int intValue;
            bool result = Int32.TryParse(value, out intValue);

            if (result) {
                return intValue;
            } else {
                return 0;
            }
        }

        public static bool GetABoolean(ModelBindingContext bindingContext, string key) {
            string value = GetA(bindingContext, key);
            if (String.IsNullOrEmpty(value)) return false;
            return Boolean.Parse(value);
        }
    }
}
