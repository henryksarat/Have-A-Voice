﻿using Social.User.Models;
using Social.User.Helpers;

namespace Social.Services.UserFeatures {
    public interface IUserService<T> {
        bool CreateUser(AbstractUserModel<T> aUserToCreate, bool aCaptchaValid, bool anAgreement, string anIpAddress, 
                        string aBaseUrl, string anActivationSubject, string anActivationBody, IRegistrationStrategy<T> aRegistrationStrategy);
    }
}