using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Services.UserFeatures {
    public interface IHAVUserRetrievalService {
        User GetUser(int aUserId);
        User GetUserByShortUrl(string aShortUrl);
        User GetUser(string anEmail, string aPassword);
        User GetUser(string anEmail);
        IEnumerable<User> GetUsersByNameSearch(string aNamePortion);
    }
}