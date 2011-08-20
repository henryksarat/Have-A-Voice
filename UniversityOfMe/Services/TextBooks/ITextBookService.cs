using System.Collections.Generic;
using System.Web;
using Social.Generic.Models;
using UniversityOfMe.Models;
using UniversityOfMe.Models.View;

namespace UniversityOfMe.Services.TextBooks {
    public interface ITextBookService {
        bool CreateTextBook(UserInformationModel<User> aCreatingUser, TextBookViewModel aCreateTextBookModel);
        IDictionary<string, string> CreateTextBookConditionsDictionaryEntry();
        void DeleteTextBook(UserInformationModel<User> aDeletingUser, int aTextBookId);
        bool EditTextBook(UserInformationModel<User> aUserInfo, TextBookViewModel aTextBookViewModel);
        TextBook GetTextBook(int aTextBookId);
        TextBook GetTextBookForEdit(UserInformationModel<User> aUser, int aTextBookId);
        IEnumerable<TextBook> GetTextBooksForUniversity(string aUniversityId);
        bool MarkAsNotActive(UserInformationModel<User> aMarkingUser, int aTextBookId);
        IEnumerable<TextBook> SearchTextBooksWithinUniversity(string aUniversityId, string aSeachOption, string aSearchString, string aOrderByOption);
    }
}