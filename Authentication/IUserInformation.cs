using Social.Generic.Models;

namespace Social.Authentication {
    public interface IUserInformation<T, U> {
        bool IsLoggedIn();
        UserInformationModel<T> GetUserInformaton();
        UserInformationModel<T> GetUserInformaton(int anId);
        void ForceUserInformationClear();
        string GetIdentityName();
    }
}
