using Amazon.S3;
using Amazon.SimpleEmail;
namespace AutomatedEmail.Services {
    public interface IEmailService {
        void SendEmails(AmazonSimpleEmailServiceClient aClient);
    }
}