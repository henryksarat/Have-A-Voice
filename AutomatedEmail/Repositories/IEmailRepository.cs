using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutomatedEmail.Models;

namespace AutomatedEmail.Repositories {
    public interface IEmailRepository {
        IEnumerable<EmailJob> GetEmailJobsToBeSent();
        void MarkEmailPresentToTrue(int anId);
        void MarkEmailPostsentToTrue(int anId);
    }
}
