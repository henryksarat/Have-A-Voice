﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models.View;

namespace HaveAVoice.Models.Services.UserFeatures {
    public interface IHAVBoardService {
        Board FindBoard(int aBoardId);
        BoardModel GetBoard(UserInformationModel aUser, int boardId);
        bool PostToBoard(UserInformationModel aPostingUser, int aSourceUserId, string aMessage);
        bool EditBoardMessage(UserInformationModel anEditBy, Board aBoard);
        bool DeleteBoardMessage(UserInformationModel aDeletingUser, int aBoardId);

        BoardReply FindBoardReply(int aReplyId);
        IEnumerable<BoardReply> FindBoardRepliesForBoard(UserInformationModel aUser, int aBoardId);
        bool PostReplyToBoard(UserInformationModel aPostingUser, int aBoardId, string aReply);
        bool EditBoardReply(UserInformationModel anEditedBy, BoardReply aBoardReply);
        bool DeleteBoardReply(UserInformationModel aDeletingUser, int aBoardReplyId);
    }
}