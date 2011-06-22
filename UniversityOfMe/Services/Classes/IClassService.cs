﻿using System.Collections.Generic;
using Social.Generic.Models;
using UniversityOfMe.Models;
using UniversityOfMe.Models.View;
using UniversityOfMe.Helpers;

namespace UniversityOfMe.Services.Classes {
    public interface IClassService {
        bool AddToClassBoard(UserInformationModel<User> aPostedByUser, int aClassId, string aReply);
        void AddToClassEnrollment(UserInformationModel<User> aStudentToEnroll, int aClassId);
        Class CreateClass(UserInformationModel<User> aCreatedByUser, CreateClassModel aCreateClassModel);
        bool CreateClassReview(UserInformationModel<User> aReviewingUser, int aClassId, string aRating, string aReview, bool anAnonymnous);
        Class GetClass(UserInformationModel<User> aViewingUser, string aClassUrlString, ClassViewType aClassViewType);
        Class GetClass(UserInformationModel<User> aViewingUser, int aClassId, ClassViewType aClassViewType);
        IEnumerable<ClassEnrollment> GetEnrolledInClass(int aClassId);
        IEnumerable<Class> GetClassesForUniversity(string aUniversityId);
        bool IsClassExists(string aClassUrlString);
        void RemoveFromClassEnrollment(UserInformationModel<User> aStudentToRemove, int aClassId);
    }
}