using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaveAVoice.Models.Repositories.UserFeatures {
    public interface IHAVBoardRepository {
        Board FindBoardByBoardId(int aBoardId);
        IEnumerable<Board> FindBoardByUserId(int aUserId);
        void AddToBoard(User aPostedByUser, int aSourceUserId, string aMessage);
        void EditBoardMessage(User anEditedBy, Board anOriginalBoard, Board aNewBoard);
        void DeleteBoardMessage(User aDeletingUser, Board aBoard);

        BoardReply FindBoardReplyByBoardReplyId(int aReplyId);
        IEnumerable<BoardReply> FindBoardRepliesByBoard(int aBoardId);
        void AddReplyToBoard(User aPostingUser, int aBoardId, string aReply);
        void EditBoardReply(User anEditedBy, BoardReply anOriginalReply, BoardReply aNewReply);
        void DeleteBoardReply(User aDeletingUser, BoardReply aReply);
    }
}