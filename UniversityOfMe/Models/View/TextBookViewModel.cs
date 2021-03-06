﻿using System;
using System.ComponentModel.DataAnnotations;
using UniversityOfMe.Models;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Web;
using UniversityOfMe.Helpers;

namespace UniversityOfMe.Models.View {
    public class TextBookViewModel {
        public int TextBookId { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string UniversityId { get; set; }

        public IEnumerable<SelectListItem> TextBookConditions { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string TextBookCondition { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string BookTitle { get; set; }

        public HttpPostedFileBase BookImage { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ClassSubject { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ClassCourse { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string BookAuthor { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ISBN { get; set; }

        public string Edition { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Price { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Details { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string TextBookImageUrl { get; set; }

        public TextBookViewModel() { }

        public TextBookViewModel(TextBook aTextBook) {
            UniversityId = aTextBook.UniversityId;
            TextBookId = aTextBook.Id;
            BookTitle = aTextBook.BookTitle;
            ClassSubject = aTextBook.ClassSubject;
            ClassCourse = aTextBook.ClassCourse;
            Edition = aTextBook.Edition.ToString();
            Price = aTextBook.Price.ToString();
            Details = aTextBook.Details;
            TextBookImageUrl = PhotoHelper.TextBookPhoto(aTextBook);
            BookAuthor = aTextBook.BookAuthor;
            ISBN = aTextBook.ISBN;
        }
    }
}