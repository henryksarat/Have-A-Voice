using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HaveAVoice.Models.Repositories {
    public interface IHAVBaseRepository {
        void ResetConnection();
        void LogError(Exception exception, string details);
    }
}
