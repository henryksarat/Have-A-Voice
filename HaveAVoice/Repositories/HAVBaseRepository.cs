using System;
using HaveAVoice.Helpers.UserInformation;
using HaveAVoice.Models;
using Social.Generic.Models;
using Social.Generic.Repositories;

namespace HaveAVoice.Repositories {
    public class HAVBaseRepository : IBaseRepository<User> {
        private static HaveAVoiceEntities theEntities = new HaveAVoiceEntities();
        private static int ERROR_USER_ID = 91;

    
        protected HaveAVoiceEntities GetEntities() {
            return theEntities;
        }

        public void LogError(AbstractUserModel<User> aUserInformation, Exception exception, string details) {
            ResetConnection();
            try {
                int myLoggingUserId = aUserInformation != null ? aUserInformation.Id : ERROR_USER_ID;
                String exceptionMessage = exception.Message;
                String innerException = exception.InnerException != null ?
                                exception.InnerException.Message : "No inner exception";
                String stackTrace = exception.StackTrace;
                ErrorLog errorLog = ErrorLog.CreateErrorLog(0, exceptionMessage, innerException, stackTrace, myLoggingUserId, DateTime.UtcNow, details);
                GetEntities().AddToErrorLogs(errorLog);
                GetEntities().SaveChanges();
            } catch {
                //Can't do shit?
            }
        }

        private User GetUserInformaton() {
            UserInformationModel<User> myUserInformation = HAVUserInformationFactory.GetUserInformation();
            if (myUserInformation != null) {
                return myUserInformation.Details;
            }
            return null;
        }

        private void ResetConnection() {
            if (theEntities != null) {
                theEntities.Dispose();
                theEntities = null;
                theEntities = new HaveAVoiceEntities();
            }
        }
    }
}
