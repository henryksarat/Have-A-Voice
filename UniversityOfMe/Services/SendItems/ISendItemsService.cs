using UniversityOfMe.Helpers;
using UniversityOfMe.Models;
using Social.Generic.Models;

namespace UniversityOfMe.Services.SendItems {
    public interface ISendItemsService {
        void SendItemToUser(int aToUserId, User aFromUser, SendItemOptions anItemToSend);
        void MarkItemAsSeen(UserInformationModel<User> aUserInfo, int anItemId);
    }
}
