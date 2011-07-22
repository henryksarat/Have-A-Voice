﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using Social.Generic.Constants;
using Social.Generic.Helpers;
using Social.Generic.Models;

namespace HaveAVoice.Models.View {
    public class EditGroupModel : BasicLocationModel {
        public int Id { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Name { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Description { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string CreatorTitle { get; set; }

        public bool AutoAccept { get; set; }

        public bool MakePublic { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ZipCodeTags { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string StateTag { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string CityTag { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string KeywordTags { get; set; }

        public ViewAction ViewAction { get; set; }

        public EditGroupModel() {
            MakePublic = true;
        }

        public EditGroupModel(Group aGroup) {
            Id = aGroup.Id;
            Name = aGroup.Name;
            Description = aGroup.Description;
            AutoAccept = aGroup.AutoAccept;
            MakePublic = aGroup.MakePublic;
            ZipCodeTags = string.Join(",", aGroup.GroupZipCodeTags.Select(z => z.ZipCode));
            KeywordTags = string.Join(",", aGroup.GroupTags.Select(t => t.Tag));
            
            foreach (GroupCityStateTag myGroupCityStateTag in aGroup.GroupCityStateTags) {
                CityTag = myGroupCityStateTag.City;
                StateTag = myGroupCityStateTag.State;
                break;
            }
        }
    }
}