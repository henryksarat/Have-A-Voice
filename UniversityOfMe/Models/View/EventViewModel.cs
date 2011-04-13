using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace UniversityOfMe.Models.View {
    public class EventViewModel {
        public int Id { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string UniversityId { get; set; }

        public IEnumerable<SelectListItem> EventPrivacyOptions { get; set; }
        
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string EventPrivacyOption { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Title { get; set; }

        public DateTime StartDate { get; set; }
        
        public DateTime EndDate { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Information { get; set; }

        public EventViewModel() { }

        public EventViewModel(Event anExternal) {
            Id = anExternal.Id;
            UniversityId = anExternal.UniversityId;
            EventPrivacyOption = anExternal.EntireSchool.ToString();
            Title = anExternal.Title;
            StartDate = anExternal.StartDate;
            EndDate = anExternal.EndDate;
            Information = anExternal.Information;
        }
    }
}