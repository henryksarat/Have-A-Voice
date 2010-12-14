using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;
using System.Web.Mvc;

namespace HaveAVoice.Models.View {
    public class EditUserModel {
        public User UserInformation { get; private set; }
        public string NewPassword { get; private set; }
        public string RetypedPassword { get; private set; }
        public string OriginalEmail { get; private set; }
        public string OriginalUsername { get; private set; }
        public HttpPostedFileBase ImageFile { get; private set; }
        public string ProfilePictureURL { get; private set; }
        public IEnumerable<SelectListItem> Timezones { get; private set; }
        public IEnumerable<SelectListItem> States { get; private set; }

        public EditUserModel(Builder aBuilder) {
            UserInformation = aBuilder.getUser();
            NewPassword = aBuilder.getNewPassword();
            RetypedPassword = aBuilder.getRetypedPassword();
            OriginalEmail = aBuilder.getOriginalEmail();
            OriginalUsername = aBuilder.getOriginalUsername();
            ImageFile = aBuilder.getImageFile();
            ProfilePictureURL = aBuilder.getProfilePictureUrl();
            Timezones = aBuilder.getTimezones();
            States = aBuilder.getStates();
        }

        public class Builder {
            private User theUser;
            private string theNewPassword;
            private string theRetypedPassword;
            private string theOriginalEmail;
            private string theOriginalUsername;
            private HttpPostedFileBase theImageFile;
            private string theProfilePictureUrl;
            private IEnumerable<SelectListItem> theTimezones;
            private IEnumerable<SelectListItem> theStates;

            public Builder(User aUser) {
                theUser = aUser;
                theNewPassword = string.Empty;
                theRetypedPassword = string.Empty;
                theOriginalEmail = theUser.Email;
                theOriginalUsername = theUser.Username;
            }

            public User getUser() {
                return theUser;
            }

            public string getNewPassword() {
                return theNewPassword;
            }

            public string getRetypedPassword() {
                return theRetypedPassword;
            }

            public string getOriginalEmail() {
                return theOriginalEmail;
            }

            public string getOriginalUsername() {
                return theOriginalUsername;
            }

            public IEnumerable<SelectListItem> getStates() {
                return theStates;
            }

            public HttpPostedFileBase getImageFile() {
                return theImageFile;
            }

            public string getProfilePictureUrl() {
                return theProfilePictureUrl;
            }

            public IEnumerable<SelectListItem> getTimezones() {
                return theTimezones;
            }

            public Builder setStates(IEnumerable<SelectListItem> aStates) {
                theStates = aStates;
                return this;
            }

            public Builder setImageFile(HttpPostedFileBase aFile) {
                theImageFile = aFile;
                return this;
            }

            public Builder setProfilePictureUrl(string aUrl) {
                theProfilePictureUrl = aUrl;
                return this;
            }

            public Builder setTimezones(IEnumerable<SelectListItem> aTimezones) {
                theTimezones = aTimezones;
                return this;
            }

            public Builder setNewPassword(string aPassword) {
                theNewPassword = aPassword;
                return this;
            }

            public Builder setRetypedPassword(string aPassword) {
                theRetypedPassword = aPassword;
                return this;
            }

            public Builder setOriginalEmail(string anEmail) {
                theOriginalEmail = anEmail;
                return this;
            }

            public Builder setOriginalUsername(string aUsername) {
                theOriginalUsername = aUsername;
                return this;
            }

            public EditUserModel Build() {
                return new EditUserModel(this);
            }
        }
    }
}
