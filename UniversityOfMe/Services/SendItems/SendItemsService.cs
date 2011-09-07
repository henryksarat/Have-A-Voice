using UniversityOfMe.Helpers;
using UniversityOfMe.Repositories.SendItems;
using Social.Generic.Models;
using UniversityOfMe.Models;

namespace UniversityOfMe.Services.SendItems {
    public class SendItemsService : ISendItemsService {
        private ISendItemsRepository theSendItemsRepo;

        public SendItemsService() 
            : this(new EntitySendItemsRepository()) { }

        public SendItemsService(ISendItemsRepository aSendItemsRepository) {
            theSendItemsRepo = aSendItemsRepository;
        }

        public void SendItemToUser(int aToUserId, Models.User aFromUser, SendItemOptions anItemToSend) {
            theSendItemsRepo.SendItemToUser(aToUserId, aFromUser, (int)anItemToSend);
        }

        public void MarkItemAsSeen(UserInformationModel<User> aUserInfo, int anItemId) {
            theSendItemsRepo.MarkItemAsSeen(aUserInfo.Details, anItemId);
        }
    }
}