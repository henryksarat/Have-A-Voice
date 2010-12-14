using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HaveAVoice.Models.Services {
    public interface IHAVBaseService {
        void LogError(Exception exception, string details);
        void ResetConnection();
    }
}
