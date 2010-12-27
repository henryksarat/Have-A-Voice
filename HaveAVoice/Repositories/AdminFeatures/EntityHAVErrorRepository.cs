using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using HaveAVoice.Repositories;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.AdminFeatures {
    public class EntityHAVErrorRepository : IHAVErrorRepository {
        private HaveAVoiceEntities theEntities = new HaveAVoiceEntities();

        public IEnumerable<ErrorLog> GetAllErrors() {
            return (from c in theEntities.ErrorLogs.Include("User")
                    select c).ToList<ErrorLog>();
        }
    }
}