using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;
using System.Web.Mvc;

namespace HaveAVoice.Models.View {
    public class CreateUserModelBuilder {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public string City { get; set; }
        public string Email { get; set; }
        public IEnumerable<SelectListItem> States { get; set; }
        public bool Agreement { get; set; }
        public string State { get; set; }

        public CreateUserModelBuilder() {
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
            return User.CreateUser(0, Username, Email, Password, FirstName, LastName, City, State, DateOfBirth, myTempDateFiller, myTempDateFiller, myTempIp, false, myTempUtcOffset);
        }

        public String getDateOfBirthFormatted() {
            return DateOfBirth.ToString("MM-dd-yyyy");
        }
    }
}
