using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UniversityOfMe.Models;
using Social.User.Services;

namespace UniversityOfMe.Services.Users {
    public interface IUofMeUserRetrievalService : IUserRetrievalService<User> {
        IEnumerable<User> GetAllFemaleUsers();
        IEnumerable<User> GetAllMaleUsers();
        User GetUserByChangeEmailInformation(string anOldEmail, string aNewEmailHash);
    }
}