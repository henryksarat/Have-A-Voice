using Social.BaseWebsite.Models;
using System.Linq;

namespace UniversityOfMe.Models.View {
    public class LoggedInWrapperModel<T> : ILoggedInModel<T> {
        public User User { get; private set; }
        private T Model;

        public LoggedInWrapperModel(User aUser) {
            User = aUser;
        }

        public T Get() {
            return Model;
        }

        public void Set(T aModel) {
            Model = aModel;
        }

        public int GetUnreadMessageCount() {
            int myUnreadReceived = (from m in User.MessagesReceived
                                    where m.ToViewed == false
                                    select m).Count<Message>();
            int myUnreadSent = (from m in User.MessagesSent
                                where m.FromViewed == false
                                && m.FromDeleted == false
                                && m.RepliedTo == true
                                select m).Count<Message>();

            return myUnreadReceived + myUnreadSent;
        }
    }
}