using System;
using Social.Generic.Repositories;
using Social.Generic.Models;

namespace Social.Generic.Services {
    public class BaseService<T> : IBaseService<T> {
        private static IBaseRepository<T> theBaseRespository;

        public BaseService(IBaseRepository<T> baseRepository) {
            theBaseRespository = baseRepository;
        }

        public void LogError(AbstractUserModel<T> aUserInformation, Exception exception, string details) {
            theBaseRespository.LogError(aUserInformation, exception, details);
        }
    }
}
