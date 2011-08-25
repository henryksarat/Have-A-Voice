using System.Collections.Generic;
using Social.Generic.Models;

namespace Social.Board.Repositories {
    public interface IBoardRepository<T, U, V> {
        void AddUserToBoardViewedState(int aUserId, U aBoard, bool aViewedState);
        U AddToBoard(T aPostedByUser, int aSourceUserId, string aMessage);
        void DeleteBoardMessage(T aDeletingUser, U aBoard);
        void EditBoardMessage(T anEditedBy, U anOriginalBoard, U aNewBoard);
        U FindBoardByBoardId(int aBoardId);
        T GetSourceUserForBoard(int aBoardId);
        AbstractBoardModel<U> FindAbstractBoardByBoardId(int aBoardId);
        IEnumerable<U> FindBoardByUserId(int aUserId);
        void MarkBoardAsViewed(T aViewingUser, int aBoardId);

        void AddReplyToBoard(T aPostingUser, int aBoardId, string aReply);
        void DeleteBoardReply(T aDeletingUser, V aReply);
        void EditBoardReply(T anEditedBy, V anOriginalReply, V aNewReply);
        V FindBoardReplyByBoardReplyId(int aReplyId);
        AbstractBoardReplyModel<V> FindAbstractBoardReplyByBoardReplyId(int aReplyId);
        IEnumerable<V> FindBoardRepliesByBoard(int aBoardId);
    }
}