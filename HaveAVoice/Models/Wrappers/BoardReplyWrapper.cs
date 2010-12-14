using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HaveAVoice.Models {
    public class BoardReplyWrapper {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int BoardId { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Message { get; set; }

        public DateTime DateTimeStamp { get; set; }

        public bool Deleted { get; set; }
        
        public BoardReply ToModel() {
            return BoardReply.CreateBoardReply(Id, UserId, BoardId, Message, DateTimeStamp, Deleted);
        }

        public static BoardReplyWrapper Build(BoardReply aBoardReply) {
            return new BoardReplyWrapper() {
                Id = aBoardReply.Id,
                UserId = aBoardReply.UserId,
                BoardId = aBoardReply.BoardId,
                Message = aBoardReply.Message,
                DateTimeStamp = aBoardReply.DateTimeStamp,
                Deleted = aBoardReply.Deleted
            };
        }
    }
}