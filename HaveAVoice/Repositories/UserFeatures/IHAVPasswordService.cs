using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.UserFeatures {
    public interface IHAVPasswordRepository {
        void UpdateUserForgotPasswordHash(string anEmail, string aHashCode);
        User GetUserByEmailAndForgotPasswordHash(string anEmail, string aHashCode);
        void ChangePassword(int aUserId, string aPassword);
    }
}