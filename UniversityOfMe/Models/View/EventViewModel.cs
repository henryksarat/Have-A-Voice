using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Globalization;
using Social.Generic.Constants;

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

        public EventViewModel() {
            StartDate = DateTime.Today.Date.ToString("MM-dd-yyyy");
            EndDate = DateTime.Today.Date.ToString("MM-dd-yyyy");
            StartTimes = new SelectList(Constants.GetTimes(), "Value", "Key");
            EndTimes = new SelectList(Constants.GetTimes(), "Value", "Key");
        }

        public EventViewModel(Event anExternal) {
            Id = anExternal.Id;
            UniversityId = anExternal.UniversityId;
            EventPrivacyOption = anExternal.EntireSchool.ToString();
            Title = anExternal.Title;

            TimeZone myLocalZone = TimeZone.CurrentTimeZone;
            DateTime myStartDateLocal = myLocalZone.ToLocalTime(anExternal.StartDate);
            DateTime myEndDateLocal = myLocalZone.ToLocalTime(anExternal.EndDate);

            StartDate = myStartDateLocal.ToString("MM-dd-yyyy");
            StartTime = myStartDateLocal.ToString("hh:mm tt");
            EndDate = myEndDateLocal.ToString("MM-dd-yyyy");
            EndTime = myEndDateLocal.ToString("hh:mm tt");
            Information = anExternal.Information;

            StartTimes = new SelectList(Constants.GetTimes(), "Value", "Key", StartTime);
            EndTimes = new SelectList(Constants.GetTimes(), "Value", "Key", EndTime);
        }

        public DateTime GetStartDate() {
            return Convert.ToDateTime(StartDate + " " + StartTime);
        }

        public DateTime GetEndDate() {
            return Convert.ToDateTime(EndDate + " " + EndTime);
        }
    }
}