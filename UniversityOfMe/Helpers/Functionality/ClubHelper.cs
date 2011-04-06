using System.Web.Mvc;
using HaveAVoice.Services.Clubs;
using Social.Validation;
using UniversityOfMe.Models;
using UniversityOfMe.Services.Professors;

namespace UniversityOfMe.Helpers.Functionality {
    public class ClubHelper {
        public static bool IsAdmin(User aUser, int aClubId) {
            IClubService myClubService = new ClubService(new ModelStateWrapper(new ModelStateDictionary()));
            return myClubService.IsAdmin(aUser, aClubId);
        }
    }
}