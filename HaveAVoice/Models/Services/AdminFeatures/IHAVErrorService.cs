using System.Collections.Generic;

namespace HaveAVoice.Models.Services.AdminFeatures {
    public interface IHAVErrorService {
        IEnumerable<ErrorLog> GetAllErrors();
    }
}