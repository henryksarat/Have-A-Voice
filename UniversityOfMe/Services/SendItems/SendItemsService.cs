using UniversityOfMe.Helpers;
using UniversityOfMe.Repositories.SendItems;

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
    }
}