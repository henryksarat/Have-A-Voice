
using System.Web.Mvc;
namespace UniversityOfMe.Helpers {
    public class MetaHelper {
        public const string DEFAULT_START = "University of Me | ";
        public const string DEFAULT_DESCRIPTION = "University of Me | Campus Dating, Flirting, Buy/Sell Textbooks, Class Help, Professor Reviews, University Events";
        public const string DEFAULT_KEYWORDS = "College Only Social Networking, Buy Textbooks, Sell Textbooks, Professor Reviews, Class Help, College Events, College Dating, Anonymous Flirting";

        public static string MetaDescription(string aDescription) {
            var myDescription = new TagBuilder("meta");
            myDescription.MergeAttribute("name", "description");
            myDescription.MergeAttribute("content", DEFAULT_START + aDescription);
            return myDescription.ToString();
        }

        public static string MetaDescription() {
            var myDescription = new TagBuilder("meta");
            myDescription.MergeAttribute("name", "description");
            myDescription.MergeAttribute("content", DEFAULT_DESCRIPTION);
            return myDescription.ToString();
        }

        public static string MetaKeywords(string aKeywords) {
            var myDescription = new TagBuilder("meta");
            myDescription.MergeAttribute("name", "keywords");
            myDescription.MergeAttribute("content", aKeywords + ", " + DEFAULT_KEYWORDS);
            return myDescription.ToString();
        }

        public static string MetaKeywords() {
            var myDescription = new TagBuilder("meta");
            myDescription.MergeAttribute("name", "keywords");
            myDescription.MergeAttribute("content", DEFAULT_KEYWORDS);
            return myDescription.ToString();
        }
    }
}