﻿using System.Collections.Generic;
using Social.User.Services;
using UniversityOfMe.Models;
using UniversityOfMe.Models.View;

namespace UniversityOfMe.Services.Users {
    public interface IUofMeUserService : IUserService<User, Role, UserRole> {
        bool ChangeEmail(string anOldEmail, string aNewEmailHash);
        bool EditUser(EditUserModel aUserToEdit, string aHashedPassword);
        IEnumerable<User> GetNewestUsers(User aRequestingUser, string aUniversityId, int aLimit);
        EditUserModel GetUserForEdit(User aUser);
        int GetNumberOfRegisteredUsers();
    }
}
