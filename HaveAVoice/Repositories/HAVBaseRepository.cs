using System;
using HaveAVoice.Helpers.UserInformation;
using HaveAVoice.Repositories.UserFeatures;
using HaveAVoice.Models;
using HaveAVoice.Models.View;

namespace HaveAVoice.Repositories {
    public class HAVBaseRepository : IHAVBaseRepository {
        private static HaveAVoiceEntities theEntities = new HaveAVoiceEntities();
        private static int ERROR_USER_ID = 91;

    
        protected HaveAVoiceEntities GetEntities() {
            return theEntities;
        }

        public void LogError(Exception exception, string details) {
            ResetConnection();
            IHAVUserRepository userRepository = new EntityHAVUserRepository();
            User myUser = GetUserInformaton();
            try {
                User myLoggingUser = myUser != null ?
                                myUser : userRepository.GetUser(ERROR_USER_ID);
                String exceptionMessage = exception.Message;
                String innerException = exception.InnerException != null ?
                                exception.InnerException.Message : "No inner exception";
                String stackTrace = exception.StackTrace;
                ErrorLog errorLog = ErrorLog.CreateErrorLog(0, exceptionMessage, innerException, stackTrace, DateTime.UtcNow, details);
                errorLog.User = userRepository.GetUser(myLoggingUser.Id); ;
                GetEntities().AddToErrorLogs(errorLog);
                GetEntities().SaveChanges();
            } catch {
                //Can't do shit?
            }
        }

        private User GetUserInformaton() {
            UserInformationModel myUserInformation = HAVUserInformationFactory.GetUserInformation();
            if (myUserInformation != null) {
                return myUserInformation.Details;
            }
            return null;
        }

        public void ResetConnection() {
            theEntities.Dispose();
            theEntities = null;
            theEntities = new HaveAVoiceEntities();
        }
    }
}
