using System.Collections.Generic;
using HaveAVoice.Models;

namespace HaveAVoice.Services.AdminFeatures {
    public interface IHAVErrorService {
        IEnumerable<ErrorLog> GetAllErrors();
    }
}