using System;
using System.ComponentModel.DataAnnotations;
using UniversityOfMe.Models;
using System.Web.Mvc;
using System.Collections.Generic;

namespace UniversityOfMe.Models.View {
    public class CreateProfessorModel {
        public IEnumerable<SelectListItem> Universities { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string UniversityId { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string FirstName { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string LastName { get; set; }

        public Professor ToModel() {
            return Professor.CreateProfessor(0, UniversityId, FirstName, LastName, 0, DateTime.UtcNow);
        }
    }
}