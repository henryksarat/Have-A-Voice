using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.UserFeatures {
    public interface IHAVUserRetrievalRepository {
        User GetUser(int anId);
        User GetUser(string anEmail, string aPassword);
        User GetUser(string anEmail);
    }
}