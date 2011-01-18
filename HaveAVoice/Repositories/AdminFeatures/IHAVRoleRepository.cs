using System;
using System.Collections.Generic;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.AdminFeatures {
    public interface IHAVRoleRepository {
        Role Create(User aCreatedByUser, Role aRoleToCreate, List<int> aSelectedPermissionIds, int aSelectedRestrictionId);
        Role Edit(User aEditedByUser, Role aRoleToEdit, List<int> aSelectedPermissionIds, int aSelectedRestrictionId);
        void Delete(User aDeletedByUser, Role aRoleToDelete);
        Role FindRole(int id);
        IEnumerable<User> FindUsersInRole(int aRoleId);
        IEnumerable<Role> GetAllRoles();
        Role GetDefaultRole();
        Role GetAuthorityRole();
        Role GetNotConfirmedUserRole();
        void MoveUsersToRole(List<int> aUsers, int aFromRoleId, int aToRoleId);
    }
}