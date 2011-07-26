using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Social.Generic.Models {
    public class AbstractEventViewModel {
        public int Id { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string UniversityId { get; set; }

        public IEnumerable<SelectListItem> StartTimes { get; set; }

        public IEnumerable<SelectListItem> EndTimes { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string StartTime { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string EndTime { get; set; }

        public string StartDate { get; set; }
        
        public string EndDate { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Title { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Information { get; set; }

        public AbstractEventViewModel() {
            StartDate = DateTime.Today.Date.ToString("MM-dd-yyyy");
            EndDate = DateTime.Today.Date.ToString("MM-dd-yyyy");
            StartTimes = new SelectList(Constants.Constants.GetTimes(), "Value", "Key");
            EndTimes = new SelectList(Constants.Constants.GetTimes(), "Value", "Key");
        }

        public DateTime GetStartDate() {
            return Convert.ToDateTime(StartDate + " " + StartTime);
        }

        public DateTime GetEndDate() {
            return Convert.ToDateTime(EndDate + " " + EndTime);
        }
    }
}