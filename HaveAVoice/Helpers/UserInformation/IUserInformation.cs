using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HaveAVoice.Models;
using HaveAVoice.Helpers.Enums;
using HaveAVoice.Models.View;
using Social.Generic.Models;

namespace HaveAVoice.Helpers.UserInformation {
    public interface IUserInformation {
        UserInformationModel<User> GetUserInformaton();
    }
}
