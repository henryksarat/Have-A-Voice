﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using HaveAVoice.Helpers.Enums;
using HaveAVoice.Helpers;

namespace HaveAVoice.Models.View {

    public class CreateUserModelBuilder : CreateUserModel {
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string State { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string City { get; set; }


        public CreateUserModelBuilder() {
            DateOfBirth = DateTime.UtcNow;
            FirstName = string.Empty;
            LastName = string.Empty;
            Password = string.Empty;
            City = string.Empty;
            Email = string.Empty;
            States = new List<SelectListItem>();
            State = string.Empty;
        }

        public override User Build() {
            DateTime myTempDateFiller = DateTime.UtcNow;
            string myTempIp = "127.0.0.1";

            return User.CreateUser(0, Email, Password, FirstName, LastName, City, 
                State, DateOfBirth, myTempDateFiller, myTempDateFiller, 
                myTempIp, ShortUrl, Gender);
        }

        public override String getDateOfBirthFormatted() {
            return DateOfBirth.ToString("MM-dd-yyyy");
        }
    }
}
