using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Repositories;
using HaveAVoice.Helpers;

namespace HaveAVoice.Services {
    public class HAVBaseService : IHAVBaseService{
        private static IHAVBaseRepository theBaseRespository;

        public HAVBaseService(IHAVBaseRepository baseRepository) {
            theBaseRespository = baseRepository;
        }

        public void LogError(Exception exception, string details) {
            theBaseRespository.LogError(exception, details);
        }

        public void ResetConnection() {
            theBaseRespository.ResetConnection();
        }
    }
}
