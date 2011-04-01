using Social.Generic.Models;
using Social.Validation;

namespace Social.User.Helpers {
    public interface IRegistrationStrategy<T> {
        bool ExtraFieldsAreValid(AbstractUserModel<T> aUser, IValidationDictionary aValidationDictionary);
        void PostRegistration(AbstractUserModel<T> aUser);
        void BackUpPlanForEmailNotSending(AbstractUserModel<T> aUser);
    }
}
