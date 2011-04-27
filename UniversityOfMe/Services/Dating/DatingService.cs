using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Social.Generic.Constants;
using UniversityOfMe.Models;
using UniversityOfMe.Repositories.Dating;
using UniversityOfMe.Services.Users;

namespace UniversityOfMe.Services.Dating {
    public class DatingService : IDatingService {
        private IDatingRepository theDatingRepository;
        private IUofMeUserRetrievalService theUserRetrievalService;

        public DatingService()
            : this(new UofMeUserRetrievalService(), new DatingRepository()) { }

        public DatingService(IUofMeUserRetrievalService aUserRetrievalService, IDatingRepository aDatingRepository) {
            theUserRetrievalService = aUserRetrievalService;
            theDatingRepository = aDatingRepository;
        }

        public void AddDatingResult(int aSourceUserId, int anAskingUserId, bool aResponse) {
            theDatingRepository.CreateDatingLog(aSourceUserId, anAskingUserId, aResponse);
        }

        public User GetDatingMember(User anAskingUser) {
            IEnumerable<User> myUsersOfOppositeSex = new List<User>();
            if (anAskingUser.Gender.Equals(Gender.MALE)) {
                myUsersOfOppositeSex = theUserRetrievalService.GetAllFemaleUsers();
            } else {
                myUsersOfOppositeSex = theUserRetrievalService.GetAllMaleUsers();
            }

            IEnumerable<User> mySourceUsersWhereThisUserAskedAlready = theDatingRepository.GetSourceUsersWhereSpecifiedUserIsTheAskingUser(anAskingUser);
            List<User> myPossibleCandidates = new List<User>();

            foreach (User myUser in myUsersOfOppositeSex) {
                bool myExists = (from u in mySourceUsersWhereThisUserAskedAlready
                                 where u.Id == myUser.Id
                                 select u).Count<User>() > 0 ? true : false;
                if (!myExists) {
                    myPossibleCandidates.Add(myUser);
                }
            }

            Random myRandom = new Random();
            myPossibleCandidates = myPossibleCandidates.OrderBy<User, int>(u => myRandom.Next()).ToList<User>(); ;

            return myPossibleCandidates.FirstOrDefault<User>();
        }

        public void MarkDatingLogAsSeenBySourceUser(User aSourceUser, int aDatingLogId) {
            throw new NotImplementedException();
        }

        public DatingLog UserDatingMatch(User anAskingUser) {
            IEnumerable<DatingLog> myDatingLogsAsSource = theDatingRepository.GetYesDatingLogsUserHasNotSeenYet(anAskingUser);
            IEnumerable<DatingLog> myDatingLogsAsAsked = theDatingRepository.GetYesDatingLogsUserHasBeenAskedAbout(anAskingUser);

            DatingLog myMatchedDatingLog = null;

            foreach (DatingLog myDatingLog in myDatingLogsAsSource) {
                bool myIsMatch = (from d in myDatingLogsAsAsked
                                  where d.AskedUserId == myDatingLog.SourceUserId && d.SourceUserId == myDatingLog.AskedUserId
                                  select d).Count() > 0 ? true : false;

                if (myIsMatch) {
                    myMatchedDatingLog = myDatingLog;
                    break;
                }
            }

            return myMatchedDatingLog;
        }
    }
}
