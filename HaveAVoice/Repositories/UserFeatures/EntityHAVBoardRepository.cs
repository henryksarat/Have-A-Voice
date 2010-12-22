using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.UserFeatures {
    public class EntityHAVBoardRepository : HAVBaseRepository, IHAVBoardRepository {
        public Board FindBoardByBoardId(int aBoardId) {
            return (from b in GetEntities().Boards
                    where b.Id == aBoardId
                    select b).FirstOrDefault<Board>();
        }

        public IEnumerable<Board> FindBoardByUserId(int aUserId) {
            return (from b in GetEntities().Boards
                    where b.User.Id == aUserId
                    && b.Deleted == false
                    select b)
                    .OrderByDescending(b => b.DateTimeStamp)
                    .ToList<Board>();
        }

        public void AddToBoard(User aPostedByUser, int aSourceUserId, string aMessage) {
            IHAVUserRepository myUserRepository = new EntityHAVUserRepository();
            Board myBoard = Board.CreateBoard(0, aSourceUserId, aPostedByUser.Id, aMessage, DateTime.UtcNow, false, false);
            GetEntities().AddToBoards(myBoard);
            GetEntities().SaveChanges();
        }

        public void EditBoardMessage(User aEditedBy, Board anOriginalBoard, Board aBoard) {
            string myOldMessage = anOriginalBoard.Message;
                AuditBoard myAudit =
                    AuditBoard.CreateAuditBoard(0, anOriginalBoard.Id, myOldMessage, DateTime.UtcNow, aEditedBy.Id);
                anOriginalBoard.Message = aBoard.Message;
                anOriginalBoard.UpdatedDateTimeStamp = DateTime.UtcNow;
                anOriginalBoard.UpdatedByUserId = aEditedBy.Id;
                GetEntities().AddToAuditBoards(myAudit);
                GetEntities().ApplyCurrentValues(anOriginalBoard.EntityKey.EntitySetName, anOriginalBoard);
                GetEntities().SaveChanges();
        }

        public void DeleteBoardMessage(User aDeletingUser, Board aBoard) {
            aBoard.Deleted = true;
            aBoard.DeletedByUserId = aDeletingUser.Id;
            GetEntities().ApplyCurrentValues(aBoard.EntityKey.EntitySetName, aBoard);
            GetEntities().SaveChanges();
        }

        public BoardReply FindBoardReplyByBoardReplyId(int aReplyId) {
            return (from br in GetEntities().BoardReplies
                    where br.Id == aReplyId
                    && br.Deleted == false
                    select br).FirstOrDefault<BoardReply>();
        }

        public IEnumerable<BoardReply> FindBoardRepliesByBoard(int aBoardId) {
            return (from br in GetEntities().BoardReplies
                    where br.Board.Id == aBoardId
                    && br.Deleted == false
                    select br).ToList<BoardReply>();
        }

        public void AddReplyToBoard(User aPostingUser, int aBoardId, string aReply) {
            BoardReply myReply = new BoardReply();
            myReply.User = GetUser(aPostingUser.Id);
            myReply.Board = FindBoardByBoardId(aBoardId);
            myReply.Message = aReply;
            myReply.DateTimeStamp = DateTime.UtcNow;
            GetEntities().AddToBoardReplies(myReply);
            GetEntities().SaveChanges();
        }

        public void EditBoardReply(User anEditedBy, BoardReply anOriginalReply, BoardReply aNewReply) {
            string myOldMessage = anOriginalReply.Message;
            AuditBoardReply myAudit =
                AuditBoardReply.CreateAuditBoardReply(0, anOriginalReply.Id, myOldMessage, DateTime.UtcNow, anEditedBy.Id);
            anOriginalReply.Message = aNewReply.Message;
            anOriginalReply.UpdatedDateTimeStamp = DateTime.UtcNow;
            anOriginalReply.UpdatedByUserId = anEditedBy.Id;
            GetEntities().AddToAuditBoardReplies(myAudit);
            GetEntities().ApplyCurrentValues(anOriginalReply.EntityKey.EntitySetName, anOriginalReply);
            GetEntities().SaveChanges();
        }

        public void DeleteBoardReply(User aDeletingUser, BoardReply aReply) {
            aReply.Deleted = true;
            aReply.DeletedByUserId = aDeletingUser.Id;
            GetEntities().ApplyCurrentValues(aReply.EntityKey.EntitySetName, aReply);
            GetEntities().SaveChanges();
        }

        private User GetUser(int anId) {
            IHAVUserRetrievalRepository myUserRetrieval = new EntityHAVUserRetrievalRepository();
            return myUserRetrieval.GetUser(anId);
        }
    }
}