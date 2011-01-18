using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace HaveAVoice.Models.View {
    public class CreateAuthorityUserModelBuilder {
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string FirstName { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Password { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Username { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string RepresentingCity { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Email { get; set; }

        public IEnumerable<SelectListItem> States { get; set; }
        public bool Agreement { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string RepresentingState { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Token { get; set; }

        public CreateAuthorityUserModelBuilder() {
            DateOfBirth = DateTime.UtcNow;
            FirstName = string.Empty;
            LastName = string.Empty;
            Password = string.Empty;
            Username = string.Empty;
            RepresentingCity = string.Empty;
            Email = string.Empty;
            States = new List<SelectListItem>();
            RepresentingState = string.Empty;
        }

        public User Build() {
            DateTime myTempDateFiller = DateTime.UtcNow;
            string myTempIp = "127.0.0.1";
            string myTempUtcOffset = "shitty";
            return User.CreateUser(0, Username, Email, Password, FirstName, LastName, RepresentingCity, RepresentingState, DateOfBirth, myTempDateFiller, myTempDateFiller, myTempIp, false, myTempUtcOffset);
        }

        public String getDateOfBirthFormatted() {
            return DateOfBirth.ToString("MM-dd-yyyy");
        }
    }
}
