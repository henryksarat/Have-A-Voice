using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UniversityOfMe.Services.Professors;
using UniversityOfMe.Models;

namespace UniversityOfMe.Repositories.Professors {
    public class EntityProfessorReviewRepository : IProfessorReviewRepository {
        private UniversityOfMeEntities theEntities = new UniversityOfMeEntities();

        public void CreateProfessorReview(User aCreatingUser, ProfessorReview aProfessorReview) {
            aProfessorReview.UserId = aCreatingUser.Id;
            aProfessorReview.DateTimeStamp = DateTime.UtcNow;

            theEntities.AddToProfessorReviews(aProfessorReview);
            theEntities.SaveChanges();
        }
    }
}