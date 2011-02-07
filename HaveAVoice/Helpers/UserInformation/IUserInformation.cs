using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HaveAVoice.Models;
using HaveAVoice.Helpers.Enums;
using HaveAVoice.Models.View;

namespace HaveAVoice.Helpers.UserInformation {
    public interface IUserInformation {
        UserInformationModel GetUserInformaton();
    }
}
