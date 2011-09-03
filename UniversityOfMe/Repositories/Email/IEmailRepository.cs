using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniversityOfMe.Models;

namespace UniversityOfMe.Repositories.Email {
    public interface IEmailRepository {
        void AddEmailJob(string anEmailDescription, string aFromEmail, string aToEmail, string aSubject, string aBody);
    }
}
