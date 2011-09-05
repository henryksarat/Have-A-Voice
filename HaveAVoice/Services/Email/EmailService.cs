using HaveAVoice.Helpers.Configuration;
using HaveAVoice.Repositories.Email;
using Social.Email;

namespace HaveAVoice.Services.Email {
    public class EmailService : IEmail {
        private IEmailRepository theEmailRepository;

        public EmailService()
            : this(new EntityEmailRepository()) { }

        public EmailService(IEmailRepository anEmailRepo) {
            theEmailRepository = anEmailRepo;
        }

        public void SendEmail(string anEmailDescription, string toEmail, string subject, string body) {
            theEmailRepository.AddEmailJob(anEmailDescription, SiteConfiguration.NewAccountsEmail(), toEmail, subject, body);
        }
    }
}