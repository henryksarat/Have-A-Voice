using System;
using System.Collections.Generic;
using System.Linq;
using Social.User.Models;
using Social.User.Repositories;
using UniversityOfMe.Models;
using UniversityOfMe.Models.Social;
using UniversityOfMe.Repositories.UserRepos;

namespace UniversityOfMe.Repositories {
    public class EntityWhoIsOnlineRepository : IWhoIsOnlineRepository<User, WhoIsOnline> {
        private UniversityOfMeEntities theEntities = new UniversityOfMeEntities();

        public void AddToWhoIsOnline(User aCurrentUser, string aCurrentIpAddress) {
            IUserRepository<User, Role, UserRole> userRepository = new EntityUserRepository();
            WhoIsOnline myWhoIsOnline = WhoIsOnline.CreateWhoIsOnline(0, aCurrentUser.Id, DateTime.UtcNow, aCurrentIpAddress, false);
            theEntities.AddToWhoIsOnlines(myWhoIsOnline);
            theEntities.SaveChanges();
        }

        public AbstractWhoIsOnlineModel<WhoIsOnline> GetAbstractWhoIsOnlineEntry(User aCurrentUser, string aCurrentIpAddress) {
            WhoIsOnline myOnline = GetWhoIsOnlineEntry(aCurrentUser, aCurrentIpAddress);
            return SocialWhoIsOnlineModel.Create(myOnline);
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

        public void UpdateTimeOfWhoIsOnline(User aCurrentUser, string aCurrentIpAddress) {
            WhoIsOnline originalWhoIsOnline = GetWhoIsOnlineEntry(aCurrentUser, aCurrentIpAddress);
            originalWhoIsOnline.DateTimeStamp = DateTime.UtcNow;
            theEntities.ApplyCurrentValues(originalWhoIsOnline.EntityKey.EntitySetName, originalWhoIsOnline);
            theEntities.SaveChanges();
        }

        private List<WhoIsOnline> GetWhoIsOnlineEntries(User aCurrentUser, string aCurrentIpAddress) {
            return (from w in theEntities.WhoIsOnlines
                    where w.User.Id == aCurrentUser.Id
                    && w.IpAddress == aCurrentIpAddress
                    select w).ToList<WhoIsOnline>();
        }

        private WhoIsOnline GetWhoIsOnlineEntry(User aCurrentUser, string aCurrentIpAddress) {
            return (from w in theEntities.WhoIsOnlines
                    where w.User.Id == aCurrentUser.Id
                    && w.IpAddress == aCurrentIpAddress
                    select w).FirstOrDefault<WhoIsOnline>();
        }
    }
}