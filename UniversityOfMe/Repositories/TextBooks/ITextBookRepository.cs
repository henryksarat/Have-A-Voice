using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniversityOfMe.Models;

namespace UniversityOfMe.Repositories.Textbooks {
    public interface ITextBookRepository {
        void CreateTextbook(User aCreatingUser, string aUniversityId, string aTextBookCondition, string aBookTitle,
            string aBookAuthor, string aBookImage, string aClassSubject, string aClassCourse, int anEdition, double aPrice, string aDetails, string aISBN);
        void DeleteTextBook(int aTextBookId);
        TextBook GetTextBook(int aTextBookId);
        IEnumerable<TextBook> GetTextBooksForUniversity(string aUniversityId);
        IEnumerable<TextBookCondition> GetTextBookConditions();
        TextBook GetNewestTextBook();
        void MarkTextBookAsNonActive(int aTextBookId);
        void UpdateTextBook(TextBook aTextBook);
    }
}
