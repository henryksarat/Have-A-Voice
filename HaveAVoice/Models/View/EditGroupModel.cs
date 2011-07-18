using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using Social.Generic.Constants;

namespace HaveAVoice.Models.View {
    public class EditGroupModel {
        public int Id { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Name { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Description { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string CreatorTitle { get; set; }
        
        public bool AutoAccept { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ZipCodeTags { get; set; }

        public IEnumerable<SelectListItem> States { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string StateTag { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string CityTag { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string KeywordTags { get; set; }

        public EditGroupModel() { }

        public EditGroupModel(Group aGroup) {
            Id = aGroup.Id;
            Name = aGroup.Name;
            Description = aGroup.Description;
            AutoAccept = aGroup.AutoAccept;
            ZipCodeTags = string.Join(",", aGroup.GroupZipCodeTags.Select(z => z.ZipCode));
            KeywordTags = string.Join(",", aGroup.GroupTags.Select(t => t.Tag));
            
            foreach (GroupCityStateTag myGroupCityStateTag in aGroup.GroupCityStateTags) {
                CityTag = myGroupCityStateTag.City;
                StateTag = myGroupCityStateTag.State;
                break;
            }

            States = new SelectList(UnitedStates.STATES);
        }
    }
}