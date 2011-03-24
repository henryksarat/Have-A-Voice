using System.Collections.Generic;
using System.Web.Mvc;
using Social.Generic;

namespace HaveAVoice.Models.View {
    public class SwitchUserRoles {
        public List<Pair<User, bool>> Users { get; private set; }
        public SelectList CurrentRoles { get; private set; }
        public SelectList MoveToRoles { get; private set; }
        public int SelectedCurrentRoleId  { get; private set; }
        public int SelectedMoveToRoleId { get; private set; }

        private SwitchUserRoles(Builder aBuilder) {
            Users = aBuilder.Users();
            CurrentRoles = aBuilder.CurrentRoles();
            MoveToRoles = aBuilder.MoveToRoles();
            SelectedCurrentRoleId = aBuilder.SelectedCurrentRoleId();
            SelectedMoveToRoleId = aBuilder.SelectedMoveToRoleId();
        }

        public class Builder {
            private List<Pair<User, bool>> theUsers = new List<Pair<User, bool>>();
            private IEnumerable<Role> theRoles = new List<Role>();
            private int theSelectedCurrentRoleId = 0;
            private int theSelectedMoveToRoleId = 0;

            public Builder() { }

            public Builder Users(List<Pair<User, bool>> aUsers) {
                theUsers = aUsers;
                return this;
            }

            public Builder Roles(IEnumerable<Role> aRoles) {
                theRoles = aRoles;
                return this;
            }

            public Builder SelectedCurrentRoleId(int aRoleId) {
                theSelectedCurrentRoleId = aRoleId;
                return this;
            }

            public Builder SelectedMoveToRoleId(int aRoleId) {
                theSelectedMoveToRoleId = aRoleId;
                return this;
            }

            public List<Pair<User, bool>> Users() {
                return theUsers;
            }

            public int SelectedCurrentRoleId() {
                return theSelectedCurrentRoleId;
            }

            public int SelectedMoveToRoleId() {
                return theSelectedMoveToRoleId;
            }

            public SelectList CurrentRoles() {
                List<SelectListItem> myFinalRoles = new List<SelectListItem>();

                SelectListItem mySelectList = new SelectListItem();
                mySelectList.Text = "Select role";
                mySelectList.Value = "0";
                myFinalRoles.Add(mySelectList);
                foreach (Role myRole in theRoles) {
                    mySelectList = new SelectListItem();
                    mySelectList.Text = myRole.Name;
                    mySelectList.Value = myRole.Id.ToString();
                    mySelectList.Selected = true;
                    myFinalRoles.Add(mySelectList);
                }

                return new SelectList(myFinalRoles, "Value", "Text", theSelectedCurrentRoleId);
            }

            public SelectList MoveToRoles() {
                List<SelectListItem> myFinalRoles = new List<SelectListItem>();

                SelectListItem mySelectList = new SelectListItem();
                mySelectList.Text = "Select role";
                mySelectList.Value = "0";
                myFinalRoles.Add(mySelectList);
                foreach (Role myRole in theRoles) {
                    mySelectList = new SelectListItem();
                    mySelectList.Text = myRole.Name;
                    mySelectList.Value = myRole.Id.ToString();
                    mySelectList.Selected = true;
                    myFinalRoles.Add(mySelectList);
                }

                return new SelectList(myFinalRoles, "Value", "Text", theSelectedMoveToRoleId);
            }

            public SwitchUserRoles Build() {
                return new SwitchUserRoles(this);
            }
        }
    }
}