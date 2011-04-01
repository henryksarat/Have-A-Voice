using System;
using System.ComponentModel.DataAnnotations;

namespace Social.Generic.Models {
    public abstract class AbstractUserModel<T> : AbstractSocialModel<T> {
        public int Id { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Email { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Password { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string FirstName { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string LastName { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string City { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string State { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime LastLogin { get; set; }
        public DateTime RegistrationDate { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string RegistrationIp { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Website { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string AboutMe { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string CookieHash { get; set; }
        public DateTime? CookieHashCreationDate { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ActivationCode { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ForgotPasswordHash { get; set; }
        public DateTime? ForgotPasswordHashDateTimeStamp { get; set; }
        public int? Zip { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ShortUrl { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Gender { get; set; }

        public abstract T CreateNewModel();
    }
}
