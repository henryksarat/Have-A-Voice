using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace UniversityOfMe.Models.View {
    public class CreateProfessorReviewModel {
        public Professor Professor { get; set; }

        public int ProfessorId { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ProfessorName { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string UniversityId { get; set; }

        public IEnumerable<SelectListItem> AcademicTerms { get; set; }
        
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string AcademicTermId { get; set; }

        public IEnumerable<SelectListItem> Years { get; set; }

        public int Year { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Class { get; set; }

        public int Rating { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Review { get; set; }

        public bool Anonymous { get; set; }

        public ProfessorReview ToModel() {
            return ProfessorReview.CreateProfessorReview(0, 0, ProfessorId, AcademicTermId, Year, Class, Rating, Review, DateTime.UtcNow, Anonymous);
        }
    }
}