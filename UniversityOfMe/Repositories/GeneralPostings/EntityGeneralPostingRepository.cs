using System;
using System.Collections.Generic;
using System.Linq;
using UniversityOfMe.Models;
using UniversityOfMe.Repositories.GeneralPostings;

namespace UniversityOfMe.Repositories.Classes {
    public class EntityGeneralPostingRepository : IGeneralPostingRepository {
        private UniversityOfMeEntities theEntities = new UniversityOfMeEntities();

        public GeneralPosting CreateGeneralPosting(User aCreatedByUser, string aUniversityId, string aTitle, string aBody) {
            GeneralPosting myGeneralPosting = GeneralPosting.CreateGeneralPosting(0, aCreatedByUser.Id, aUniversityId, aTitle, aBody, DateTime.UtcNow);
            theEntities.AddToGeneralPostings(myGeneralPosting);
            theEntities.SaveChanges();

            GeneralPostingViewState myViewState = GeneralPostingViewState.CreateGeneralPostingViewState(0, aCreatedByUser.Id, myGeneralPosting.Id, true, DateTime.UtcNow, false);
            theEntities.AddToGeneralPostingViewStates(myViewState);
            theEntities.SaveChanges();

            return myGeneralPosting;
        }

        public void CreateGeneralPostingReply(User aPostedByUser, int aGeneralPostingId, string aReply) {
            DateTime myDateTime = DateTime.UtcNow;

            GeneralPostingReply myGeneralPosting = GeneralPostingReply.CreateGeneralPostingReply(0, aGeneralPostingId, aPostedByUser.Id, aReply, myDateTime);
            theEntities.AddToGeneralPostingReplies(myGeneralPosting);

            GeneralPostingViewState myViewState = GetGeneralPostingViewState(aPostedByUser.Id, aGeneralPostingId);
            if (myViewState == null) {
                myViewState = GeneralPostingViewState.CreateGeneralPostingViewState(0, aPostedByUser.Id, aGeneralPostingId, true, myDateTime, false);
                theEntities.AddToGeneralPostingViewStates(myViewState);
            } else {
                myViewState.Viewed = true;
                myViewState.DateTimeStamp = myDateTime;
            }

            IEnumerable<GeneralPostingViewState> myViewStates = GetViewStatesExceptForPostingUser(aPostedByUser, aGeneralPostingId);

            foreach (GeneralPostingViewState myLocalViewState in myViewStates) {
                myLocalViewState.Viewed = false;
                myLocalViewState.DateTimeStamp = myDateTime;
            }

            //Just in case the founder wasn't added for some reason
            GeneralPosting myOriginalGeneralPosting = GetGeneralPosting(aGeneralPostingId);
            GeneralPostingViewState myFounderViewState = GetGeneralPostingViewState(myGeneralPosting.UserId, myGeneralPosting.Id);
            if (myOriginalGeneralPosting == null) {
                myFounderViewState = GeneralPostingViewState.CreateGeneralPostingViewState(0, myGeneralPosting.UserId, myGeneralPosting.Id, false, myDateTime, false);
                theEntities.AddToGeneralPostingViewStates(myFounderViewState);
            }

            theEntities.SaveChanges();
        }

        public GeneralPosting GetGeneralPosting(int aGeneralPostingId) {
            return (from g in theEntities.GeneralPostings
                    where g.Id == aGeneralPostingId
                    select g).FirstOrDefault<GeneralPosting>();
        }

        public IEnumerable<GeneralPosting> GetGeneralPostingsForUniversity(string aUniversityId) {
            return (from g in theEntities.GeneralPostings
                    where g.UniversityId == aUniversityId
                    select g).ToList<GeneralPosting>();
        }

        public void MarkGeneralPostingAsView(User aUser, int aGeneralPostingId) {
            GeneralPostingViewState myViewState = GetGeneralPostingViewState(aUser.Id, aGeneralPostingId);
            if (myViewState != null) {
                myViewState.Viewed = true;
                theEntities.SaveChanges();
            }
        }

        public void Subscribe(User aUser, int aGeneralPostingId) {
            GeneralPostingViewState myViewState = GetGeneralPostingViewState(aUser.Id, aGeneralPostingId);
            if (myViewState == null) {
                myViewState = GeneralPostingViewState.CreateGeneralPostingViewState(0, aUser.Id, aGeneralPostingId, true, DateTime.UtcNow, false);
                theEntities.AddToGeneralPostingViewStates(myViewState);
            } else {
                myViewState.Unsubscribe = false;
            }

            theEntities.SaveChanges();
        }

        public void Unsubscribe(User aUser, int aGeneralPostingId) {
            GeneralPostingViewState myViewState = GetGeneralPostingViewState(aUser.Id, aGeneralPostingId);
            myViewState.Unsubscribe = true;
            theEntities.SaveChanges();
        }

        private GeneralPostingViewState GetGeneralPostingViewState(int aUserId, int aGeneralPostingId) {
            return (from v in theEntities.GeneralPostingViewStates
                    where v.UserId == aUserId
                    && v.GeneralPostingId == aGeneralPostingId
                    && !v.Unsubscribe
                    select v).FirstOrDefault<GeneralPostingViewState>();
        }

        private IEnumerable<GeneralPostingViewState> GetViewStatesExceptForPostingUser(User aPostingUser, int aGeneralPostingId) {
            return (from v in theEntities.GeneralPostingViewStates
                    where v.UserId != aPostingUser.Id
                    && v.GeneralPostingId == aGeneralPostingId
                    && !v.Unsubscribe
                    select v);
        }
    }
}