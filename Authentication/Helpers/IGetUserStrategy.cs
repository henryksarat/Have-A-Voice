using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Social.Generic.Models;

namespace Social.Authentication.Helpers {
    public interface IGetUserStrategy<T> {
        UserInformationModel<T> GetUserInformationModel(int anId);
    }
}
