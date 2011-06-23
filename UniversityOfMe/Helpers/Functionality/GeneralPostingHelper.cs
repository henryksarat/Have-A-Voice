using System.Linq;
using UniversityOfMe.Models;

namespace UniversityOfMe.Helpers.Functionality {
    public class GeneralPostingHelper {
        public static bool IsSubscribed(User aUser, GeneralPosting aGeneralPosting) {
            return (from g in aGeneralPosting.GeneralPostingViewStates
                    where g.UserId == aUser.Id
                    && !g.Unsubscribe
                    && g.GeneralPostingId == aGeneralPosting.Id
                    select g).Count<GeneralPostingViewState>() > 0 ? true : false;
        }
    }
}