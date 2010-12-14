using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;
using System.Web.Mvc;

namespace HaveAVoice.Models.View {
    public class CreateUserModelBuilder {
        private String theFullName = string.Empty;
        private DateTime theDateOfBirth = DateTime.UtcNow;
        private String thePassword = string.Empty;
        private String theUsername = string.Empty;
        private String theCity = string.Empty;
        private String theEmail = string.Empty;
        private IEnumerable<SelectListItem> theStates = new List<SelectListItem>();
        private bool theAgreement = false;
        private String theState = string.Empty;

        private String[] theSplitName;


        public CreateUserModelBuilder() { }

        public CreateUserModelBuilder FullName(String aValue) {
            theFullName = aValue;
            return this;
        }

        public CreateUserModelBuilder DateOfBirth(DateTime aValue) {
            theDateOfBirth = aValue;
            return this;
        }

        public CreateUserModelBuilder Password(String aValue) {
            thePassword = aValue;
            return this;
        }

        public CreateUserModelBuilder Username(String aValue) {
            theUsername = aValue;
            return this;
        }


        public CreateUserModelBuilder City(String aValue) {
            theCity = aValue;
            return this;
        }

        public CreateUserModelBuilder Email(String aValue) {
            theEmail = aValue;
            return this;
        }

        public CreateUserModelBuilder States(IEnumerable<SelectListItem> aValue) {
            theStates = aValue;
            return this;
        }

        public CreateUserModelBuilder Agreement(bool aValue) {
            theAgreement = aValue;
            return this;
        }

        public CreateUserModelBuilder State(String aValue) {
            theState = aValue;
            return this;
        }

        public String FullName() {
            return theFullName;
        }

        public DateTime DateOfBirth() {
            return theDateOfBirth;
        }

        public String Password() {
            return thePassword;
        }

        public String Username() {
            return theUsername;
        }


        public String City() {
            return theCity;
        }

        public String Email() {
            return theEmail;
        }

        public IEnumerable<SelectListItem> States() {
            return theStates;
        }

        public bool Agreement() {
            return theAgreement;
        }

        public String State() {
            return theState;
        }

        public User Build() {
            theSplitName = FullName().Split(' ');
            User myUser = new User();
            myUser.Username = Username();
            myUser.Email = Email();
            myUser.DateOfBirth = DateOfBirth();
            myUser.Password = Password();
            myUser.LastName = getLastName();
            myUser.MiddleName = getMiddleName();
            myUser.FirstName = getFirstName();
            myUser.City = City();
            myUser.State = State();

        return myUser;
        }

        private String getFirstName() {
            if (theSplitName.Length != 0) {
                return FullName().Split(' ')[0];
            }

            return String.Empty;
        }

        private String getMiddleName() {
            String myMiddleName = string.Empty;
            if (theSplitName.Length > 2) {
                int myCurrentIndex = 0;
                foreach (String myNamePart in theSplitName) {
                    if (!myCurrentIndex.Equals(0) && !myCurrentIndex.Equals(theSplitName.Length - 1)) {
                        myMiddleName += theSplitName[myCurrentIndex] + " ";
                    }
                    myCurrentIndex++;
                }
            }

            return myMiddleName.Trim();
        }

        private String getLastName() {
            if (theSplitName.Length == 2) {
                return FullName().Split(' ')[1];
            } else if (theSplitName.Length > 2) {
                return theSplitName[theSplitName.Length - 1];
            }

            return String.Empty;

        }

        public String getDateOfBirthFormatted() {
            return DateOfBirth().ToString("MM-dd-yyyy");
        }
    }
}
