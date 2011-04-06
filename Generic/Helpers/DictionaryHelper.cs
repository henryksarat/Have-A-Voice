using System.Collections.Generic;

namespace Social.Generic.Helpers {
    public static class DictionaryHelper {
        public static IDictionary<string, string> DictionaryWithSelect() {
            IDictionary<string, string> myDictionary = new Dictionary<string, string>();
            myDictionary.Add(Constants.Constants.SELECT, Constants.Constants.SELECT);
            return myDictionary;
        }
    }
}
