using System.Web.Mvc;

namespace Social.ViewHelpers {
    public class CheckBoxHelper {
        public static string StandardCheckbox(string aName, string aValue, bool aIsChecked) {
            var checkboxTag = new TagBuilder("input");
            checkboxTag.MergeAttribute("type", "checkbox");
            checkboxTag.MergeAttribute("name", aName);
            checkboxTag.MergeAttribute("value", aValue);

            if (aIsChecked) {
                checkboxTag.MergeAttribute("checked", "checked");
            }
            return checkboxTag.ToString();
        }
    }
}
