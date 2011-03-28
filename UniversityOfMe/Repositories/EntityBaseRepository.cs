using System;
using Social.Generic.Models;
using Social.Generic.Repositories;
using UniversityOfMe.Models;

namespace UniversityOfMe.Repositories {
    public class EntityBaseRepository : IBaseRepository<User> {
        private static UniversityOfMeEntities theEntities = new UniversityOfMeEntities();
        private static int ERROR_USER_ID = 91;

        public void LogError(AbstractUserModel<User> aUserInformation, Exception exception, string details) {
            ResetConnection();
            try {
                int myLoggingUserId = aUserInformation != null ? aUserInformation.Id : ERROR_USER_ID;
                String exceptionMessage = exception.Message;
                String innerException = exception.InnerException != null ?
                                exception.InnerException.Message : "No inner exception";
                String stackTrace = exception.StackTrace;
                ErrorLog errorLog = ErrorLog.CreateErrorLog(0, exceptionMessage, innerException, stackTrace, myLoggingUserId, DateTime.UtcNow, details);
                theEntities.AddToErrorLogs(errorLog);
                theEntities.SaveChanges();
            } catch {
                //Can't do shit?
            }
        }

        private void ResetConnection() {
            if (theEntities != null) {
                theEntities.Dispose();
                theEntities = null;
                theEntities = new UniversityOfMeEntities();
            }
        }
    }
}
