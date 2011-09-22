using System.ComponentModel.DataAnnotations;
using Social.Generic.Models;
using System;

namespace UniversityOfMe.Models.SocialModels {
    public class SocialUserModel : AbstractUserModel<User> {
        protected SocialUserModel() { }

        public static SocialUserModel Create(User anExternal) {
            if (anExternal != null) {
                return new SocialUserModel(anExternal);
            }
            return null;
        }

        public override User CreateNewModel() {
            User myUser = User.CreateUser(Id, Email, Password, FirstName, LastName, 
                Gender, LastLogin, RegistrationDate, RegistrationIp);
            myUser.City = City;
            myUser.State = State;
            myUser.Website = Website;
            myUser.AboutMe = AboutMe;
            myUser.CookieHash = CookieHash;
            myUser.CookieCreationDate = CookieHashCreationDate;
            myUser.ActivationCode = ActivationCode;
            myUser.ForgotPasswordHash = ForgotPasswordHash;
            myUser.ForgotPasswordHashDateTimeStamp = ForgotPasswordHashDateTimeStamp;
            if (DateOfBirth.HasValue && DateOfBirth.Value.Year != DateTime.Today.Year && DateOfBirth.Value.Year != (DateTime.Today.Year - 1)
                && DateOfBirth.Value.Year != (DateTime.Today.Year + 1) && DateOfBirth.Value.Year != 1) {
                myUser.DateOfBirth = DateOfBirth;
            }
            return myUser;
        }

        protected SocialUserModel(User anExternal) {
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
            Zip = anExternal.Zip;

            Model = anExternal;
        }
    }
}