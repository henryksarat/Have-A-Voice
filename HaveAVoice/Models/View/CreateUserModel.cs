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

        public string Zip { get; set; }

        public String getDateOfBirthFormatted() {
            if (DateOfBirth.HasValue) {
                return DateOfBirth.Value.ToString("MM-dd-yyyy");
            } else {
                return string.Empty;
            }
        }

        public override User CreateNewModel() {
            DateTime myTempDateFiller = DateTime.UtcNow;
            string myTempIp = "127.0.0.1";

            int myParsedZip = 0;
            int.TryParse(Zip, out myParsedZip);
            return User.CreateUser(0, Email, Password, FirstName, LastName, City,
                                   State, myTempDateFiller, myTempDateFiller,
                                   myTempIp, myParsedZip, Gender, false);
        }
    }
}