using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniversityOfMe.Models;

namespace UniversityOfMe.Repositories.Professors {
    public interface IProfessorReviewRepository {
        void CreateProfessorReview(User aCreatingUser, ProfessorReview aProfessorReview);
        ProfessorReview GetProfessorReview(User aUserReviewing, int aProfessorId);
    }
}
