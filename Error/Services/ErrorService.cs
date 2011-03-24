using System.Collections.Generic;
using Social.Error.Repositories;

namespace Social.Error.Services {
    public class ErrorService<T> : IErrorService<T> {
        private IErrorRepository<T> theRepository;

        public ErrorService(IErrorRepository<T> aRepository) {
            theRepository = aRepository;
        }

        public IEnumerable<T> GetAllErrors() {
            return theRepository.GetAllErrors();
        }
    }
}