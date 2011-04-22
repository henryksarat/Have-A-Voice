using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaveAVoice.Helpers.Authority {
    public class AuthorityClassification {
        public static List<string> GetAuthorityPostionsViewableByZip() {
            List<string> myViewablePositions = new List<string>();
            myViewablePositions.Add("ALDERMAN");
            myViewablePositions.Add("STATE_REP");
            return myViewablePositions;
        }

        public static List<string> GetAuthorityPostionsViewableByCity() {
            List<string> myViewablePositions = new List<string>();
            myViewablePositions.Add("MAYOR");
            myViewablePositions.Add("CITY_CLERK");
            return myViewablePositions;
        }

        public static List<string> GetAuthorityPostionsViewableByState() {
            List<string> myViewablePositions = new List<string>();
            myViewablePositions.Add("SENATOR");
            return myViewablePositions;
        }
    }
}