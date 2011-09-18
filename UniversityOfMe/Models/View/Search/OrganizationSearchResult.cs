using System.Linq;
using System.Web.Mvc;
using UniversityOfMe.Helpers;
using UniversityOfMe.Helpers.Functionality;
using Social.Generic.Models;

namespace UniversityOfMe.Models.View.Search {
    public class OrganizationSearchResult : ISearchResult {
        public UserInformationModel<User> UserInformationModel { private get; set; }
        private Club theOrganization;

        public OrganizationSearchResult(Club aClub) {
            theOrganization = aClub;
        }

        public string CreateResult() {
            var myClubDiv = new TagBuilder("div");
            myClubDiv.AddCssClass("res-con organization clearfix");

            var myActionsDiv = new TagBuilder("div");
            myActionsDiv.AddCssClass("actions");

            var myClubAction = new TagBuilder("a");
            if (UserInformationModel != null && ClubHelper.IsAdmin(UserInformationModel.Details, theOrganization.Id)) {
                myClubAction.AddCssClass("remove");
                myClubAction.MergeAttribute("href", URLHelper.BuildOrganizationSetAsActive(theOrganization));
                myClubAction.InnerHtml += "Set Club As Inactive";
            } else if (UserInformationModel != null && ClubHelper.IsAdmin(UserInformationModel.Details, theOrganization.Id) && !theOrganization.Active) {
                myClubAction.AddCssClass("add");
                myClubAction.MergeAttribute("href", URLHelper.BuildOrganizationSetAsActive(theOrganization));
                myClubAction.InnerHtml += "Set Club As Active";
            } else if (UserInformationModel != null && ClubHelper.IsPending(UserInformationModel.Details, theOrganization.Id)) {
                myClubAction.AddCssClass("remove");
                myClubAction.MergeAttribute("href", URLHelper.BuildOrganizationCancelRequestToJoin(theOrganization));
                myClubAction.InnerHtml += "Cancel My Request to Join";
            } else if (UserInformationModel != null && ClubHelper.IsMember(UserInformationModel.Details, theOrganization.Id)) {
                myClubAction.AddCssClass("remove");
                myClubAction.MergeAttribute("href", URLHelper.BuildOrganizationQuitOrganization(theOrganization));
                myClubAction.InnerHtml += "Quit Organization";
            } else {
                myClubAction.AddCssClass("add");
                myClubAction.MergeAttribute("href", URLHelper.BuildOrganizationRequestToJoin(theOrganization));
                myClubAction.InnerHtml += "Request to Join Organization";
            }


            myActionsDiv.InnerHtml += myClubAction.ToString();

            var myImageSrc = new TagBuilder("img");
            myImageSrc.MergeAttribute("src", PhotoHelper.ClubPhoto(theOrganization));

            var myTitleSpan = new TagBuilder("span");
            myTitleSpan.AddCssClass("title");

            var myNameLinked = new TagBuilder("a");
            myNameLinked.AddCssClass("itemlinked");
            myNameLinked.MergeAttribute("href", URLHelper.BuildClubUrl(theOrganization));
            myNameLinked.InnerHtml += theOrganization.Name;

            myTitleSpan.InnerHtml += myNameLinked.ToString();

            int myMemberCount = theOrganization.ClubMembers.Count;

            myClubDiv.InnerHtml += myActionsDiv.ToString();
            myClubDiv.InnerHtml += myImageSrc.ToString();
            myClubDiv.InnerHtml += myTitleSpan.ToString();
            myClubDiv.InnerHtml += "<br />";
            myClubDiv.InnerHtml += myMemberCount == 1 ? myMemberCount + " Member" : myMemberCount + " Members";

            return myClubDiv.ToString();
        }
    }
}