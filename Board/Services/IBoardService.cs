using System.Collections.Generic;
using Social.Generic.Models;

namespace Social.Board.Services {
    public interface IBoardService<T, U, V> {
        U GetBoard(UserInformationModel<T> aUser, int boardId);
        bool DeleteBoardMessage(UserInformationModel<T> aDeletingUser, int aBoardId);
        bool EditBoardMessage(UserInformationModel<T> anEditBy, AbstractBoardModel<U> aBoard);
        bool PostToBoard(UserInformationModel<T> aPostingUser, int aSourceUserId, string aMessage);

        bool DeleteBoardReply(UserInformationModel<T> aDeletingUser, int aBoardReplyId);
        bool EditBoardReply(UserInformationModel<T> anEditedBy, AbstractBoardReplyModel<V> aBoardReply);
        V FindBoardReply(int aReplyId);
        IEnumerable<V> FindBoardRepliesForBoard(UserInformationModel<T> aUser, int aBoardId);
        bool PostReplyToBoard(UserInformationModel<T> aPostingUser, int aBoardId, string aReply);
    }
}