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
            return aValue.Replace(' ', '-');
        }

        public static string[] FromUrlFriendly(string aValue) {
            return aValue.Replace('-', ' ').Split(' ');
        }

        public static string FromUrlFriendlyToNormalString(string aValue) {
            return aValue.Replace('-', ' ');
        }

        public static string BuildClassReviewUrl(Class aClass) {
            return "/" + aClass.UniversityId + "/Class/Details/" + ToUrlFriendly(aClass.Subject + " " + aClass.Course);
        }

        public static string BuildClassDiscussionUrl(Class aClass) {
            return "/" + aClass.UniversityId + "/Class/Details/" + ToUrlFriendly(aClass.Subject + " " + aClass.Course);
        }

        public static string BuildTextbookUrl(TextBook aTextbook) {
            return "/" + aTextbook.UniversityId + "/TextBook/Details/" + aTextbook.Id;
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

        public static string SearchAllClasses() {
            return "/Search/Class?searchString=&page=1";
        }

        public static string SearchTextbooksByClass() {
            return "/Search/TextbookAdvanced?searchString=&page=1&searchBy=ClassCode&orderBy=LowestPrice";
        }

        public static string SearchTextbooksByTitle() {
            return "/Search/TextbookAdvanced?searchString=&page=1&searchBy=Title&orderBy=LowestPrice";
        }

        public static string SearchTextbooks(string aClassSubject, string aClassCourse) {
            return "/Search/TextbookAdvanced?searchString=" + aClassSubject + aClassCourse + "&page=1&searchBy=ClassCode&orderBy=LowestPrice";
        }

        public static string SearchMarketplace(ItemType anItemType) {
            return "/Search/Marketplace?page=1&itemType=" + anItemType.Id + "";
        }

        public static string CreateTextBook(string aUnviersityId, string aClassSubject, string aClassCourse) {
            return "/" + aUnviersityId + "/Textbook/Create?classSubject=" + aClassSubject + "&classCourse=" + aClassCourse;
        }

        public static string ItemDetails(MarketplaceItem anItem) {
            return "/" + anItem.UniversityId + "/Marketplace/Details/" + anItem.Id;
        }
    }
}