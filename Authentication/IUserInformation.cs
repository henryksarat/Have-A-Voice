using Social.Generic.Models;

namespace Social.Authentication {
    public interface IUserInformation<T, U> {
        bool IsLoggedIn();
        UserInformationModel<T> GetUserInformaton();
        void ForceUserInformationClear();
    }
}
