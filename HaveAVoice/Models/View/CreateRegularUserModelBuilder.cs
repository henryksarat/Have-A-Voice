using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using HaveAVoice.Helpers.Enums;
using HaveAVoice.Helpers;

namespace HaveAVoice.Models.View {

    public class CreateRegularUserModelBuilder : CreateUserModel {
        public IEnumerable<SelectListItem> States { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string City { get; set; }


        public CreateRegularUserModelBuilder() {
            DateOfBirth = DateTime.UtcNow;
            FirstName = string.Empty;
            LastName = string.Empty;
            Password = string.Empty;
            Username = string.Empty;
            City = string.Empty;
            Email = string.Empty;
            States = new List<SelectListItem>();
            State = string.Empty;
        }

        public User Build() {
            DateTime myTempDateFiller = DateTime.UtcNow;
            string myTempIp = "127.0.0.1";
            string myTempUtcOffset = "shitty";
            bool myGender = HAVConstants.MALE;

            if (Gender.ToUpper().Equals("FEMALE")) {
                myGender = HAVConstants.FEMALE;
            }
            return User.CreateUser(0, Username, Email, Password, FirstName, LastName, City, State, DateOfBirth, myTempDateFiller, myTempDateFiller, myTempIp, false, myTempUtcOffset, myGender);
        }

        public String getDateOfBirthFormatted() {
            return DateOfBirth.ToString("MM-dd-yyyy");
        }
    }
}
