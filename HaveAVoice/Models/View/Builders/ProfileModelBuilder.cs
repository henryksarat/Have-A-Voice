using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaveAVoice.Models.View.Builders {
    public class ProfileModelBuilder {
        private User theUser;
        private IEnumerable<IssueReply> theIssueReplys;
        private string theBoardMessage;
        private IEnumerable<Board> theBoardMessages;
        private bool theIsFan;
        private IEnumerable<Fan> theFans;
        private IEnumerable<Fan> theFansOf;

        public ProfileModelBuilder(User aUser) {
                theUser = aUser;
                theIssueReplys = new List<IssueReply>();
                theBoardMessage = string.Empty;
                theBoardMessages = new List<Board>();
                theIsFan = false;
                theFans = new List<Fan>();
                theFansOf = new List<Fan>();
        }

        public ProfileModelBuilder SetFans(IEnumerable<Fan> aFans) {
            theFans = aFans;
            return this;
        }

        public ProfileModelBuilder SetFansOf(IEnumerable<Fan> aFansOf) {
            theFansOf = aFansOf;
            return this;
        }

        public ProfileModelBuilder SetBoardMessage(string aMessage) {
            theBoardMessage = aMessage;
            return this;
        }

        public ProfileModelBuilder SetIssueReplys(IEnumerable<IssueReply> aIssueReplys) {
            theIssueReplys = aIssueReplys;
            return this;
        }

        public ProfileModelBuilder SetBoardMessages(IEnumerable<Board> aBoardMessages) {
            theBoardMessages = aBoardMessages;
            return this;
        }

        public ProfileModelBuilder SetIsFan(bool anIsFan) {
            theIsFan = anIsFan;
            return this;
        }

        public User GetUser() {
            return theUser;
        }

        public string GetBoardMessage() {
            return theBoardMessage;
        }

        public IEnumerable<IssueReply> GetIssueReplys() {
            return theIssueReplys;
        }

        public IEnumerable<Board> GetBoardMessages() {
            return theBoardMessages;
        }

        public bool GetIsFan() {
            return theIsFan;
        }

        public IEnumerable<Fan> GetFans() {
            return theFans;
        }

        public IEnumerable<Fan> GetFansOf() {
            return theFansOf;
        }

        public ProfileModel Build() {
            return new ProfileModel(this);
        }
    }
}