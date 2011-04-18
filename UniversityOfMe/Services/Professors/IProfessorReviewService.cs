﻿using System.Collections.Generic;
using UniversityOfMe.Models;
using Social.Generic.Models;

namespace UniversityOfMe.Services.Professors {
    public interface IProfessorReviewService {
        bool CreateProfessorReview(UserInformationModel<User> aReviewingUser, ProfessorReview aProfessorReview);
    }
}