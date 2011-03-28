using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Social.Generic.Models;

namespace Social.Generic.Repositories {
    public interface IBaseRepository<T> {
        void LogError(AbstractUserModel<T> aUserInformation, Exception exception, string details);
    }
}
