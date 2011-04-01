using System.Collections.Generic;
using Social.Generic;
using UniversityOfMe.Models;

namespace UniversityOfMe.Helpers {
    public class SelectionHelper {
        public static List<Pair<Permission, bool>> PermissionSelection(List<int> selectedPermissions,
                                                                       IEnumerable<Permission> allPermissions) {
            List<Pair<Permission, bool>> permissionSelection = new List<Pair<Permission, bool>>();

            foreach (Permission permission in allPermissions) {
                Pair<Permission, bool> pair = new Pair<Permission, bool>();
                pair.First = permission;
                pair.Second = false;

                if (selectedPermissions != null && selectedPermissions.Contains(permission.Id)) {
                    pair.Second = true;
                }

                permissionSelection.Add(pair);
            }

            return permissionSelection;
        }

        public static List<Pair<User, bool>> UserSelection(List<int> aSelectedUsers, IEnumerable<User> anAllUsers) {
            List<Pair<User, bool>> myUserSelection = new List<Pair<User, bool>>();

            foreach (User myUser in anAllUsers) {
                Pair<User, bool> pair = new Pair<User, bool>();
                pair.First = myUser;
                pair.Second = false;

                if (aSelectedUsers != null && aSelectedUsers.Contains(myUser.Id)) {
                    pair.Second = true;
                }

                myUserSelection.Add(pair);
            }

            return myUserSelection;
        }
    }
}
