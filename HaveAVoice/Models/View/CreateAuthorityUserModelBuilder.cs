using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using HaveAVoice.Helpers;

namespace HaveAVoice.Models.View {
    public class CreateAuthorityUserModelBuilder : CreateUserModel {
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string RepresentingCity { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string RepresentingState { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Token { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string AuthorityType { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string UserPosition { get; set; }

        public CreateAuthorityUserModelBuilder() {
            DateOfBirth = DateTime.UtcNow;
            States = new List<SelectListItem>();
        }

        public override User Build() {
            DateTime myTempDateFiller = DateTime.UtcNow;
            string myTempIp = "127.0.0.1";

            User myUser = User.CreateUser(0, Email, Password, FirstName, LastName, 
                RepresentingCity, RepresentingState, DateOfBirth, myTempDateFiller, myTempDateFiller,
                myTempIp, ShortUrl, Gender);
            myUser.UserPositionId = UserPosition;
            return myUser;
        }

        public override String getDateOfBirthFormatted() {
            return DateOfBirth.ToString("MM-dd-yyyy");
        }
    }
}
