using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.UserFeatures {
    public class EntityHAVWhoIsOnlineRepository : HAVBaseRepository, IHAVWhoIsOnlineRepository {
        public List<WhoIsOnline> GetWhoIsOnlineEntries(User currentUser, string currentIpAddress) {
            return (from w in GetEntities().WhoIsOnline
                    where w.User.Id == currentUser.Id
                    && w.IpAddress == currentIpAddress
                    select w).ToList<WhoIsOnline>();
        }

        public void AddToWhoIsOnline(User currentUser, string currentIpAddress) {
            IHAVUserRepository userRepository = new EntityHAVUserRepository();
            WhoIsOnline model = new WhoIsOnline();
            model.User = GetUser(currentUser.Id);
            model.DateTimeStamp = DateTime.UtcNow;
            model.IpAddress = currentIpAddress;
            GetEntities().AddToWhoIsOnline(model);
            GetEntities().SaveChanges();
        }

        public void UpdateTimeOfWhoIsOnline(User currentUser, string currentIpAddress) {
            WhoIsOnline originalWhoIsOnline = GetWhoIsOnlineEntry(currentUser, currentIpAddress);
            originalWhoIsOnline.DateTimeStamp = DateTime.UtcNow;
            GetEntities().ApplyCurrentValues(originalWhoIsOnline.EntityKey.EntitySetName, originalWhoIsOnline);
            GetEntities().SaveChanges();
        }

        public WhoIsOnline GetWhoIsOnlineEntry(User currentUser, string currentIpAddress) {
            return (from w in GetEntities().WhoIsOnline
                    where w.User.Id == currentUser.Id
                    && w.IpAddress == currentIpAddress
                    select w).FirstOrDefault<WhoIsOnline>();
        }

        public void MarkForceLogOutOfOtherUsers(User currentUser, string currentIpAddress) {
            List<WhoIsOnline> otherUsers = (from w in GetEntities().WhoIsOnline
                                            where w.User.Id == currentUser.Id
                                            && w.IpAddress != currentIpAddress
                                            select w).ToList<WhoIsOnline>();

            foreach (WhoIsOnline onlineEntry in otherUsers) {
                onlineEntry.ForceLogOut = true;
                GetEntities().ApplyCurrentValues(onlineEntry.EntityKey.EntitySetName, onlineEntry);
            }
            GetEntities().SaveChanges();
        }

        public void RemoveFromWhoIsOnline(User currentUser, string currentIpAddress) {
            List<WhoIsOnline> onlineEntries = GetWhoIsOnlineEntries(currentUser, currentIpAddress);
            foreach (WhoIsOnline onlineEntry in onlineEntries) {
                GetEntities().DeleteObject(onlineEntry);
            }

            GetEntities().SaveChanges();
        }

        private User GetUser(int anId) {
            IHAVUserRetrievalRepository myUserRetrieval = new EntityHAVUserRetrievalRepository();
            return myUserRetrieval.GetUser(anId);
        }
    }
}