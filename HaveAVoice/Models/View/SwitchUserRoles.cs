using System.Collections.Generic;
using System.Web.Mvc;
using Social.Generic;
using BaseWebsite.Models;

namespace HaveAVoice.Models.View {
    /*
    public class SwitchUserRoles : AbstractSwitchUserRoles<User, Role> {
        protected override SelectList GetCurrentRoles() {
            List<SelectListItem> myFinalRoles = new List<SelectListItem>();

            SelectListItem mySelectList = new SelectListItem();
            mySelectList.Text = "Select role";
            mySelectList.Value = "0";
            myFinalRoles.Add(mySelectList);
            foreach (Role myRole in Roles) {
                mySelectList = new SelectListItem();
                mySelectList.Text = myRole.Name;
                mySelectList.Value = myRole.Id.ToString();
                mySelectList.Selected = true;
                myFinalRoles.Add(mySelectList);
            }

            return new SelectList(myFinalRoles, "Value", "Text", SelectedCurrentRoleId);
        }

        protected override SelectList GetMoveToRoles() {
            List<SelectListItem> myFinalRoles = new List<SelectListItem>();

            SelectListItem mySelectList = new SelectListItem();
            mySelectList.Text = "Select role";
            mySelectList.Value = "0";
            myFinalRoles.Add(mySelectList);
            foreach (Role myRole in Roles) {
                mySelectList = new SelectListItem();
                mySelectList.Text = myRole.Name;
                mySelectList.Value = myRole.Id.ToString();
                mySelectList.Selected = true;
                myFinalRoles.Add(mySelectList);
            }

            return new SelectList(myFinalRoles, "Value", "Text", SelectedMoveToRoleId);
        }
    }
     * */
}