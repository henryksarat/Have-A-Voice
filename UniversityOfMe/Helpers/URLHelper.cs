using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UniversityOfMe.Models;

namespace UniversityOfMe.Helpers {
    public static class URLHelper {
        public static string ToUrlFriendly(string aValue) {
            return aValue.Replace(' ', '_');
        }

        public static string[] FromUrlFriendly(string aValue) {
            return aValue.Replace('_', ' ').Split(' ');
        }

        public static string BuildClassReviewUrl(Class aClass) {
            return "/" + aClass.UniversityId + "/Class/Details/" + String.Join("_", aClass.ClassCode, aClass.AcademicTermId, aClass.Year) + "?classViewType=" + ClassViewType.Review;
        }

        public static string BuildClassDiscussionUrl(Class aClass) {
            return "/" + aClass.UniversityId + "/Class/Details/" + String.Join("_", aClass.ClassCode, aClass.AcademicTermId, aClass.Year) + "?classViewType=" + ClassViewType.Discussion;
        }

        public static string BuildProfessorUrl(Professor aProfessor) {
            return "/" + aProfessor.UniversityId + "/Professor/Details/" + ToUrlFriendly(aProfessor.FirstName + " " + aProfessor.LastName);
        }

        public static string BuildTextbookUrl(TextBook aTextbook) {
            return "/" + aTextbook.UniversityId + "/TextBook/Details/" + aTextbook.Id;
        }

        public static string BuildEventUrl(Event anEvent) {
            return "/" + anEvent.UniversityId + "/Event/Details/" + anEvent.Id;
        }

        public static string BuildClubUrl(Club aClub) {
            return "/" + aClub.UniversityId + "/Club/Details/" + aClub.Id;
        }

        public static string MessageUrl(int aMessageId) {
            return "/Message/Details/" + aMessageId;
        }

        public static string BuildGeneralPostingsUrl(GeneralPosting aGeneralPosting) {
            return "/" + aGeneralPosting.UniversityId + "/GeneralPosting/Details/" + aGeneralPosting.Id;
        }

        public static string ProfileUrl(User aUser) {
            return "/" + aUser.ShortUrl;
        }

        public static string PhotoAlbumDetailsUrl(PhotoAlbum aPhotoAlbum) {
            return "/PhotoAlbum/Details/" + aPhotoAlbum.Id;
        }

        public static string BoardDetailsUrl(Board aBoard) {
            return "/Board/Details/" + aBoard.Id;
        }

        public static string PhotoDisplayUrl(Photo aPhoto) {
            return "/Photo/Display/" + aPhoto.Id;
        }
    }
}