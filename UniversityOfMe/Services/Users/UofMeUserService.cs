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

namespace UniversityOfMe.Services.Users {
    public class UofMeUserService : UserService<User, Role, UserRole>, IUofMeUserService {
        private const string INVALID_EMAIL = "That is not a valid email.";

        private IValidationDictionary theValidationDictionary;
        private IUserRetrievalService<User> theUserRetrievalService;
        private IUofMeUserRepository theUserRepository;

        public UofMeUserService(IValidationDictionary aValidationDictionary)
            : this(aValidationDictionary, new UserRetrievalService<User>(new EntityUserRetrievalRepository()), new EntityUserRepository(), new SocialEmail()) { }

        public UofMeUserService(IValidationDictionary aValidationDictionary, IUserRetrievalService<User> aUserRetrievalService, IUofMeUserRepository aUserRepo, IEmail anEmail)
            : base(aValidationDictionary, aUserRepo, anEmail) {
            theValidationDictionary = aValidationDictionary;
            theUserRetrievalService = aUserRetrievalService;
            theUserRepository = aUserRepo;
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
            myOriginalUser.Email = aUser.Email;
            myOriginalUser.Password = aHashedPassword;
            myOriginalUser.FirstName = aUser.FirstName;
            myOriginalUser.LastName = aUser.LastName;
            myOriginalUser.DateOfBirth = aUser.DateOfBirth;
            myOriginalUser.City = aUser.City;
            myOriginalUser.State = aUser.State == Constants.SELECT ? string.Empty : aUser.State;
            myOriginalUser.Zip = aUser.Zip;
            myOriginalUser.Website = aUser.Website;
            myOriginalUser.Gender = aUser.Gender;
            myOriginalUser.AboutMe = aUser.AboutMe;

            theUserRepository.UpdateUser(myOriginalUser);

            return true;
        }

        public IEnumerable<User> GetNewestUsers(User aRequestingUser, string aUniversityId, int aLimit) {
            return theUserRepository.GetNewestUsersFromUniversity(aRequestingUser, aUniversityId, aLimit);
        }

        public EditUserModel GetUserForEdit(User aUser) {
            int myUserId = aUser.Id;
            User myUser = theUserRetrievalService.GetUser(myUserId);
            IEnumerable<SelectListItem> myStates =
                new SelectList(UnitedStates.STATES, myUser.State);
            IEnumerable<SelectListItem> myGenders =
                new SelectList(Constants.GENDERS, myUser.Gender);

            return new EditUserModel(myUser) {
                OriginalEmail = myUser.Email,
                OriginalFullName = NameHelper.FullName(myUser),
                OriginalGender = myUser.Gender,
                OriginalPassword = myUser.Password,
                OriginalWebsite = myUser.Website,
                States = myStates,
                Genders = myGenders,
                ProfilePictureURL = PhotoHelper.ProfilePicture(myUser)
            };
        }

        private bool ValidateEditedUser(EditUserModel aUser, string aOriginalEmail) {
            ValidEmail(aUser.Email, aOriginalEmail);
            DateOfBirthValidation.ValidDateOfBirth(theValidationDictionary, aUser.DateOfBirth);

            if (aUser.Gender.Equals(Constants.SELECT)) {
                theValidationDictionary.AddError("Gender", Constants.SELECT, "Gender is required.");
            }

            return theValidationDictionary.isValid;
        }

        private bool ValidEmail(string anEmail, string anOriginalEmail) {
            if (!EmailValidation.IsValidEmail(anEmail)) {
                theValidationDictionary.AddError("Email", anEmail.Trim(), INVALID_EMAIL);
            } else if (anOriginalEmail != null && (anOriginalEmail != anEmail)
                && (theUserRepository.EmailRegistered(anEmail))) {
                theValidationDictionary.AddError("Email", anEmail, "Someone already registered with that email. Please try another one.");
            } else {
                UniversityHelper.IsValidUniversityEmail(anEmail, theValidationDictionary);
            }

            return theValidationDictionary.isValid;
        }
    }
}
