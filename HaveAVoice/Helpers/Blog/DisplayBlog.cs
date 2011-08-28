using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace HaveAVoice.Helpers.Blog {
    public static class DisplayBlog {
        public static string DisplayOurBlog() {
            String URLString = "http://haveavoiceblog.wordpress.com/feed/";
            XmlTextReader reader = new XmlTextReader(URLString);

            while (reader.Read()) {
                switch (reader.NodeType) {
                    case XmlNodeType.Element: // The node is an element.
                        Console.Write("<" + reader.Name);

                        while (reader.MoveToNextAttribute()) // Read the attributes.
                            Console.Write(" " + reader.Name + "='" + reader.Value + "'");
                        Console.Write(">");
                        Console.WriteLine(">");
                        break;
                    case XmlNodeType.Text: //Display the text in each element.
                        Console.WriteLine(reader.Value);
                        break;
                    case XmlNodeType.EndElement: //Display the end of the element.
                        Console.Write("</" + reader.Name);
                        Console.WriteLine(">");
                        break;
                }
            }

            return string.Empty;
        }
    }
}