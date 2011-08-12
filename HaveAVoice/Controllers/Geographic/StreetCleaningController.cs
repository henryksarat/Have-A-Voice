using System;
using System.Web.Mvc;
using System.Xml;
using Social.Generic;
using Social.Generic.ActionFilters;
using HaveAVoice.Controllers.Helpers;
using Social.Validation;

namespace HaveAVoice.Controllers.Geographic {
    public class StreetCleaningController : Controller {
        private IValidationDictionary theValidationDictionary;

        public StreetCleaningController() {
            theValidationDictionary = new ModelStateWrapper(this.ModelState);
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult Index() {
            return View("Index");
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        public ActionResult Index(string northStreet, string southStreet, string westStreet, string eastStreet, string address, string city, string state) {
            Pair<double, double> myNW = GetLatLong(northStreet + " and " + westStreet, city, state);
            Pair<double, double> myNE = GetLatLong(northStreet + " and " + eastStreet, city, state);
            Pair<double, double> mySW = GetLatLong(southStreet + " and " + westStreet, city, state);
            Pair<double, double> mySE = GetLatLong(southStreet + " and " + eastStreet, city, state);

            Pair<double, double> myAddress = GetLatLong(address, city, state);
            
            bool myInArea = false;

            if (myAddress.First < myNW.First && myAddress.First < myNE.First && myAddress.First > mySW.First && myAddress.First > mySE.First
                && myAddress.Second > myNW.Second && myAddress.Second < myNE.Second && myAddress.Second > mySW.Second && myAddress.Second < mySE.Second) {
                    myInArea = true;
            }

            if (myInArea) {
                TempData["Message"] += MessageHelper.SuccessMessage("It is within the region!");
            } else {
                TempData["Message"] += MessageHelper.NormalMessage("It is NOT within the region.");
            }

            theValidationDictionary.ForceModleStateExport();
            return RedirectToAction("Index");
        }

        private Pair<double, double> GetLatLong(string aStreetOne, string aCity, string aState) {
            XmlTextReader reader = new XmlTextReader("http://maps.googleapis.com/maps/api/geocode/xml?address=" +aStreetOne + ", " + aCity + ", " + aState + "&sensor=false");
            bool myGeometryStart = false;
            bool myLocationStart = false;
            bool myLatStart = false;
            bool myLongStart = false;
            double myLong = 0;
            double myLat = 0;

            while (reader.Read()) {
                switch (reader.NodeType) {
                    case XmlNodeType.Element: // The node is an element.
                        if (reader.Name.Equals("geometry")) {
                            myGeometryStart = true;
                        }

                        if (myGeometryStart) {
                            if (reader.Name.Equals("location")) {
                                myLocationStart = true;
                                myGeometryStart = false;
                            }
                        }

                        if (myLocationStart) {
                            if (reader.Name.Equals("lat")) {
                                myLatStart = true;
                                myLocationStart = false;
                            }
                        }

                        if (myLatStart) {
                            if (reader.Name.Equals("lng")) {
                                myLongStart = true;
                                myLatStart = false;
                            }
                        }

                        break;
                    case XmlNodeType.Text: //Display the text in each element.
                        if (myLatStart) {
                            myLat = Double.Parse(reader.Value);
                        }

                        if (myLongStart) {
                            myLong = Double.Parse(reader.Value);
                            myLongStart = false;
                        }
                        break;
                    case XmlNodeType.EndElement: //Display the end of the element.
                        Console.Write("</" + reader.Name);
                        Console.WriteLine(">");
                        break;
                }
            }

            return new Pair<double, double>() {
                First = myLat,
                Second = myLong
            };
        }
    }
}
