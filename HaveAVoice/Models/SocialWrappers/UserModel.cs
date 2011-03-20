using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Social.User.Models;

namespace HaveAVoice.Models.SocialWrappers {
    public class UserModel : AbstractUserModel<User> {
        private UserModel theModel;

        public UserModel(User anExternal) {
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

        public override User FromModel() {
            return User.CreateUser(Id, Email, Password, FirstName, LastName, City, State, DateOfBirth, LastLogin, RegistrationDate, RegistrationIp, ShortUrl, Gender);
        }
    }
}