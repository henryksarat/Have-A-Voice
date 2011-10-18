using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace UniversityOfMe.Helpers {
    public static class BlogReader {
        public static string GetBlog() {
            string myHtml = string.Empty;
            int myTitleNumber = 1;
            bool myGrabTitle = false;
            bool myGrabLink = false;
            bool myGrabDate = false;
            string myCurrentTitle = string.Empty;
            string myCurrentLink = string.Empty;
            string myCurrentDate = string.Empty;
            try {
                XmlTextReader reader = new XmlTextReader("http://blog.universityof.me/rss");
                while (reader.Read()) {
                    switch (reader.NodeType) {
                        case XmlNodeType.Element: // The node is an element.
                            if (reader.Name.Equals("title")) {
                                if (myTitleNumber >= 2 && myTitleNumber <= 4) {
                                    myGrabTitle = true;
                                }

                                myTitleNumber++;
                            }

                            if (myTitleNumber >= 3 && myTitleNumber <= 7) {
                                if (reader.Name.Equals("link")) {
                                    myGrabLink = true;
                                } else if (reader.Name.Equals("pubDate")) {
                                    myGrabDate = true;
                                }
                            }

                            break;
                        case XmlNodeType.Text: //Display the text in each element.
                            if (myGrabTitle) {
                                myCurrentTitle = reader.Value;
                                myGrabTitle = false;
                            }

                            if (myGrabLink) {
                                myCurrentLink = reader.Value;
                                myGrabLink = false;
                            }

                            if (myGrabDate) {
                                DateTime myDateTime = Convert.ToDateTime(reader.Value);
                                myCurrentDate = String.Format("{0:MMM dd, yyyy}", myDateTime);
                                myGrabDate = false;
                            }

                            break;
                    }

                    if (!string.IsNullOrEmpty(myCurrentTitle) && !string.IsNullOrEmpty(myCurrentLink) && !string.IsNullOrEmpty(myCurrentDate)) {
                        myHtml += "<div><a class=\"main-page-blog\" href=\"" + myCurrentLink + "\">" + myCurrentTitle + "</a></div><div class=\"inner-blog\">" + myCurrentDate + "</div>";
                        myCurrentTitle = string.Empty;
                        myCurrentLink = string.Empty;
                        myCurrentDate = string.Empty;

                    }
                }
            } catch (Exception) {
                myHtml = "I'm sorry, currently we are unable to grab our blog. Please visit <a class=\"main-page-blog-read-more\" href=\"http://blog.universityof.me\">blog.universityof.me</a>.";
            }

            return myHtml;
        }
    }
}