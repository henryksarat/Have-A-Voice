﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Social.Generic.Models;

namespace HaveAVoice.Models.SocialWrappers {
    public class SocialBoardReplyModel : AbstractBoardReplyModel<BoardReply> {
        public static SocialBoardReplyModel Create(BoardReply anExternal) {
            return new SocialBoardReplyModel(anExternal);
        }

        public SocialBoardReplyModel() { }

        public override BoardReply FromModel() {
            BoardReply myBoardReply = BoardReply.CreateBoardReply(Id, UserId, BoardId, Message, DateTimeStamp, Deleted);
            myBoardReply.UpdatedByUserId = UpdatedByUserId;
            myBoardReply.UpdatedDateTimeStamp = UpdatedDateTimeStamp;
            myBoardReply.DeletedByUserId = DeletedByUserId;
            return myBoardReply;
        }

        private SocialBoardReplyModel(BoardReply anExternal) {
            Id = anExternal.Id;
            UserId = anExternal.UserId;
            BoardId = anExternal.BoardId;
            Message = anExternal.Message;
            DateTimeStamp = anExternal.DateTimeStamp;
            UpdatedByUserId = anExternal.UpdatedByUserId;
            UpdatedDateTimeStamp = anExternal.UpdatedDateTimeStamp;
            Deleted = anExternal.Deleted;
            DeletedByUserId = anExternal.DeletedByUserId;
        }
    }
}