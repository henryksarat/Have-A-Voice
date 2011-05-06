using UniversityOfMe.Helpers;
using UniversityOfMe.Models;

namespace UniversityOfMe.Services.SendItems {
    public interface ISendItemsService {
        void SendItemToUser(int aToUserId, User aFromUser, SendItemOptions anItemToSend);
    }
}
