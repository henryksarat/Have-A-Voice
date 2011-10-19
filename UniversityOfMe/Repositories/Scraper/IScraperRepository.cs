﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniversityOfMe.Models;

namespace UniversityOfMe.Repositories.Scraper {
    public interface IScraperRepository {
        bool ProfessorExists(string aUniversityId, string aFirstname, string aLastName);
        Professor CreateProfessor(string aUniversityId, int aCreatedById, string aFirstName, string aLastName);
        Professor GetProfessor(string aUniversityId, string aFirstname, string aLastName);

        bool ClassExists(string aUniversityId, string aSubject,
                         string aCourse);
        Class CreateClass(string aUniversityId, int aCreatedById,
                         string aSubject, string aCourse, string aTitle);
        Class GetClass(string aUniversityId, string aSubject,
                       string aCourse);


        void CreateClassProfessor(int aProfessorId, int aClassId);
    }
}
