using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Social.Generic.Models;

namespace HaveAVoice.Models.View {
    public class EventViewModel : AbstractEventViewModel {
        public IEnumerable<SelectListItem> EventPrivacyOptions { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string EventPrivacyOption { get; set; }

        public IEnumerable<Event> Results { get; set; }

        public EventViewModel() : base() { }

        public EventViewModel(Event anExternal) : base() {
            Id = anExternal.Id;

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