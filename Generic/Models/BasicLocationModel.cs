using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Social.Generic.Constants;

namespace Social.Generic.Models {
    public class BasicLocationModel {
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string City { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string State { get; set; }

        public IEnumerable<SelectListItem> States { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ZipCode { get; set; }

        public BasicLocationModel() {
            States = new SelectList(UnitedStates.STATES);
        }

        public BasicLocationModel(string aState) {
            States = new SelectList(UnitedStates.STATES, aState);
        }
    }
}
