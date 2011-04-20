using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UniversityOfMe.Models;
using Social.Validation;
using UniversityOfMe.Services;

namespace UniversityOfMe.Helpers {
    public static class UniversityHelper {
        public static bool IsFromUniversity(User aFrom, string aUniversityId) {
            if (aUniversityId == null) {
                return false;
            }

            bool myIsFromUniversity = false;
            foreach (UserUniversity myUserUniversity in aFrom.UserUniversities) {
                if (myUserUniversity.UniversityEmail.UniversityId.ToUpper().Equals(aUniversityId.ToUpper())) {
                    myIsFromUniversity = true;
                    break;
                }
            }
            return myIsFromUniversity;
        }

        public static string GetMainUniversity(User aUser) {
            foreach (UserUniversity myUserUniversity in aUser.UserUniversities) {
                return myUserUniversity.UniversityEmail.UniversityId;
            }

            return null;
        }

        public static IEnumerable<string> GetUniversityAffiliations(User aUser) {
            List<string> myUniversityAffiliations = new List<string>();
            foreach (UserUniversity myUserUniversity in aUser.UserUniversities) {
                myUniversityAffiliations.Add(myUserUniversity.UniversityEmail.UniversityId);
            }

            return myUniversityAffiliations;
        }

        public static bool IsValidUniversityEmail(string anEmail, IValidationDictionary aValidationDictionary) {
            IUniversityService theUniversityService = new UniversityService(aValidationDictionary);

            if (!theUniversityService.IsValidUniversityEmailAddress(anEmail)) {
                IEnumerable<string> myValidEmails = theUniversityService.ValidEmails();
                string myValidEmailsInString = string.Join(",", myValidEmails);

                aValidationDictionary.AddError("Email", anEmail, "I'm sorry that is not a valid University email. We currently only accept the following emails: " + myValidEmailsInString);
            }

            return aValidationDictionary.isValid;
        }
    }
}
