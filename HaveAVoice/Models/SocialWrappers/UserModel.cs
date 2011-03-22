using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Social.User.Models;

namespace HaveAVoice.Models.SocialWrappers {
    public class SocialUserModel : AbstractUserModel<User> {
        public static SocialUserModel Create(User anExternal) {
            return new SocialUserModel(anExternal);
        }

        public override User FromModel() {
            return User.CreateUser(Id, Email, Password, FirstName, LastName, City, State, DateOfBirth, LastLogin, RegistrationDate, RegistrationIp, ShortUrl, Gender);
        }

        private SocialUserModel(User anExternal) {
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
        }
    }
}