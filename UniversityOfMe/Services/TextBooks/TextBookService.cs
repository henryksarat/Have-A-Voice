using System;
using System.Collections.Generic;
using Social.Generic.Helpers;
using Social.Generic.Models;
using Social.Photo.Exceptions;
using Social.Photo.Helpers;
using Social.Validation;
using UniversityOfMe.Helpers.Constants;
using UniversityOfMe.Models;
using UniversityOfMe.Models.View;
using UniversityOfMe.Repositories.Textbooks;
using UniversityOfMe.Repositories.TextBooks;
using System.Linq;
using UniversityOfMe.Helpers.Textbook;

namespace UniversityOfMe.Services.TextBooks {
    public class TextBookService : ITextBookService {
        private IValidationDictionary theValidationDictionary;
        private ITextBookRepository theTextBookRepo;

        public TextBookService(IValidationDictionary aValidationDictionary)
            : this(aValidationDictionary, new EntityTextBookRepository()) { }

        public TextBookService(IValidationDictionary aValidationDictionary, ITextBookRepository aTextBookRepo) {
            theValidationDictionary = aValidationDictionary;
            theTextBookRepo = aTextBookRepo;
        }


        public bool CreateTextBook(UserInformationModel<User> aCreatingUser, CreateTextBookModel aCreateTextBookModel) {
            if (!ValidTextBook(aCreateTextBookModel)) {
                return false;
            }

            string myBookImageName = string.Empty;

            if (aCreateTextBookModel.BookImage != null) {
                try {
                    myBookImageName = SocialPhotoHelper.TakeImageAndResizeAndUpload(
                        TextBookConstants.TEXTBOOK_PHOTO_PATH, 
                        aCreateTextBookModel.BookTitle.GetHashCode().ToString(),
                        aCreateTextBookModel.BookImage,
                        TextBookConstants.BOOK_MAX_SIZE);
                } catch (Exception myException) {
                    throw new PhotoException("Unable to upload the textbook image.", myException);
                }
            }

            theTextBookRepo.CreateTextbook(aCreatingUser.Details, 
                aCreateTextBookModel.UniversityId, 
                aCreateTextBookModel.TextBookCondition,
                aCreateTextBookModel.BookTitle, 
                myBookImageName, 
                aCreateTextBookModel.ClassCode, 
                aCreateTextBookModel.BuySell,
                aCreateTextBookModel.Edition == null ? 0 : int.Parse(aCreateTextBookModel.Edition),
                double.Parse(aCreateTextBookModel.Price),
                string.IsNullOrEmpty(aCreateTextBookModel.Details) ? null : aCreateTextBookModel.Details);

            return true;
        }

        public IDictionary<string, string> CreateBuySellDictionaryEntry() {
            IDictionary<string, string> myDictionary = DictionaryHelper.DictionaryWithSelect();
            myDictionary.Add("Buy", "B");
            myDictionary.Add("Sell", "S");
            return myDictionary;
        }

        public IDictionary<string, string> CreateTextBookConditionsDictionaryEntry() {
            IDictionary<string, string> myDictionary = DictionaryHelper.DictionaryWithSelect();
            IEnumerable<TextBookCondition> myTextBookConditions = theTextBookRepo.GetTextBookConditions();
            foreach (TextBookCondition myTextBookCondition in myTextBookConditions) {
                myDictionary.Add(myTextBookCondition.Display, myTextBookCondition.ConditionId);
            }
            return myDictionary;
        }

        public TextBook GetTextBook(int aTextBookId) {
            return theTextBookRepo.GetTextBook(aTextBookId);
        }

        public IEnumerable<TextBook> GetTextBooksForUniversity(string aUniversityId) {
            return theTextBookRepo.GetTextBooksForUniversity(aUniversityId).OrderByDescending(t => t.DateTimeStamp);
        }

        public bool MarkAsNotActive(UserInformationModel<User> aMarkingUser, int aTextBookId) {
            TextBook myTextBook = GetTextBook(aTextBookId);
            if (myTextBook.UserId == aMarkingUser.Details.Id) {
                theTextBookRepo.MarkTextBookAsNonActive(aTextBookId);
                return true;
            }

            theValidationDictionary.AddError("User", aMarkingUser.UserId.ToString(), "You are not the person that created this textbook entry, you cannot mark is non-active.");
            return false;
        }

        public IEnumerable<TextBook> SearchTextBooksWithinUniversity(string aUniversityId, string aSeachOption, string aSearchString, string anOrderByOption) {
            IEnumerable<TextBook> myTextBooks = GetTextBooksForUniversity(aUniversityId);

            if (aSeachOption.Equals(TextbookSearch.TITLE)) {
                myTextBooks = (from t in myTextBooks
                               where t.BookTitle.Contains(aSearchString)
                               select t);
            } else if (aSeachOption.Equals(TextbookSearch.CLASS_CODE)) {
                myTextBooks = (from t in myTextBooks
                               where t.ClassCode.Contains(aSearchString)
                               select t);
            }

            if (anOrderByOption.Equals(TextbookSearch.LOWEST_PRICE)) {
                myTextBooks = myTextBooks.OrderBy(t => t.Price);
            } else if (anOrderByOption.Equals(TextbookSearch.HIGHEST_PRICE)) {
                myTextBooks = myTextBooks.OrderByDescending(t => t.Price);
            } else if (anOrderByOption.Equals(TextbookSearch.DATE)) {
                myTextBooks = myTextBooks.OrderBy(t => t.DateTimeStamp);
            }

            return myTextBooks;
        }

        private bool ValidTextBook(CreateTextBookModel aCreateTextBoook) {
            if (string.IsNullOrEmpty(aCreateTextBoook.UniversityId)) {
                theValidationDictionary.AddError("UniversityId", aCreateTextBoook.UniversityId, "A university must be present.");
            }
            
            if (string.IsNullOrEmpty(aCreateTextBoook.BookTitle)) {
                theValidationDictionary.AddError("BookTitle", aCreateTextBoook.BookTitle, "You must specify a book title.");
            }

            if (string.IsNullOrEmpty(aCreateTextBoook.ClassCode)) {
                theValidationDictionary.AddError("ClassCode", aCreateTextBoook.ClassCode, "You must specify which class the textbook the class was/will be used in.");
            }

            double myNum;
            if (!double.TryParse(aCreateTextBoook.Price, out myNum) || myNum < 0.01) {
                theValidationDictionary.AddError("Price", aCreateTextBoook.Price.ToString(), "The asking price must be above zero.");
            }

            int myEdition;
            if (!string.IsNullOrEmpty(aCreateTextBoook.Edition) && (!int.TryParse(aCreateTextBoook.Edition, out myEdition) || myEdition < 0)) {
                theValidationDictionary.AddError("Edition", aCreateTextBoook.Edition.ToString(), "If you specify an edition is must be non-zero and non-negative.");
            }

            if (!DropDownItemValidation.IsValid(aCreateTextBoook.BuySell)) {
                theValidationDictionary.AddError("BuySell", aCreateTextBoook.BuySell, "You must specify if you want to buy or sell the textbook.");
            }

            if (!DropDownItemValidation.IsValid(aCreateTextBoook.TextBookCondition)) {
                theValidationDictionary.AddError("TextBookCondition", aCreateTextBoook.TextBookCondition, "You must specify the condition of the textbook.");
            }

            if (aCreateTextBoook.BookImage != null && !PhotoValidation.IsValidImageFile(aCreateTextBoook.BookImage.FileName)) {
                theValidationDictionary.AddError("BookImage", aCreateTextBoook.BookImage.FileName, PhotoValidation.INVALID_IMAGE);
            }

            return theValidationDictionary.isValid;
        }
    }
}
