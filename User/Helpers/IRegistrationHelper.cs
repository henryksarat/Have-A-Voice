using Social.Generic.Models;
using Social.Validation;

namespace Social.User.Helpers {
    public interface IRegistrationStrategy<T> {
        bool ExtraFieldsAreValid(AbstractUserModel<T> aUser, IValidationDictionary aValidationDictionary);
        AbstractUserModel<T> AddFieldsToUserObject(AbstractUserModel<T> aUser);
    }
}
