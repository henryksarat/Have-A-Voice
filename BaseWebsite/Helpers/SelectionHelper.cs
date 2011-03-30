using System.Collections.Generic;
using Social.Generic;
using Social.Generic.Models;

namespace BaseWebsite.Helpers {
    public class SelectionHelper<T> {
        public static List<Pair<T, bool>> UserSelection(List<int> aSelectedUsers, IEnumerable<AbstractUserModel<T>> anAllUsers) {
            List<Pair<T, bool>> myUserSelection = new List<Pair<T, bool>>();

            foreach (AbstractUserModel<T> myUser in anAllUsers) {
                Pair<T, bool> pair = new Pair<T, bool>();
                pair.First = myUser.FromModel();
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
