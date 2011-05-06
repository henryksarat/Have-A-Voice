using UniversityOfMe.Models;
using System.Collections.Generic;
using System.Linq;

namespace UniversityOfMe.Repositories.Notifications {
    public class EntityNotificationRepository : INotificationRepository {
        private UniversityOfMeEntities theEntities = new UniversityOfMeEntities();

        public IEnumerable<SendItem> GetSendItemsForUser(User aUser) {
            return (from s in theEntities.SendItems
                    where s.ToUserId == aUser.Id
                    select s).OrderByDescending(s => s.DateTimeStamp).ToList<SendItem>();
        }
    }
}