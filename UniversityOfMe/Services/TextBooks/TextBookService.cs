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
using Social.Admin.Helpers;
using Social.Admin.Exceptions;
using Social.Generic.Constants;
using System.Web;
using Social.Generic.Exceptions;
using UniversityOfMe.Helpers;
using UniversityOfMe.Helpers.AWS;
using UniversityOfMe.Helpers.Configuration;

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


        public bool CreateTextBook(UserInformationModel<User> aCreatingUser, TextBookViewModel aCreateTextBookModel) {
            if (!ValidTextBook(aCreateTextBookModel)) {
                return false;
            }

            string myBookImageName = string.Empty;

            if (aCreateTextBookModel.BookImage != null) {
                try {
                    myBookImageName = AWSPhotoHelper.TakeImageAndResizeAndUpload(aCreateTextBookModel.BookImage,
                        AWSHelper.GetClient(),
                        SiteConfiguration.TextbookPhotosBucket(),
                        aCreateTextBookModel.BookTitle.GetHashCode().ToString(),
                        TextBookConstants.BOOK_MAX_SIZE);
                } catch (Exception myException) {
                    throw new PhotoException("Unable to upload the textbook image.", myException);
                }
            }

            theTextBookRepo.CreateTextbook(aCreatingUser.Details, 
                aCreateTextBookModel.UniversityId, 
                aCreateTextBookModel.TextBookCondition,
                aCreateTextBookModel.BookTitle, 
                aCreateTextBookModel.BookAuthor,
                myBookImageName, 
                aCreateTextBookModel.ClassCode,
                aCreateTextBookModel.Edition == null ? 0 : int.Parse(aCreateTextBookModel.Edition),
                double.Parse(aCreateTextBookModel.Price),
                string.IsNullOrEmpty(aCreateTextBookModel.Details) ? null : aCreateTextBookModel.Details,
                aCreateTextBookModel.ISBN);

            return true;
        }

        public IDictionary<string, string> CreateTextBookConditionsDictionaryEntry() {
            IDictionary<string, string> myDictionary = DictionaryHelper.DictionaryWithSelect();
            IEnumerable<TextBookCondition> myTextBookConditions = theTextBookRepo.GetTextBookConditions();
            foreach (TextBookCondition myTextBookCondition in myTextBookConditions) {
                myDictionary.Add(myTextBookCondition.Display, myTextBookCondition.ConditionId);
            }
            return myDictionary;
        }

        public void DeleteTextBook(UserInformationModel<User> aDeletingUser, int aTextBookId) {
            TextBook myTextBook = theTextBookRepo.GetTextBook(aTextBookId);

            if (aDeletingUser.Details.Id != myTextBook.UserId) {
                if (!PermissionHelper<User>.AllowedToPerformAction(theValidationDictionary, aDeletingUser, SocialPermission.Delete_Any_Textbook)) {
                    throw new PermissionDenied(ErrorKeys.PERMISSION_DENIED);
                }
            }

            if (!string.IsNullOrEmpty(myTextBook.BookPicture)) {
                AWSPhotoHelper.PhysicallyDeletePhoto(AWSHelper.GetClient(), SiteConfiguration.TextbookPhotosBucket(), myTextBook.BookPicture);
            }

            theTextBookRepo.DeleteTextBook(myTextBook.Id);
        }

        public bool EditTextBook(UserInformationModel<User> aUserInfo, TextBookViewModel aTextBookViewModel) {
            if (!ValidTextBook(aTextBookViewModel)) {
                return false;
            }

            TextBook myTextBook = GetTextBook(aTextBookViewModel.TextBookId);
            myTextBook.BookTitle = aTextBookViewModel.BookTitle;
            myTextBook.Edition = aTextBookViewModel.Edition == null ? 0 : int.Parse(aTextBookViewModel.Edition);
            myTextBook.TextBookConditionId = aTextBookViewModel.TextBookCondition;
            myTextBook.Details = aTextBookViewModel.Details;
            myTextBook.Price = double.Parse(aTextBookViewModel.Price);
            myTextBook.ClassCode = string.IsNullOrEmpty(aTextBookViewModel.ClassCode) ? null : aTextBookViewModel.ClassCode;
            myTextBook.BookAuthor = aTextBookViewModel.BookAuthor;
            myTextBook.ISBN = string.IsNullOrEmpty(aTextBookViewModel.ISBN) ? null : aTextBookViewModel.ISBN;

            theTextBookRepo.UpdateTextBook(myTextBook);

            if (aTextBookViewModel.BookImage != null) {
                string myOldTextBookImage = myTextBook.BookPicture;

                UpdateTextBookPhoto(aTextBookViewModel.TextBookId.ToString(), aTextBookViewModel.BookImage, myTextBook);

                if (!string.IsNullOrEmpty(myOldTextBookImage)) {
                    AWSPhotoHelper.PhysicallyDeletePhoto(AWSHelper.GetClient(), SiteConfiguration.TextbookPhotosBucket(), myOldTextBookImage);
                }
            }

            return true;
        }

        public TextBook GetTextBook(int aTextBookId) {
            return theTextBookRepo.GetTextBook(aTextBookId);
        }

        public TextBook GetTextBookForEdit(UserInformationModel<User> aUser, int aTextBookId) {
            TextBook myTextbook = theTextBookRepo.GetTextBook(aTextBookId);

            if (aUser.Details.Id != myTextbook.UserId) {
                if (!PermissionHelper<User>.AllowedToPerformAction(theValidationDictionary, aUser, SocialPermission.Edit_Any_Textbook)) {
                    throw new PermissionDenied(ErrorKeys.PERMISSION_DENIED);
                }
            }
            return myTextbook;
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

        private bool ValidTextBook(TextBookViewModel aCreateTextBoook) {
            if (string.IsNullOrEmpty(aCreateTextBoook.UniversityId)) {
                theValidationDictionary.AddError("UniversityId", aCreateTextBoook.UniversityId, "A university must be present.");
            }

            if (string.IsNullOrEmpty(aCreateTextBoook.BookTitle)) {
                theValidationDictionary.AddError("BookTitle", aCreateTextBoook.BookTitle, "You must specify a book title.");
            }

            if (string.IsNullOrEmpty(aCreateTextBoook.BookAuthor)) {
                theValidationDictionary.AddError("BookAuthor", aCreateTextBoook.BookAuthor, "You must specify the book author(s).");
            }

            double myNum;
            if (!double.TryParse(aCreateTextBoook.Price, out myNum) || myNum < 0.01) {
                theValidationDictionary.AddError("Price", aCreateTextBoook.Price.ToString(), "The asking price must be above zero.");
            }

            int myEdition;
            if (!string.IsNullOrEmpty(aCreateTextBoook.Edition) && (!int.TryParse(aCreateTextBoook.Edition, out myEdition) || myEdition < 0)) {
                theValidationDictionary.AddError("Edition", aCreateTextBoook.Edition.ToString(), "If you specify an edition is must be non-zero and non-negative.");
            }

            if (!DropDownItemValidation.IsValid(aCreateTextBoook.TextBookCondition)) {
                theValidationDictionary.AddError("TextBookCondition", aCreateTextBoook.TextBookCondition, "You must specify the condition of the textbook.");
            }

            if (aCreateTextBoook.BookImage != null && !PhotoValidation.IsValidImageFile(aCreateTextBoook.BookImage.FileName)) {
                theValidationDictionary.AddError("BookImage", aCreateTextBoook.BookImage.FileName, PhotoValidation.INVALID_IMAGE);
            }

            return theValidationDictionary.isValid;
        }

        private void UpdateTextBookPhoto(string anId, HttpPostedFileBase aTextBookImage, TextBook aTextBook) {
            string myImageName = string.Empty;

            try {
                myImageName = AWSPhotoHelper.TakeImageAndResizeAndUpload(aTextBookImage,
                        AWSHelper.GetClient(),
                        SiteConfiguration.TextbookPhotosBucket(),
                        anId,
                        TextBookConstants.BOOK_MAX_SIZE);
            } catch (Exception myException) {
                throw new PhotoException("Error while resizing and uploading the textbook photo. ", myException);
            }
            try {
                aTextBook.BookPicture = myImageName;
                theTextBookRepo.UpdateTextBook(aTextBook);
            } catch (Exception myException) {
                AWSPhotoHelper.PhysicallyDeletePhoto(AWSHelper.GetClient(), SiteConfiguration.TextbookPhotosBucket(), myImageName);
                throw new CustomException("Error while updating the textbook to the new textbook photo.", myException);
            }
        }
    }
}
