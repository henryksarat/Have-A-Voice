using System;
using System.Collections.Generic;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.AdminFeatures {
    public interface IHAVErrorRepository {
        IEnumerable<ErrorLog> GetAllErrors();
    }
}