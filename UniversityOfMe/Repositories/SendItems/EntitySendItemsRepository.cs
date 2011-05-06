using System.Linq;
using UniversityOfMe.Models;
using System.Collections.Generic;
using System;

namespace UniversityOfMe.Repositories.SendItems {
    public class EntitySendItemsRepository : ISendItemsRepository {
        private UniversityOfMeEntities theEntities = new UniversityOfMeEntities();

        public void SendItemToUser(int aToUserId, User aFromUser, int anItemEnumeration) {
            SendItem mySendItem = SendItem.CreateSendItem(0, aToUserId, aFromUser.Id, anItemEnumeration, DateTime.UtcNow);
            theEntities.AddToSendItems(mySendItem);
            theEntities.SaveChanges();
        }
    }
}