using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniversityOfMe.Models;

namespace UniversityOfMe.Repositories.Email {
    public interface IEmailRepository {
        void AddEmailJob(string aFromEmail, string aToEmail, string aSubject, string aBody);
        IEnumerable<EmailJob> GetEmailJobsToBeSent();
        void MarkEmailPresentToTrue(int anId);
        void MarkEmailPostsentToTrue(int anId);
    }
}
