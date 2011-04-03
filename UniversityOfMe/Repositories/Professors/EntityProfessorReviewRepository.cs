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

        public IEnumerable<ProfessorReview> GetProfessorReviewsByUnversityAndName(string aUniversityId, string aProfessorFirstName, string aProfessorLastName) {
            return (from pr in theEntities.ProfessorReviews
                    join p in theEntities.Professors on pr.ProfessorId equals p.Id
                    where p.UniversityId == aUniversityId
                    && p.FirstName == aProfessorFirstName
                    && p.LastName == aProfessorLastName
                    select pr).ToList<ProfessorReview>();
        }
    }
}