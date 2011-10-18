using HaveAVoice.Models.View;
using Social.Generic.Models;

namespace HaveAVoice.Models.SocialWrappers {
    public class SocialUserModel : AbstractUserModel<User> {
        public string OriginalStringZip { get; set; }
        public string Zip { get; set; }

        public static SocialUserModel Create(User anExternal) {
            if (anExternal != null) {
                return new SocialUserModel(anExternal);
            }
            return null;
        }

        public override User CreateNewModel() {
            User myUser = User.CreateUser(Id, Email, Password, FirstName, LastName, City, State, LastLogin, RegistrationDate, RegistrationIp, int.Parse(Zip), Gender, false);
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
            Model = anExternal;

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
            Gender = anExternal.Gender;
            Website = anExternal.Website;
            AboutMe = anExternal.AboutMe;
            CookieHash = anExternal.CookieHash;
            CookieHashCreationDate = anExternal.CookieCreationDate;
            ActivationCode = anExternal.ActivationCode;
            ForgotPasswordHash = anExternal.ForgotPasswordHash;
            ForgotPasswordHashDateTimeStamp = anExternal.ForgotPasswordHashDateTimeStamp;
            Zip = anExternal.Zip.ToString();
        }
    }
}