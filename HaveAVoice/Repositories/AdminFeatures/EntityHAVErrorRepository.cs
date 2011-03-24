using System.Collections.Generic;
using System.Linq;
using HaveAVoice.Models;
using Social.Error.Repositories;

namespace HaveAVoice.Repositories.AdminFeatures {
    public class EntityHAVErrorRepository : IErrorRepository<ErrorLog> {
        private HaveAVoiceEntities theEntities = new HaveAVoiceEntities();

        public IEnumerable<ErrorLog> GetAllErrors() {
            return (from c in theEntities.ErrorLogs.Include("User")
                    select c).ToList<ErrorLog>();
        }
    }
}