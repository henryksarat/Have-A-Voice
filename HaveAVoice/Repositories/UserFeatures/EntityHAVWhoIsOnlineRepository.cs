using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.UserFeatures {
    public class EntityHAVWhoIsOnlineRepository : IHAVWhoIsOnlineRepository {
        private HaveAVoiceEntities theEntities = new HaveAVoiceEntities();

        public List<WhoIsOnline> GetWhoIsOnlineEntries(User aCurrentUser, string aCurrentIpAddress) {
            return (from w in theEntities.WhoIsOnlines
                    where w.User.Id == aCurrentUser.Id
                    && w.IpAddress == aCurrentIpAddress
                    select w).ToList<WhoIsOnline>();
        }

        public void AddToWhoIsOnline(User aCurrentUser, string aCurrentIpAddress) {
            IHAVUserRepository userRepository = new EntityHAVUserRepository();
            WhoIsOnline myWhoIsOnline = WhoIsOnline.CreateWhoIsOnline(0, aCurrentUser.Id, DateTime.UtcNow, aCurrentIpAddress, false);
            theEntities.AddToWhoIsOnlines(myWhoIsOnline);
            theEntities.SaveChanges();
        }

        public void UpdateTimeOfWhoIsOnline(User aCurrentUser, string aCurrentIpAddress) {
            WhoIsOnline originalWhoIsOnline = GetWhoIsOnlineEntry(aCurrentUser, aCurrentIpAddress);
            originalWhoIsOnline.DateTimeStamp = DateTime.UtcNow;
            theEntities.ApplyCurrentValues(originalWhoIsOnline.EntityKey.EntitySetName, originalWhoIsOnline);
            theEntities.SaveChanges();
        }

        public WhoIsOnline GetWhoIsOnlineEntry(User aCurrentUser, string aCurrentIpAddress) {
            return (from w in theEntities.WhoIsOnlines
                    where w.User.Id == aCurrentUser.Id
                    && w.IpAddress == aCurrentIpAddress
                    select w).FirstOrDefault<WhoIsOnline>();
        }

        public void MarkForceLogOutOfOtherUsers(User aCurrentUser, string aCurrentIpAddress) {
            List<WhoIsOnline> otherUsers = (from w in theEntities.WhoIsOnlines
                                            where w.User.Id == aCurrentUser.Id
                                            && w.IpAddress != aCurrentIpAddress
                                            select w).ToList<WhoIsOnline>();

            foreach (WhoIsOnline onlineEntry in otherUsers) {
                onlineEntry.ForceLogOut = true;
                theEntities.ApplyCurrentValues(onlineEntry.EntityKey.EntitySetName, onlineEntry);
            }
            theEntities.SaveChanges();
        }

        public void RemoveFromWhoIsOnline(User aCurrentUser, string aCurrentIpAddress) {
            List<WhoIsOnline> onlineEntries = GetWhoIsOnlineEntries(aCurrentUser, aCurrentIpAddress);
            foreach (WhoIsOnline onlineEntry in onlineEntries) {
                theEntities.DeleteObject(onlineEntry);
            }

            theEntities.SaveChanges();
        }
    }
}