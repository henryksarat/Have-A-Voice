
namespace HaveAVoice.Repositories.Email {
    public interface IEmailRepository {
        void AddEmailJob(string anEmailDescription, string aFromEmail, string aToEmail, string aSubject, string aBody);
    }
}
