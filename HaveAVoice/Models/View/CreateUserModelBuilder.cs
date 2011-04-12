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

    public class CreateUserModelBuilder : CreateUserModel {
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

        public override User CreateNewModel() {
            DateTime myTempDateFiller = DateTime.UtcNow;
            string myTempIp = "127.0.0.1";

            return User.CreateUser(0, Email, Password, FirstName, LastName, City, 
                State, DateOfBirth, myTempDateFiller, myTempDateFiller,
                myTempIp, Zip, ShortUrl, Gender);
        }
    }
}
