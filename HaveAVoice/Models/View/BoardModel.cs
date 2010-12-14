using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

namespace HaveAVoice.Models.View {
    public class BoardModel {
        public Board Board { get; set; }
        public IEnumerable<BoardReply> BoardReplies { get; set; }

        public BoardModel() {
            Board = new Board();
            BoardReplies = new List<BoardReply>();
        }
    }
}