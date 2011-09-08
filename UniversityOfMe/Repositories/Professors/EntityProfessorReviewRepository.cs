using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UniversityOfMe.Services.Professors;
using UniversityOfMe.Models;
using UniversityOfMe.Repositories.Helpers;
using UniversityOfMe.Helpers.Badges;

namespace UniversityOfMe.Repositories.Professors {
    public class EntityProfessorReviewRepository : IProfessorReviewRepository {
        private UniversityOfMeEntities theEntities = new UniversityOfMeEntities();

        public void CreateProfessorReview(User aCreatingUser, ProfessorReview aProfessorReview) {
            aProfessorReview.UserId = aCreatingUser.Id;
            aProfessorReview.DateTimeStamp = DateTime.UtcNow;

            BadgeHelper.AddNecessaryBadgesAndPoints(theEntities, aCreatingUser.Id, BadgeAction.POSTED_REVIEW, BadgeSection.PROFESSOR, aProfessorReview.ProfessorId);

            theEntities.AddToProfessorReviews(aProfessorReview);
            theEntities.SaveChanges();
        }


        public ProfessorReview GetProfessorReview(User aUserReviewing, int aProfessorId) {
            return (from r in theEntities.ProfessorReviews
                    where r.UserId == aUserReviewing.Id
                    && r.ProfessorId == aProfessorId
                    select r).FirstOrDefault<ProfessorReview>();
        }
    }
}