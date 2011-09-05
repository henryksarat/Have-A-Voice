using System.Collections.Generic;
using System.Web.Mvc;
using Social.Email;
using Social.Generic.Constants;
using Social.User.Services;
using Social.Validation;
using UniversityOfMe.Helpers;
using UniversityOfMe.Models;
using UniversityOfMe.Models.View;
using UniversityOfMe.Repositories.UserRepos;
using Social.Generic.Helpers;
using Social.Generic.Exceptions;
using System;
using System.Text.RegularExpressions;
using System.Linq;
using UniversityOfMe.Services.Email;

namespace UniversityOfMe.Services.Users {
    public class UofMeUserService : UserService<User, Role, UserRole>, IUofMeUserService {
        private const string INVALID_EMAIL = "That is not a valid email.";
        private const string CHANGE_EMAIL_SUBJECT = "UniversityOf.Me | Email Change Confirmattion";
        private const string CHANGE_EMAIL_BODY = "Hello!\nTo change your account email please click the following link and enter your old email address: ";

        private IValidationDictionary theValidationDictionary;
        private IUofMeUserRetrievalService theUserRetrievalService;
        private IUofMeUserRepository theUserRepository;

        public UofMeUserService(IValidationDictionary aValidationDictionary)
            : this(aValidationDictionary, new UofMeUserRetrievalService(), new EntityUserRepository(), new EmailService()) { }

        public UofMeUserService(IValidationDictionary aValidationDictionary, IUofMeUserRetrievalService aUserRetrievalService, IUofMeUserRepository aUserRepo, IEmail anEmail)
            : base(aValidationDictionary, aUserRepo, anEmail) {
            theValidationDictionary = aValidationDictionary;
            theUserRetrievalService = aUserRetrievalService;
            theUserRepository = aUserRepo;
        }

        public bool ChangeEmail(string anOldEmail, string aNewEmailHash) {
            if (string.IsNullOrEmpty(anOldEmail)) {
                theValidationDictionary.AddError("OldEmail", anOldEmail, "You must enter the old email you are changing from.");
                return false;
            }

            User myUser = theUserRetrievalService.GetUserByChangeEmailInformation(anOldEmail, aNewEmailHash);
            if (myUser == null) {
                theValidationDictionary.AddError("OldEmail", anOldEmail, "We can't create a match between the link you clicked and the old email you entered. Please click the link again and try again or try changing your email again.");
                return false;
            }

            User myPotentialNewEmailTakenByUser = theUserRetrievalService.GetUser(myUser.NewEmail);
            if (myPotentialNewEmailTakenByUser != null) {
                theValidationDictionary.AddError("NewEmail", myUser.NewEmail, "That email is already the active email of someone else. " +
                                                                              "If this is incorrect please contact us by clicking \"Contact Us\" at " +
                                                                              "the bottom of the page and describing the problem.");
                return false;
            }

            myUser.Email = myUser.NewEmail;
            myUser.NewEmail = null;
            myUser.NewEmailHash = null;

            theUserRepository.UpdateUserEmailAndUniversities(myUser);

            return true;
        }

        public bool EditUser(EditUserModel aUser, string aHashedPassword) {
            if (!ValidateEditedUser(aUser, aUser.OriginalEmail)) {
                return false;
            }

            if ((!string.IsNullOrEmpty(aUser.NewPassword) || !string.IsNullOrEmpty(aUser.RetypedPassword))
                && !PasswordValidation.ValidPassword(theValidationDictionary, aUser.NewPassword, aUser.RetypedPassword)) {
                return false;
            }

            User myOriginalUser = theUserRetrievalService.GetUser(aUser.Id);
            myOriginalUser.NewEmail = aUser.NewEmail.Trim();
            myOriginalUser.Password = aHashedPassword;
            myOriginalUser.FirstName = aUser.FirstName.Trim();
            myOriginalUser.LastName = aUser.LastName.Trim();
            myOriginalUser.DateOfBirth = aUser.DateOfBirth;
            myOriginalUser.City = aUser.City.Trim();
            myOriginalUser.State = aUser.State == Constants.SELECT ? null : aUser.State;
            myOriginalUser.Zip = aUser.Zip;
            myOriginalUser.Website = aUser.Website.Trim();
            myOriginalUser.Gender = aUser.Gender.Trim();
            myOriginalUser.AboutMe = aUser.AboutMe.Trim();
            myOriginalUser.Job = aUser.Job.Trim();
            myOriginalUser.RelationshipStatus = (aUser.RelationshipStatus == Constants.SELECT ? null :aUser.RelationshipStatus);

            if (!string.IsNullOrEmpty(aUser.ShortUrl)) {
                myOriginalUser.ShortUrl = aUser.ShortUrl;
            }

            if (!string.IsNullOrEmpty(aUser.NewEmail)) {
                myOriginalUser.NewEmailHash = HashHelper.DoHash(aUser.NewEmail + myOriginalUser.Id);
            }

            theUserRepository.UpdateUser(myOriginalUser);

            if (!string.IsNullOrEmpty(aUser.NewEmail)) {
                string myUrl = UOMConstants.BASE_URL + "/Authentication/ChangeEmail/" + myOriginalUser.NewEmailHash;
                IEmail myEmail = new EmailService();
                try {
                    myEmail.SendEmail("CHANGE EMAIL", aUser.NewEmail, CHANGE_EMAIL_SUBJECT, CHANGE_EMAIL_BODY + myUrl);
                } catch (Exception myException) {
                    throw new CustomException("An error occurred while trying to send out the email to confirm you changing your email. " +
                                              "However, all your other changes have been saved. Please try changing your email again.", myException);
                }
            }

            return true;
        }

        public IEnumerable<User> GetNewestUsers(User aRequestingUser, string aUniversityId, int aLimit) {
            return theUserRepository.GetNewestUsersFromUniversity(aRequestingUser, aUniversityId, aLimit);
        }

        public int GetNumberOfRegisteredUsers() {
            return theUserRepository.GetRegisteredUsers().Count<User>();
        }

        public EditUserModel GetUserForEdit(User aUser) {
            int myUserId = aUser.Id;
            User myUser = theUserRetrievalService.GetUser(myUserId);
            IEnumerable<SelectListItem> myStates =
                new SelectList(UnitedStates.STATES, myUser.State);
            IEnumerable<SelectListItem> myGenders =
                new SelectList(Constants.GENDERS, myUser.Gender);

            IEnumerable<RelationshipStatu> myAllRelationships = theUserRepository.GetAllRelationshipStatus();
            IDictionary<string, string> myRelationshipsDictionary = DictionaryHelper.DictionaryWithSelect();
            foreach (RelationshipStatu myRelationship in myAllRelationships) {
                myRelationshipsDictionary.Add(myRelationship.DisplayName, myRelationship.Name);
            }

            string mySelectedRelationship = string.IsNullOrEmpty(myUser.RelationshipStatus) ? Constants.SELECT : myUser.RelationshipStatus;

            IEnumerable<SelectListItem> myRelationships =
                new SelectList(myRelationshipsDictionary, "Value", "Key", mySelectedRelationship);

            return new EditUserModel(myUser) {
                OriginalEmail = myUser.Email,
                OriginalFullName = NameHelper.FullName(myUser),
                OriginalGender = myUser.Gender,
                OriginalPassword = myUser.Password,
                OriginalWebsite = myUser.Website,
                States = myStates,
                Genders = myGenders,
                RelationshipStatu = myRelationships,
                ProfilePictureURL = PhotoHelper.ProfilePicture(myUser),
                Job = myUser.Job
            };
        }

        private bool ValidateEditedUser(EditUserModel aUser, string aOriginalEmail) {
            ValidEmail(aUser.NewEmail, aOriginalEmail);
            DateOfBirthValidation.ValidDateOfBirth(theValidationDictionary, aUser.DateOfBirth);

            if (aUser.FirstName.Trim().Length == 0) {
                theValidationDictionary.AddError("FirstName", aUser.FirstName.Trim(), "First name is required.");
            }
            if (aUser.LastName.Trim().Length == 0) {
                theValidationDictionary.AddError("LastName", aUser.LastName.Trim(), "Last name is required.");
            }

            if (aUser.Gender.Equals(Constants.SELECT)) {
                theValidationDictionary.AddError("Gender", Constants.SELECT, "Gender is required.");
            }

            if (!VarCharValidation.Valid100Length(aUser.Job.Trim())) {
                theValidationDictionary.AddError("Job", aUser.Job.Trim(), "Job can only be a maximum of 100 characters.");
            }

            if (!string.IsNullOrEmpty(aUser.ShortUrl)) {
                if (theUserRepository.ShortUrlTaken(aUser.ShortUrl)) {
                    theValidationDictionary.AddError("ShortUrl", aUser.ShortUrl, "That UniversityOf.Me URL is already taken by another member.");
                } else {
                    Regex myRegex = new Regex(RegexHelper.OnlyCharactersAndNumbersAndPeriods(), RegexOptions.IgnoreCase);
                    Match myMatch = myRegex.Match(aUser.ShortUrl);
                    if (!myMatch.Success) {
                        theValidationDictionary.AddError("ShortUrl", aUser.ShortUrl, "The UniversityOf.Me URL can only consist of letters, numbers, and periods.");
                    }
                }
            }

            return theValidationDictionary.isValid;
        }

        private bool ValidEmail(string anEmail, string anOriginalEmail) {
            if (!String.IsNullOrEmpty(anEmail) && !EmailValidation.IsValidEmail(anEmail)) {
                theValidationDictionary.AddError("Email", anEmail.Trim(), INVALID_EMAIL);
            } else if (anOriginalEmail != null && (anOriginalEmail != anEmail)
                && (theUserRepository.EmailRegistered(anEmail))) {
                theValidationDictionary.AddError("Email", anEmail, "Someone already registered with that email. Please try another one.");
            } else if (!String.IsNullOrEmpty(anEmail)) {
                UniversityHelper.IsValidUniversityEmail(anEmail, theValidationDictionary);
            }

            return theValidationDictionary.isValid;
        }
    }
}
