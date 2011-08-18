
using UniversityOfMe.Models;
namespace UniversityOfMe.Repositories.Badges {
    public interface IBadgeRepository {
        void AddForAddedFriend(User aUser, int aToUserId);
    }
}
