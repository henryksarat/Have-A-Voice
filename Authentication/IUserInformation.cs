using Social.Generic.Models;

namespace Social.Authentication {
    public interface IUserInformation<T, U> {
        UserInformationModel<T> GetUserInformaton();
    }
}
