using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using HaveAVoice.Helpers.Enums;
using Social.Generic.Models;

namespace HaveAVoice.Models.View {
    public abstract class CreateUserModel : AbstractUserModel<User> {
        public bool Agreement { get; set; }

        public IEnumerable<SelectListItem> Genders { get; set; }

        public IEnumerable<SelectListItem> States { get; set; }

        public int Zip { get; set; }

        public String getDateOfBirthFormatted() {
            return DateOfBirth.ToString("MM-dd-yyyy");
        }

        public override User CreateNewModel() {
            DateTime myTempDateFiller = DateTime.UtcNow;
            string myTempIp = "127.0.0.1";

            return User.CreateUser(0, Email, Password, FirstName, LastName, City,
                                   State, DateOfBirth, myTempDateFiller, myTempDateFiller,
                                   myTempIp, Zip, ShortUrl, Gender);
        }
    }
}