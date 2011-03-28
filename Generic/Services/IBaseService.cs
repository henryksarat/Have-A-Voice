using System;
using Social.Generic.Models;

namespace Social.Generic.Services {
    public interface IBaseService<T> {
        void LogError(AbstractUserModel<T> aUserInformation, Exception exception, string details);
    }
}
