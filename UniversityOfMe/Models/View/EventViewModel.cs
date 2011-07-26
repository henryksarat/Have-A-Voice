using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Social.Generic.Constants;
using Social.Generic.Models;

namespace UniversityOfMe.Models.View {
    public class EventViewModel : AbstractEventViewModel {
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Title { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string UniversityId { get; set; }

        public IEnumerable<SelectListItem> EventPrivacyOptions { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string EventPrivacyOption { get; set; }

        public EventViewModel() : base() { }

        public EventViewModel(Event anExternal) : base() {
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
        }
    }
}