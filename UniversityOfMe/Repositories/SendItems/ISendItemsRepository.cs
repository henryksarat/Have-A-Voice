using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniversityOfMe.Models;

namespace UniversityOfMe.Repositories.SendItems {
    public interface ISendItemsRepository {
        void SendItemToUser(int aToUserId, User aFromUser, int anItemEnumeration);
        void MarkItemAsSeen(User aUser, int anItemId);
    }
}
