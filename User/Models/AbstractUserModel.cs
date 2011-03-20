using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Social.User.Models {
    public abstract class AbstractUserModel<T> {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime LastLogin { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string RegistrationIp { get; set; }
        public string Website { get; set; }
        public string AboutMe { get; set; }
        public string CookieHash { get; set; }
        public DateTime CookieHashCreationDate { get; set; }
        public string ActivationCode { get; set; }
        public string ForgotPasswordHash { get; set; }
        public DateTime ForgotPasswordHashDateTimeStamp { get; set; }
        public int Zip { get; set; }
        public string ShortUrl { get; set; }
        public string Gender { get; set; }

        public abstract T FromModel();
    }
}
