using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UniversityOfMe.Models;

namespace UniversityOfMe.Helpers {
    public static class URLHelper {
        public static string AnonymousFlirtUrl(string aUniversityId) {
            return "/" + aUniversityId + "/Flirt/List";
        }

        public static string ToUrlFriendly(string aValue) {
            return aValue.Replace(' ', '_');
        }

        public static string[] FromUrlFriendly(string aValue) {
            return aValue.Replace('_', ' ').Split(' ');
        }

        public static string BuildClassReviewUrl(Class aClass) {
            return "/" + aClass.UniversityId + "/Class/Details/" + String.Join("_", aClass.ClassCode, aClass.AcademicTermId, aClass.Year) + "?classViewType=" + ClassViewType.Review;
        }

        public static string BuildClassBoardUrl(ClassBoard aClassBoard) {
            return "/" + aClassBoard.Class.UniversityId + "/ClassBoard/Details?classId=" + aClassBoard.ClassId + "&classBoardId=" + aClassBoard.Id;
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

        public static string EventUrl(Event anEvent) {
            return "/" + anEvent.UniversityId + "/Event/Details/" + anEvent.Id;
        }

        public static string BuildEventAttendUrl(Event anEvent) {
            return "/Event/Attend/" + anEvent.Id;
        }

        public static string BuildEventUnAttendUrl(Event anEvent) {
            return "/Event/Unattend/" + anEvent.Id;
        }

        public static string BuildClubUrl(Club aClub) {
            return "/" + aClub.UniversityId + "/Club/Details/" + aClub.Id;
        }

        public static string BuildOrganizationCancelRequestToJoin(Club aClub) {
            return "/ClubMember/Cancel?clubId=" + aClub.Id + "&universityId=" + aClub.UniversityId;
        }

        public static string BuildOrganizationQuitOrganization(Club aClub) {
            return "/ClubMember/Remove?clubId=" + aClub.Id + "&universityId=" + aClub.UniversityId;
        }

        public static string BuildOrganizationRequestToJoin(Club aClub) {
            return "/ClubMember/RequestToJoin?clubId=" + aClub.Id +"&universityId=" + aClub.UniversityId;
        }

        public static string BuildOrganizationSetAsInactive(Club aClub) {
            return "/Club/Deactivate?clubId=" + aClub.Id;
        }

        public static string BuildOrganizationUrl(Club aClub) {
            return "/" + aClub.UniversityId + "/Club/Details/" + aClub.Id;
        }

        public static string BuildOrganizationSetAsActive(Club aClub) {
            return "/Club/Activate?clubId=" + aClub.Id;
        }

        public static string MarkSentItemAsSeen(int anId) {
            return "/SendItems/MarkAsSeen/" + anId;
        }

        public static string MessageUrl(int aMessageId) {
            return "/Message/Details/" + aMessageId;
        }

        public static string BadgeHideUrl(int aUserBadgeId) {
            return "/Badge/Hide?id=" + aUserBadgeId;
        }

        public static string BadgeUrl(string aBadgeImage) {
            return "/Content/images/badges/" + aBadgeImage;
        }

        public static string BadgeListUrl(int anUserId) {
            return "/Badge/ListBadgesForUser?userId=" + anUserId;
        }

        public static string BadgeListUrl() {
            return "/Badge/List";
        }

        public static string BuildGeneralPostingsUrl(GeneralPosting aGeneralPosting) {
            return "/" + aGeneralPosting.UniversityId + "/GeneralPosting/Details/" + aGeneralPosting.Id;
        }

        public static string ProfileUrl(User aUser) {
            if (!string.IsNullOrEmpty(aUser.ShortUrl)) {
                return "/" + aUser.ShortUrl;
            } else {
                return "/Profile/Show/" + aUser.Id;
            }
        }

        public static string ProfileUrlForAllBoards(User aUser, bool aShowAllPhotoAlbums) {
            return "/Profile/ShowDetailed/" + aUser.Id + "?showAllBoards=true&showAllPhotoAlbums=" + aShowAllPhotoAlbums;
        }

        public static string ProfileUrlForAllPhotoAlbums(User aUser, bool aShowAllBoards) {
            return "/Profile/ShowDetailed/" + aUser.Id + "?showAllPhotoAlbums=true&showAllBoards=" + aShowAllBoards;
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

        public static string FriendListUrl(int aUserId) {
            return "/Friend/ListForUser/" + aUserId;
        }

        public static string AddFriendUrl(User aUserToAdd) {
            return "/Friend/Add/" + aUserToAdd.Id;
        }

        public static string RemoveFriendUrl(User aUserToRemove) {
            return "/Friend/Delete/" + aUserToRemove.Id;
        }

        public static string SendMessageUrl(User aUser) {
            return "/Message/Create/" + aUser.Id;
        }

        public static string SendMessageUrl(User aUser, string aSubject) {
            return "/Message/Create/" + aUser.Id + "?subject=" + aSubject;
        }

        public static string SendItemUrl(User aUser, SendItemOptions anItem) {
            return "/SendItems/SendItem/" + aUser.Id + "?sendItem=" + anItem;
        }
    }
}