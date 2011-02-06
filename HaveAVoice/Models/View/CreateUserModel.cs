﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using HaveAVoice.Helpers.Enums;

namespace HaveAVoice.Models.View {
    public abstract class CreateUserModel {
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string FirstName { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Password { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Email { get; set; }

        public bool Agreement { get; set; }

        public IEnumerable<SelectListItem> Genders { get; set; }

        public IEnumerable<SelectListItem> States { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Gender { get; set; }

        public HAVGender SelectedGender {
            get {
                HAVGender mySelectedGender = HAVGender.Select;
                foreach (HAVGender myGender in Enum.GetValues(typeof(HAVGender))) {
                    if (Gender.ToUpper().Equals(Enum.GetName(typeof(HAVGender), myGender).ToUpper())) {
                        mySelectedGender = myGender;
                    }
                }

                return mySelectedGender;
            }
        }

        public abstract User Build();

        public abstract String getDateOfBirthFormatted();
    }
}