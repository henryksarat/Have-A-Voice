using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Social.User.Models;

namespace UniversityOfMe.Models.Social {
    public class SocialWhoIsOnlineModel : AbstractWhoIsOnlineModel<WhoIsOnline> {
        public static SocialWhoIsOnlineModel Create(WhoIsOnline anExternal) {
            return new SocialWhoIsOnlineModel(anExternal);
        }

        public override WhoIsOnline FromModel() {
            return WhoIsOnline.CreateWhoIsOnline(Id, UserId, DateTimeStamp, IpAddress, ForceLogOut);
        }

        private SocialWhoIsOnlineModel(WhoIsOnline anExternal) {
            Id = anExternal.Id;
            UserId = anExternal.UserId;
            DateTimeStamp = anExternal.DateTimeStamp;
            IpAddress = anExternal.IpAddress;
            ForceLogOut = anExternal.ForceLogOut;
        }
    }
}