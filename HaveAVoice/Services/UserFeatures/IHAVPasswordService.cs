using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaveAVoice.Services.UserFeatures {
    public interface IHAVPasswordService {
        bool ForgotPasswordRequest(string anEmail);
        bool ChangePassword(string anEmail, string aForgotPasswordHash, string aPassword, string aRetypedPassword);
    }
}