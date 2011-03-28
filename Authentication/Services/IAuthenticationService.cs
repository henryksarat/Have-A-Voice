using Social.Generic.Models;
using Social.User.Models;
using Social.Authentication.Helpers;

namespace Social.Authentication.Services {
    public interface IAuthenticationService<T, U, V, W, X, Y> {
        void ActivateNewUser(string activationCode);
        UserInformationModel<T> RefreshUserInformationModel(string anEmail, string aPassword, IProfilePictureStrategy<T> aProfilePictureStrategy);
        UserInformationModel<T> AuthenticateUser(string anEmail, string aPassword, IProfilePictureStrategy<T> aProfilePictureStrategy);
        UserInformationModel<T> CreateUserInformationModel(AbstractUserModel<T> aUser, IProfilePictureStrategy<T> aProfilePictureStrategy);
        void CreateRememberMeCredentials(AbstractUserModel<T> aUserModel);
        T ReadRememberMeCredentials();
    }
}