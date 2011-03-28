using System.ComponentModel.DataAnnotations;
using Social.Generic.Models;

namespace UniversityOfMe.Models.Social {
    public class SocialUserModel : AbstractUserModel<User> {
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        private string theUniversity = string.Empty;

        protected SocialUserModel() { }

        public static SocialUserModel Create(User anExternal) {
            if (anExternal != null) {
                return new SocialUserModel(anExternal);
            }
            return null;
        }

        public override User FromModel() {
            User myUser = User.CreateUser(Id, theUniversity, Email, Password, FirstName, LastName, 
                Gender, DateOfBirth, LastLogin, RegistrationDate, RegistrationIp, ShortUrl);
            myUser.City = City;
            myUser.State = State;
            myUser.Website = Website;
            myUser.AboutMe = AboutMe;
            myUser.CookieHash = CookieHash;
            myUser.CookieCreationDate = CookieHashCreationDate;
            myUser.ActivationCode = ActivationCode;
            myUser.ForgotPasswordHash = ForgotPasswordHash;
            myUser.ForgotPasswordHashDateTimeStamp = ForgotPasswordHashDateTimeStamp;
            return myUser;
        }

        private SocialUserModel(User anExternal) {
            theUniversity = anExternal.UniversityId;

            Id = anExternal.Id;
            Email = anExternal.Email;
            Password = anExternal.Password;
            FirstName = anExternal.FirstName;
            LastName = anExternal.LastName;
            City = anExternal.City;
            State = anExternal.State;
            DateOfBirth = anExternal.DateOfBirth;
            LastLogin = anExternal.LastLogin;
            RegistrationDate = anExternal.RegistrationDate;
            RegistrationIp = anExternal.RegistrationIp;
            ShortUrl = anExternal.ShortUrl;
            Gender = anExternal.Gender;
            Website = anExternal.Website;
            AboutMe = anExternal.AboutMe;
            CookieHash = anExternal.CookieHash;
            CookieHashCreationDate = anExternal.CookieCreationDate;
            ActivationCode = anExternal.ActivationCode;
            ForgotPasswordHash = anExternal.ForgotPasswordHash;
            ForgotPasswordHashDateTimeStamp = anExternal.ForgotPasswordHashDateTimeStamp;
            ShortUrl = anExternal.ShortUrl;
            Zip = anExternal.Zip;
        }
    }
}