using System.Collections.Generic;

namespace Social.Error.Repositories {
    public interface IErrorRepository<T> {
        IEnumerable<T> GetAllErrors();
    }
}