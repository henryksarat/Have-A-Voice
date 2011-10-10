﻿using System;
using System.Collections.Generic;
using System.Linq;
using UniversityOfMe.Models;
using UniversityOfMe.Repositories.Textbooks;

namespace UniversityOfMe.Repositories.TextBooks {
    public class EntityTextBookRepository : ITextBookRepository {
        private UniversityOfMeEntities theEntities = new UniversityOfMeEntities();

        public void CreateTextbook(User aCreatingUser, string aUniversityId, string aTextBookCondition, string aBookTitle, string aBookAuthor, 
                                   string aBookImage, string aClassSubject, string aClassCourse, int anEdition, double anAskingPrice, string aDetails, string anISBN) {
            TextBook myTextBook = TextBook.CreateTextBook(0, aCreatingUser.Id, aUniversityId, aTextBookCondition, aBookTitle, aBookAuthor, 
                anEdition, anAskingPrice, DateTime.UtcNow, true);
            myTextBook.BookPicture = aBookImage;
            myTextBook.Details = aDetails;
            myTextBook.ISBN = anISBN;
            if (!string.IsNullOrEmpty(aClassSubject)) {
                myTextBook.ClassSubject = aClassSubject;
            }
            if (!string.IsNullOrEmpty(aClassCourse)) {
                myTextBook.ClassCourse = aClassCourse;
            }
            theEntities.AddToTextBooks(myTextBook);
            theEntities.SaveChanges();
        }

        public void DeleteTextBook(int aTextBookId) {
            TextBook myTextBook = GetTextBook(aTextBookId);
            theEntities.DeleteObject(myTextBook);
            theEntities.SaveChanges();
        }

        public TextBook GetTextBook(int aTextBookId) {
            return (from t in theEntities.TextBooks
                    where t.Id == aTextBookId
                    && t.Active == true
                    select t).FirstOrDefault<TextBook>();
        }

        public IEnumerable<TextBook> GetTextBooksForUniversity(string aUniversityId) {
            return (from t in theEntities.TextBooks
                    where t.UniversityId == aUniversityId
                    && t.Active == true
                    select t).ToList<TextBook>();
        }

        public IEnumerable<TextBookCondition> GetTextBookConditions() {
            return theEntities.TextBookConditions.ToList<TextBookCondition>();
        }

        public void MarkTextBookAsNonActive(int aTextBookId) {
            TextBook myTextBook = GetTextBook(aTextBookId);
            myTextBook.Active = false;
            theEntities.ApplyCurrentValues(myTextBook.EntityKey.EntitySetName, myTextBook);
            theEntities.SaveChanges();
        }

        public void UpdateTextBook(TextBook aTextBook) {
            theEntities.ApplyCurrentValues(aTextBook.EntityKey.EntitySetName, aTextBook);
            theEntities.SaveChanges();
        }

        public TextBook GetNewestTextBook() {
            return (from t in theEntities.TextBooks
                    where t.Active
                    orderby t.DateTimeStamp
                    select t).FirstOrDefault<TextBook>();
        }
    }
}