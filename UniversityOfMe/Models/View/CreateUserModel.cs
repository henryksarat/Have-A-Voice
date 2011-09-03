﻿using UniversityOfMe.Models.SocialModels;
using System.Collections.Generic;
using System.Web.Mvc;
using System;

namespace UniversityOfMe.Models.View {
    public class CreateUserModel : SocialUserModel {
        public IEnumerable<SelectListItem> Genders { get; set; }
        public int RegisteredUserCount { get; set; }

        public CreateUserModel() {
            DateOfBirth = DateTime.UtcNow;
        }

        protected CreateUserModel(User aUser)
            : base(aUser) { }

        public string getDateOfBirthFormatted() {
            return DateOfBirth.ToString("MM-dd-yyyy");
        }
    }
}