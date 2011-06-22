using System;
using Social.Generic.Models;
using Social.Generic.Repositories;
using UniversityOfMe.Models;

namespace UniversityOfMe.Repositories {
    public class EntityBaseRepository : IBaseRepository<User> {
        private static UniversityOfMeEntities theEntities = new UniversityOfMeEntities();

        public void LogError(AbstractUserModel<User> aUserInformation, Exception exception, string details) {
            ResetConnection();
            try {
                String exceptionMessage = exception.Message;
                String innerException = exception.InnerException != null ?
                                exception.InnerException.Message : "No inner exception";
                String stackTrace = exception.StackTrace;
                ErrorLog errorLog = ErrorLog.CreateErrorLog(0, exceptionMessage, innerException, stackTrace, DateTime.UtcNow, details);
                if(aUserInformation != null) {
                    errorLog.UserId = aUserInformation.Id;
                } else {
                    errorLog.UserId = null;
                }
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
