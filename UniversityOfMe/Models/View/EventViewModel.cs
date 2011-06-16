using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Globalization;

namespace UniversityOfMe.Models.View {
    public class EventViewModel {
        public int Id { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string UniversityId { get; set; }

        public IEnumerable<SelectListItem> EventPrivacyOptions { get; set; }

        public IEnumerable<SelectListItem> StartTimes { get; set; }

        public IEnumerable<SelectListItem> EndTimes { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string EventPrivacyOption { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string StartTime { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string EndTime { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Title { get; set; }

        public string StartDate { get; set; }
        
        public string EndDate { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Information { get; set; }

        public EventViewModel() { }

        public EventViewModel(Event anExternal) {
            Id = anExternal.Id;
            UniversityId = anExternal.UniversityId;
            EventPrivacyOption = anExternal.EntireSchool.ToString();
            Title = anExternal.Title;
            StartDate = anExternal.StartDate.ToString("MM-dd-yyyy");
            StartTime = anExternal.StartDate.ToString("hh:mm tt");
            EndDate = anExternal.EndDate.ToString("MM-dd-yyyy");
            EndTime = anExternal.EndDate.ToString("hh:mm tt");
            Information = anExternal.Information;
        }

        public DateTime GetStartDate() {
            return Convert.ToDateTime(StartDate + " " + StartTime);
        }

        public DateTime GetEndDate() {
            return Convert.ToDateTime(EndDate + " " + EndTime);
        }
    }
}