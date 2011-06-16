using System.Collections.Generic;
using System.Web;
using Social.Generic.Models;
using UniversityOfMe.Models;
using UniversityOfMe.Models.View;

namespace UniversityOfMe.Services.TextBooks {
    public interface ITextBookService {
        bool CreateTextBook(UserInformationModel<User> aCreatingUser, CreateTextBookModel aCreateTextBookModel);
        IDictionary<string, string> CreateBuySellDictionaryEntry();
        IDictionary<string, string> CreateTextBookConditionsDictionaryEntry();
        TextBook GetTextBook(int aTextBookId);
        IEnumerable<TextBook> GetTextBooksForUniversity(string aUniversityId);
        bool MarkAsNotActive(UserInformationModel<User> aMarkingUser, int aTextBookId);
        IEnumerable<TextBook> SearchTextBooksWithinUniversity(string aUniversityId, string aSeachOption, string aSearchString, string aOrderByOption);
    }
}