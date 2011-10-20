﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniversityOfMe.Models;
using UniversityOfMe.Models.View;
using Social.Generic.Models;

namespace UniversityOfMe.Services {
    public interface IUniversityService {
        void AddUserToUniversity(User aUser);
        IDictionary<string, string> CreateAllUniversitiesDictionaryEntry();
        UniversityView GetUniversityProfile(UserInformationModel<User> aUserInformation, string aUniversityId);
        University GetUniversityById(string aUniversityId);
        IEnumerable<University> GetValidUniversities();
        bool IsValidUniversityEmailAddress(string anEmail);
        IEnumerable<string> ValidEmails();
        bool IsFromUniversity(User aUser, string aUniversityId);
    }
}
