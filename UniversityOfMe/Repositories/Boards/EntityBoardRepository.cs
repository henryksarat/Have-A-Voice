using System;
using System.Collections.Generic;
using System.Linq;
using Social.Board.Repositories;
using Social.Generic.Models;
using Social.User.Repositories;
using UniversityOfMe.Models;
using UniversityOfMe.Models.Social;
using UniversityOfMe.Repositories.UserRepos;

namespace UniversityOfMe.Repositories.Boards {
    public class EntityBoardRepository : IBoardRepository<User, Board, BoardReply> {
        private UniversityOfMeEntities theEntities = new UniversityOfMeEntities();

        public void AddUserToBoardViewedState(int aUserId, Board aBoard, bool aViewedState) {
            BoardViewedState myBoardViewedState = BoardViewedState.CreateBoardViewedState(0, aBoard.Id, aUserId, aViewedState, DateTime.UtcNow);
            theEntities.AddToBoardViewedStates(myBoardViewedState);
            theEntities.SaveChanges();
        }

        public Board AddToBoard(User aPostedByUser, int aSourceUserId, string aMessage) {
            IUserRepository<User, Role, UserRole> myUserRepository = new EntityUserRepository();
            Board myBoard = Board.CreateBoard(0, aSourceUserId, aPostedByUser.Id, aMessage, DateTime.UtcNow, false);
            theEntities.AddToBoards(myBoard);
            theEntities.SaveChanges();

            return myBoard;
        }

        public void DeleteBoardMessage(User aDeletingUser, Board aBoard) {
            aBoard.Deleted = true;
            aBoard.DeletedByUserId = aDeletingUser.Id;
            theEntities.ApplyCurrentValues(aBoard.EntityKey.EntitySetName, aBoard);
            theEntities.SaveChanges();
        }

        public void EditBoardMessage(User aEditedBy, Board anOriginalBoard, Board aBoard) {
            string myOldMessage = anOriginalBoard.Message;
            AuditBoard myAudit =
                AuditBoard.CreateAuditBoard(0, anOriginalBoard.Id, myOldMessage, DateTime.UtcNow, aEditedBy.Id);
            anOriginalBoard.Message = aBoard.Message;
            anOriginalBoard.UpdatedDateTimeStamp = DateTime.UtcNow;
            anOriginalBoard.UpdatedByUserId = aEditedBy.Id;
            theEntities.AddToAuditBoards(myAudit);
            theEntities.ApplyCurrentValues(anOriginalBoard.EntityKey.EntitySetName, anOriginalBoard);
            theEntities.SaveChanges();
        }

        public Board FindBoardByBoardId(int aBoardId) {
            return (from b in theEntities.Boards
                    where b.Id == aBoardId
                    select b).FirstOrDefault<Board>();
        }

        public AbstractBoardModel<Board> FindAbstractBoardByBoardId(int aBoardId) {
            Board myBoard = FindBoardByBoardId(aBoardId);
            return SocialBoardModel.Create(myBoard);
        }

        public IEnumerable<Board> FindBoardByUserId(int aUserId) {
            return (from b in theEntities.Boards
                    where b.OwnerUserId == aUserId
                    && b.Deleted == false
                    select b)
                    .OrderByDescending(b => b.DateTimeStamp)
                    .ToList<Board>();
        }

        public void MarkBoardAsViewed(User aViewingUser, int aBoardId) {
            BoardViewedState myBoardViewState = FindBoardViewedState(aViewingUser.Id, aBoardId);
            if (myBoardViewState != null && !myBoardViewState.Viewed) {
                myBoardViewState.Viewed = true;

                theEntities.ApplyCurrentValues(myBoardViewState.EntityKey.EntitySetName, myBoardViewState);
                theEntities.SaveChanges();
            }
        }

        public void AddReplyToBoard(User aPostingUser, int aBoardId, string aReply) {
            BoardReply myReply = BoardReply.CreateBoardReply(0, aPostingUser.Id, aBoardId, aReply, DateTime.UtcNow, false);
            theEntities.AddToBoardReplies(myReply);

            UpdateCurrentBoardViewedStateAndAddIfNecessaryWithoutSave(aPostingUser, aBoardId);

            theEntities.SaveChanges();
        }

        public void DeleteBoardReply(User aDeletingUser, BoardReply aReply) {
            aReply.Deleted = true;
            aReply.DeletedByUserId = aDeletingUser.Id;
            theEntities.ApplyCurrentValues(aReply.EntityKey.EntitySetName, aReply);
            theEntities.SaveChanges();
        }

        public void EditBoardReply(User anEditedBy, BoardReply anOriginalReply, BoardReply aNewReply) {
            string myOldMessage = anOriginalReply.Message;
            AuditBoardReply myAudit =
                AuditBoardReply.CreateAuditBoardReply(0, anOriginalReply.Id, myOldMessage, DateTime.UtcNow, anEditedBy.Id);
            anOriginalReply.Message = aNewReply.Message;
            anOriginalReply.UpdatedDateTimeStamp = DateTime.UtcNow;
            anOriginalReply.UpdatedByUserId = anEditedBy.Id;
            theEntities.AddToAuditBoardReplies(myAudit);
            theEntities.ApplyCurrentValues(anOriginalReply.EntityKey.EntitySetName, anOriginalReply);
            theEntities.SaveChanges();
        }

        public BoardReply FindBoardReplyByBoardReplyId(int aReplyId) {
            return (from br in theEntities.BoardReplies
                    where br.Id == aReplyId
                    && br.Deleted == false
                    select br).FirstOrDefault<BoardReply>();
        }

        public AbstractBoardReplyModel<BoardReply> FindAbstractBoardReplyByBoardReplyId(int aReplyId) {
            BoardReply myBoardReply = FindBoardReplyByBoardReplyId(aReplyId);
            return SocialBoardReplyModel.Create(myBoardReply);
        }
        public IEnumerable<BoardReply> FindBoardRepliesByBoard(int aBoardId) {
            return (from br in theEntities.BoardReplies
                    where br.Board.Id == aBoardId
                    && br.Deleted == false
                    select br).ToList<BoardReply>();
        }

        private void UpdateCurrentBoardViewedStateAndAddIfNecessaryWithoutSave(User aPostingUser, int aBoardId) {
            bool myHasViewedState = false;

            List<BoardViewedState> myViewedStates = FindBoardViewedStatesByBoardId(aBoardId);

            foreach (BoardViewedState myViewedState in myViewedStates) {
                if (myViewedState.UserId == aPostingUser.Id) {
                    myHasViewedState = true;
                    myViewedState.Viewed = true;
                } else {
                    myViewedState.Viewed = false;
                }
                myViewedState.DateTimeStamp = DateTime.UtcNow;
                theEntities.ApplyCurrentValues(myViewedState.EntityKey.EntitySetName, myViewedState);
            }

            if (!myHasViewedState) {
                AddUserToBoardViewedStateWithoutSave(aPostingUser.Id, aBoardId, true);
            }
        }

        private void AddUserToBoardViewedStateWithoutSave(int aUserId, int aBoardId, bool aViewedState) {
            BoardViewedState myBoardViewedState = BoardViewedState.CreateBoardViewedState(0, aBoardId, aUserId, aViewedState, DateTime.UtcNow);
            theEntities.AddToBoardViewedStates(myBoardViewedState);
        }

        private BoardViewedState FindBoardViewedState(int aUserId, int aBoardId) {
            return (from v in theEntities.BoardViewedStates
                    where v.UserId == aUserId
                    && v.BoardId == aBoardId
                    select v).FirstOrDefault<BoardViewedState>();
        }

        private List<BoardViewedState> FindBoardViewedStatesByBoardId(int aBoardId) {
            return (from v in theEntities.BoardViewedStates
                    where v.BoardId == aBoardId
                    select v).ToList<BoardViewedState>();
        }
    }
}