using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HaveAVoice.Models {
    public class BoardWrapper {
        public int Id { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Message { get; set; }

        public int OwnerUserId { get; set; }

        public int PostedUserId { get; set; }

        public DateTime DateTimeStamp { get; set; }

        public bool Viewed { get; set; }

        public bool Deleted { get; set; }
        
        public Board ToModel() {
            return Board.CreateBoard(Id, 0, 0, Message, DateTime.UtcNow, false, false);
        }

        public static BoardWrapper Build(Board aBoard) {
            return new BoardWrapper() {
                Id = aBoard.Id,
                Message = aBoard.Message,
                OwnerUserId = aBoard.OwnerUserId,
                PostedUserId = aBoard.PostedUserId,
                DateTimeStamp = aBoard.DateTimeStamp,
                Viewed = aBoard.Viewed,
                Deleted = aBoard.Deleted
            };
        }
    }
}