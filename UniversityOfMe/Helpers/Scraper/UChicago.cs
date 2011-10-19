using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using System.Text;
using UniversityOfMe.Repositories.Scraper;
using UniversityOfMe.Models;
using UniversityOfMe.UserInformation;

namespace UniversityOfMe.Helpers.Scraper {
    public static class UChicago {
        public static string DoScrape() {
            List<ClassStructure> myClassStructures = new List<ClassStructure>();
            List<string> myProfessors = new List<string>();

            string myEverything = GetWebPageHtml("http://timeschedules.uchicago.edu/");

            string myLinks = string.Empty;

            int myCurrentDtIndex = myEverything.IndexOf("<dt>");
            string myLink = string.Empty;
            while (myCurrentDtIndex > 0) {

                string myExtractedSafetyZone = myEverything.Substring(myCurrentDtIndex, 60);

                myExtractedSafetyZone = myExtractedSafetyZone.Remove(0, myExtractedSafetyZone.IndexOf('"') + 1);
                myExtractedSafetyZone = myExtractedSafetyZone.Remove(myExtractedSafetyZone.IndexOf('"'));

                if (!myExtractedSafetyZone.Equals("plaintex") && !myExtractedSafetyZone.Equals("plaintext")) {
                    myLinks += "http://timeschedules.uchicago.edu/" + myExtractedSafetyZone + "<br>";
                    string myConcatLink = "http://timeschedules.uchicago.edu/" + myExtractedSafetyZone;
                    string myChildPage = GetWebPageHtml(myConcatLink);
                    myChildPage = myChildPage.Replace("\n", "");

                    int myCourseNumStart = myExtractedSafetyZone.IndexOf("dept=") + 5;

                    myExtractedSafetyZone = myExtractedSafetyZone.Remove(0, myCourseNumStart);

                    int myCourseNumEnd = myExtractedSafetyZone.LastIndexOf("&term=44");
                    string myActualCourseNumber = myExtractedSafetyZone.Remove(myCourseNumEnd, myExtractedSafetyZone.Length-myCourseNumEnd);


                    int myCurrentCourseStartIndex = myChildPage.IndexOf(myActualCourseNumber + "&nb");

                    while (myCurrentCourseStartIndex > 0) {
                        ClassStructure myClassStructure = new ClassStructure();
                        string myChildExtractedSafetyZone = myChildPage.Substring(myCurrentCourseStartIndex, 60);
                        myChildExtractedSafetyZone = myChildExtractedSafetyZone.Replace("&nbsp;", "");
                        
                        int mySlashIndex = myChildExtractedSafetyZone.IndexOf('/');
                        myClassStructure.Subject = myChildExtractedSafetyZone.Substring(0, mySlashIndex);
                        myChildExtractedSafetyZone = myChildExtractedSafetyZone.Remove(0, mySlashIndex + 1);

                        int myHyphenIndex = myChildExtractedSafetyZone.IndexOf('-');
                        myClassStructure.Course = myChildExtractedSafetyZone.Substring(0, myHyphenIndex);
                        myChildExtractedSafetyZone = myChildExtractedSafetyZone.Remove(0, myHyphenIndex + 1);

                        int myNewlIneIndex = myChildExtractedSafetyZone.IndexOf("</span>");
                        myClassStructure.Section = myChildExtractedSafetyZone.Substring(0, myNewlIneIndex).Trim();


                        myChildPage = myChildPage.Remove(0, myCurrentCourseStartIndex + 60);

                        string myCourseTitle = string.Empty;
                        string myInstructor = string.Empty;
                        string myTime = string.Empty;

                        for (int i = 0; i < 8; i++) {
                            int myCourseTitleIndexStart = myChildPage.IndexOf("tdCourse");
                            int myCourseTitleIndexStartForEnd = myChildPage.IndexOf("ain\">");
                            int myFirstSpan = myChildPage.IndexOf("</span>");

                            if (i == 0) {
                                myClassStructure.Title = myChildPage.Substring(myCourseTitleIndexStartForEnd + 5, (myFirstSpan - myCourseTitleIndexStartForEnd - 5)).Trim();
                            } else if (i == 2) {
                                myClassStructure.Instructor = myChildPage.Substring(myCourseTitleIndexStartForEnd + 5, (myFirstSpan - myCourseTitleIndexStartForEnd - 5)).Trim();
                                if (!myProfessors.Contains(myClassStructure.Instructor)) {
                                    myProfessors.Add(myClassStructure.Instructor);
                                }
                            } else if (i == 3) {
                                myClassStructure.Time = myChildPage.Substring(myCourseTitleIndexStartForEnd + 5, (myFirstSpan - myCourseTitleIndexStartForEnd - 5)).Trim();
                            } else if (i == 7) {
                                myClassStructure.TotalEnrolled = myChildPage.Substring(myCourseTitleIndexStartForEnd + 5, (myFirstSpan - myCourseTitleIndexStartForEnd - 5)).Trim();
                            }

                            myChildPage = myChildPage.Remove(0, myFirstSpan + 5);
                        }



                        myCurrentCourseStartIndex = myChildPage.IndexOf(myActualCourseNumber + "&nb");
                        
                        if (!myClassStructure.Title.Equals("CANCELLED") && !myClassStructure.TotalEnrolled.Equals("0")) {
                            myClassStructures.Add(myClassStructure);
                        }
                    }
                }

                myEverything = myEverything.Remove(myCurrentDtIndex, 60);
                myCurrentDtIndex = myEverything.IndexOf("<dt>");
            }

            IScraperRepository myScraperRepo = new EntityScraperRepository();
            int myUserId = UserInformationFactory.GetUserInformation().Details.Id;

            foreach (ClassStructure myClassS in myClassStructures) {
                List<Professor> myLocalProfs = new List<Professor>();
                string[] myProfessorSplit = myClassS.Instructor.Replace("&#39;", "'").Split(';').Select(p => p.Trim()).ToArray();
                foreach (string mySingleProf in myProfessorSplit) {
                    int myFirstSpace = mySingleProf.LastIndexOf(' ');

                    string myFirstName = "UNKNOWN";
                    string myLastName = "PROFESSOR";

                    if (!string.IsNullOrEmpty(mySingleProf)) {
                        myFirstName = mySingleProf.Substring(myFirstSpace, mySingleProf.Length - myFirstSpace).Trim();
                        myLastName = mySingleProf.Substring(0, myFirstSpace + 1).Trim();
                    }

                    Professor myProf = null;
                    if (myScraperRepo.ProfessorExists("UChicago", myFirstName, myLastName)) {
                        myProf = myScraperRepo.GetProfessor("UChicago", myFirstName, myLastName);
                    } else {
                        myProf = myScraperRepo.CreateProfessor("UChicago", myUserId, myFirstName, myLastName);
                    }

                    myLocalProfs.Add(myProf);
                }

                Class myClass = null;
                if (myScraperRepo.ClassExists("UChicago", myClassS.Subject.Trim(), myClassS.Course.Trim())) {
                    myClass = myScraperRepo.GetClass("UChicago", myClassS.Subject.Trim(), myClassS.Course.Trim());
                } else {
                    myClass = myScraperRepo.CreateClass("UChicago", myUserId, myClassS.Subject.Trim(), myClassS.Course.Trim(), myClassS.Title.Trim());
                }

                foreach (Professor myLocalProf in myLocalProfs) {
                    myScraperRepo.CreateClassProfessor(myLocalProf.Id, myClass.Id);
                }
            }

            return myClassStructures.ToString();
        }

        private static string GetWebPageHtml(string anWebPage) {
            StringBuilder sb = new StringBuilder();

            // used on each read operation
            byte[] buf = new byte[8192];

            // prepare the web page we will be asking for
            HttpWebRequest request = (HttpWebRequest)
                WebRequest.Create(anWebPage);

            // execute the request
            HttpWebResponse response = (HttpWebResponse)
                request.GetResponse();

            // we will read data via the response stream
            Stream resStream = response.GetResponseStream();

            string tempString = null;
            int count = 0;

            do {
                // fill the buffer with data
                count = resStream.Read(buf, 0, buf.Length);

                // make sure we read some data
                if (count != 0) {
                    // translate from bytes to ASCII text
                    tempString = Encoding.ASCII.GetString(buf, 0, count);

                    // continue building the string
                    sb.Append(tempString);
                }
            }
            while (count > 0); // any more data to read?

            return sb.ToString();
        }

        private class ClassStructure {
            public string Subject { get; set; }
            public string Course { get; set; }
            public string Section { get; set; }
            public string Title { get; set; }
            public string Instructor { get; set; }
            public string Time { get; set; }
            public string TotalEnrolled { get; set; }
        }
    }
}