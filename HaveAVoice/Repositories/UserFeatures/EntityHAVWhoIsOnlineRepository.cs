using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.UserFeatures {
    public class EntityHAVWhoIsOnlineRepository : HAVBaseRepository, IHAVWhoIsOnlineRepository {
        public List<WhoIsOnline> GetWhoIsOnlineEntries(User aCurrentUser, string aCurrentIpAddress) {
            return (from w in GetEntities().WhoIsOnlines
                    where w.User.Id == aCurrentUser.Id
                    && w.IpAddress == aCurrentIpAddress
                    select w).ToList<WhoIsOnline>();
        }

        public void AddToWhoIsOnline(User aCurrentUser, string aCurrentIpAddress) {
            IHAVUserRepository userRepository = new EntityHAVUserRepository();
            WhoIsOnline myWhoIsOnline = WhoIsOnline.CreateWhoIsOnline(0, aCurrentUser.Id, DateTime.UtcNow, aCurrentIpAddress, false);
            GetEntities().AddToWhoIsOnlines(myWhoIsOnline);
            GetEntities().SaveChanges();
        }

        public void UpdateTimeOfWhoIsOnline(User aCurrentUser, string aCurrentIpAddress) {
            WhoIsOnline originalWhoIsOnline = GetWhoIsOnlineEntry(aCurrentUser, aCurrentIpAddress);
            originalWhoIsOnline.DateTimeStamp = DateTime.UtcNow;
            GetEntities().ApplyCurrentValues(originalWhoIsOnline.EntityKey.EntitySetName, originalWhoIsOnline);
            GetEntities().SaveChanges();
        }

        public WhoIsOnline GetWhoIsOnlineEntry(User aCurrentUser, string aCurrentIpAddress) {
            return (from w in GetEntities().WhoIsOnlines
                    where w.User.Id == aCurrentUser.Id
                    && w.IpAddress == aCurrentIpAddress
                    select w).FirstOrDefault<WhoIsOnline>();
        }

        public void MarkForceLogOutOfOtherUsers(User aCurrentUser, string aCurrentIpAddress) {
            List<WhoIsOnline> otherUsers = (from w in GetEntities().WhoIsOnlines
                                            where w.User.Id == aCurrentUser.Id
                                            && w.IpAddress != aCurrentIpAddress
                                            select w).ToList<WhoIsOnline>();

            foreach (WhoIsOnline onlineEntry in otherUsers) {
                onlineEntry.ForceLogOut = true;
                GetEntities().ApplyCurrentValues(onlineEntry.EntityKey.EntitySetName, onlineEntry);
            }
            GetEntities().SaveChanges();
        }

        public void RemoveFromWhoIsOnline(User aCurrentUser, string aCurrentIpAddress) {
            List<WhoIsOnline> onlineEntries = GetWhoIsOnlineEntries(aCurrentUser, aCurrentIpAddress);
            foreach (WhoIsOnline onlineEntry in onlineEntries) {
                GetEntities().DeleteObject(onlineEntry);
            }

            GetEntities().SaveChanges();
        }
    }
}