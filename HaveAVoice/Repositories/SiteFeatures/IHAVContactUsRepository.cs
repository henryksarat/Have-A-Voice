using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaveAVoice.Repositories.SiteFeatures {
    public interface IHAVContactUsRepository {
        void AddContactUserInquiry(string anEmail, string anInquiryType, string aInquiry);
    }
}