using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniversityOfMe.Models;

namespace UniversityOfMe.Repositories.Textbooks {
    public interface ITextBookRepository {
        void CreateTextbook(User aCreatingUser, string aUniversityId, string aTextBookCondition, string aBookTitle, string aBookImage, string aClassCode, string aBuySell, int anEdition, double aPrice, string aDetails);
        void DeleteTextBook(int aTextBookId);
        TextBook GetTextBook(int aTextBookId);
        IEnumerable<TextBook> GetTextBooksForUniversity(string aUniversityId);
        IEnumerable<TextBookCondition> GetTextBookConditions();
        void MarkTextBookAsNonActive(int aTextBookId);
        void UpdateTextBook(TextBook aTextBook);
    }
}
