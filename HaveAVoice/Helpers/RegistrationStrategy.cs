using HaveAVoice.Models;
using Social.User.Helpers;
using Social.User.Models;
using Social.Validation;

namespace HaveAVoice.Helpers {
    public class RegistrationStrategy : IRegistrationStrategy<User> {
        public bool ExtraFieldsAreValid(Social.User.Models.AbstractUserModel<User> aUser, IValidationDictionary aValidationDictionary) {
            if (aUser.State.Trim().Length != 2) {
                aValidationDictionary.AddError("State", aUser.State.Trim(), "State is required.");
            }
            if (aUser.City.Trim().Length == 0) {
                aValidationDictionary.AddError("City", aUser.City.Trim(), "City is required.");
            }
            return aValidationDictionary.isValid;
        }

        public AbstractUserModel<User> AddFieldsToUserObject(AbstractUserModel<User> aUser) {
            return aUser;
        }
    }
}