using System;
using System.Collections.Generic;

namespace HaveAVoice.Models.Repositories.AdminFeatures {
    public interface IHAVErrorRepository {
        IEnumerable<ErrorLog> GetAllErrors();
    }
}