using System.Collections.Generic;

namespace Social.Error.Services {
    public interface IErrorService<T> {
        IEnumerable<T> GetAllErrors();
    }
}