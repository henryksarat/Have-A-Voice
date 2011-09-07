using System.Linq;
using UniversityOfMe.Models;
using System.Collections.Generic;
using System;
using UniversityOfMe.Repositories.Helpers;
using UniversityOfMe.Helpers.Badges;

namespace UniversityOfMe.Repositories.SendItems {
    public class EntitySendItemsRepository : ISendItemsRepository {
        private UniversityOfMeEntities theEntities = new UniversityOfMeEntities();

        public void SendItemToUser(int aToUserId, User aFromUser, int anItemEnumeration) {
            SendItem mySendItem = SendItem.CreateSendItem(0, aToUserId, aFromUser.Id, anItemEnumeration, DateTime.UtcNow, false);
            theEntities.AddToSendItems(mySendItem);
            theEntities.SaveChanges();

            BadgeHelper.AddNecessaryBadgesAndPoints(theEntities, aFromUser.Id, BadgeAction.BEER_SENT, BadgeSection.ITEM, mySendItem.Id);
            theEntities.SaveChanges();
        }

        public void MarkItemAsSeen(User aUser, int anItemId) {
            SendItem mySendItem = GetSendItem(aUser, anItemId);
            mySendItem.Seen = true;
            theEntities.ApplyCurrentValues(mySendItem.EntityKey.EntitySetName, mySendItem);
            theEntities.SaveChanges();
        }

        private SendItem GetSendItem(User aUser, int anItemId) {
            return (from i in theEntities.SendItems
                    where i.Id == anItemId
                    && i.ToUserId == aUser.Id
                    select i).FirstOrDefault<SendItem>();
        }
    }
}