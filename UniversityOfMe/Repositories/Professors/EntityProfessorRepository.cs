﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UniversityOfMe.Services.Professors;
using UniversityOfMe.Models;

namespace UniversityOfMe.Repositories.Professors {
    public class EntityProfessorRepository : IProfessorRepository {
        private UniversityOfMeEntities theEntities = new UniversityOfMeEntities();

        public IEnumerable<Professor> GetProfessorsByUniversity(string aUniversityId) {
            return (from p in theEntities.Professors
                    where p.UniversityId == aUniversityId
                    select p).ToList<Professor>();
        }

        public void CreateProfessor(User aCreatingUser, string aUniversityId, string aFirstName, string aLastName, string aProfessorImage) {
            Professor myProfessor = Professor.CreateProfessor(0, aUniversityId, aCreatingUser.Id, aFirstName, aLastName, aProfessorImage, DateTime.UtcNow);
            theEntities.AddToProfessors(myProfessor);
            theEntities.SaveChanges();
        }

        public void CreateProfessorSuggestedPicture(User aSuggestingUser, int aProfessorId, string aProfessorImage) {
            ProfessorSuggestedPhoto mySuggested = ProfessorSuggestedPhoto.CreateProfessorSuggestedPhoto(0, aSuggestingUser.Id, aProfessorId, aProfessorImage, DateTime.UtcNow);
            theEntities.AddToProfessorSuggestedPhotos(mySuggested);
            theEntities.SaveChanges();
        }

        public Professor GetProfessor(int aProfessorId) {
            return (from p in theEntities.Professors
                    where p.Id == aProfessorId
                    select p).FirstOrDefault<Professor>();
        }

        public Professor GetProfessor(string aUniversityId, string aFirstname, string aLastname) {
            return (from p in theEntities.Professors
                    where p.UniversityId == aUniversityId
                       && p.FirstName == aFirstname
                       && p.LastName == aLastname
                    select p).FirstOrDefault<Professor>();
        }

        public IEnumerable<ProfessorReview> GetProfessorReviewsByUnversityAndName(string aUniversityId, string aProfessorName) {
            return (from pr in theEntities.ProfessorReviews
                    join p in theEntities.Professors on pr.ProfessorId equals p.Id
                    where p.UniversityId == aUniversityId
                    select pr).ToList<ProfessorReview>();
        }
    }
}