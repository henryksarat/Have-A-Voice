using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HaveAVoice.Helpers {
    public interface IHAVEmail {
        void SendEmail(string toEmail, string subject, string body);
    }
}
