using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaveAVoice.Services.SiteFeatures {
    public interface IHAVContactUsService {
        bool AddContactUsInquiry(string anEmail, string anInquiryType, string anInquiry);
    }
}