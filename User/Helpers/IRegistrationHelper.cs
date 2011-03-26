using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Social.Validation;
using Social.User.Models;

namespace Social.User.Helpers {
    public interface IRegistrationStrategy<T> {
        bool ExtraFieldsAreValid(AbstractUserModel<T> aUser, IValidationDictionary aValidationDictionary);
        AbstractUserModel<T> AddFieldsToUserObject(AbstractUserModel<T> aUser);
    }
}
