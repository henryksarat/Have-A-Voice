using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaveAVoice.Models.View {
    public class BoardFeedModel : FeedModel {
        public int BoardId { get; set; }
        public string Message { get; set; }
        public IEnumerable<BoardReply> BoardReplys { get; set; }

        public BoardFeedModel(User aUser) : base(aUser) {
            BoardReplys = new List<BoardReply>();
        }
    }
}