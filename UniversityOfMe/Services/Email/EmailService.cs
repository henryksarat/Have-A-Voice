using Social.Email;
using UniversityOfMe.Repositories.Email;
using UniversityOfMe.Helpers.Configuration;

namespace UniversityOfMe.Services.Email {
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
