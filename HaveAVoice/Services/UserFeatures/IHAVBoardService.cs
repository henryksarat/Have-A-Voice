using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models.View;
using HaveAVoice.Models;
using Social.Generic.Models;

namespace HaveAVoice.Services.UserFeatures {
    public interface IHAVBoardService {
        Board GetBoard(UserInformationModel<User> aUser, int boardId);
        bool PostToBoard(UserInformationModel<User> aPostingUser, int aSourceUserId, string aMessage);
        bool EditBoardMessage(UserInformationModel<User> anEditBy, Board aBoard);
        bool DeleteBoardMessage(UserInformationModel<User> aDeletingUser, int aBoardId);

        BoardReply FindBoardReply(int aReplyId);
        IEnumerable<BoardReply> FindBoardRepliesForBoard(UserInformationModel<User> aUser, int aBoardId);
        bool PostReplyToBoard(UserInformationModel<User> aPostingUser, int aBoardId, string aReply);
        bool EditBoardReply(UserInformationModel<User> anEditedBy, BoardReply aBoardReply);
        bool DeleteBoardReply(UserInformationModel<User> aDeletingUser, int aBoardReplyId);
    }
}