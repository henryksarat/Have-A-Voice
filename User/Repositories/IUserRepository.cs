﻿using Social.Generic.Models;

namespace Social.User {
    public interface IUserRepository<T, U, V> {
        V AddUserToRole(T user, U role);
        AbstractUserModel<T> CreateUser(T userToCreate);
        void DeleteUser(T userToDelete);
        T DeleteUserFromRole(int userId, int roleId);
        bool EmailRegistered(string email);
        void RemoveUserFromRole(T aUser, U aRole);
        bool ShortUrlTaken(string aShortUrl);
        void UpdateUser(T userToEdit);
    }
}