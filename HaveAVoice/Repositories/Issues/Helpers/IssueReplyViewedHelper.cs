using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.Issues.Helpers {
    public class IssueReplyViewedHelper {
        public static void CreateIssueReplyViewedState(HaveAVoiceEntities anEntities, int aUserId, int anIssueReplyId, bool aViewed) {
            IssueReplyViewedHelper.CreateIssueReplyViewedStateWithoutSave(anEntities, aUserId, anIssueReplyId, aViewed);
            anEntities.SaveChanges();
        }

        public static IssueReplyViewedState GetIssueReplyViewedState(HaveAVoiceEntities anEntities, int aUserId, int anIssueReplyId) {
            return (from v in anEntities.IssueReplyViewedStates
                    where v.UserId == aUserId
                    && v.IssueReplyId == anIssueReplyId
                    select v).FirstOrDefault<IssueReplyViewedState>();
        }

        public static void UpdateCurrentIssueReplyViewedStateAndAddIfNecessaryWithoutSave(HaveAVoiceEntities anEntities, int aUserId, int anIssueReplyId) {
            bool myHasViewedState = false;
            bool myAuthorHasViewedState = false;

            IEnumerable<IssueReplyViewedState> myViewedStates = GetIssueReplyViewedStates(anEntities, anIssueReplyId);

            IssueReply myIssueReply = GetIssueReply(anIssueReplyId);
            int myAuthorId = myIssueReply.UserId;

            foreach (IssueReplyViewedState myViewedState in myViewedStates) {
                if (myViewedState.UserId == aUserId) {
                    myHasViewedState = true;
                    myViewedState.Viewed = true;
                } else {
                    myViewedState.Viewed = false;
                }


                if (myViewedState.UserId == myAuthorId) {
                    myAuthorHasViewedState = true;
                }

                myViewedState.LastUpdated = DateTime.UtcNow;
                anEntities.ApplyCurrentValues(myViewedState.EntityKey.EntitySetName, myViewedState);
            }

            if (!myHasViewedState) {
                IssueReplyViewedHelper.CreateIssueReplyViewedStateWithoutSave(anEntities, aUserId, anIssueReplyId, true);
            }

            if (!myAuthorHasViewedState) {
                IssueReplyViewedHelper.CreateIssueReplyViewedStateWithoutSave(anEntities, myAuthorId, anIssueReplyId, false);
            }
        }

        private static IssueReply GetIssueReply(int anIssueReplyId) {
            IHAVIssueReplyRepository myIssueReplyRepo = new EntityHAVIssueReplyRepository();
            return myIssueReplyRepo.GetIssueReply(anIssueReplyId);
        }

        private static IEnumerable<IssueReplyViewedState> GetIssueReplyViewedStates(HaveAVoiceEntities anEntities, int anIssueReplyId) {
            return (from v in anEntities.IssueReplyViewedStates
                    where v.IssueReplyId == anIssueReplyId
                    select v).ToList<IssueReplyViewedState>();
        }

        private static void CreateIssueReplyViewedStateWithoutSave(HaveAVoiceEntities aEntities, int aUserId, int anIssueReplyId, bool aViewed) {
            IssueReplyViewedState myViewedState = IssueReplyViewedState.CreateIssueReplyViewedState(0, anIssueReplyId, aUserId, aViewed, DateTime.UtcNow);
            aEntities.AddToIssueReplyViewedStates(myViewedState);
        }
    }
}