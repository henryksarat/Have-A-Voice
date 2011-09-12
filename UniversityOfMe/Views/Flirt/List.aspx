<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInListModel<AnonymousFlirt>>" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<% Social.Generic.Models.UserInformationModel<User> myUserInfo = UniversityOfMe.UserInformation.UserInformationFactory.GetUserInformation(); %>
    Anonymous Flirts Sent Within the <%= UniversityHelper.GetMainUniversity(myUserInfo.Details).UniversityName %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% Social.Generic.Models.UserInformationModel<User> myUserInfo = UniversityOfMe.UserInformation.UserInformationFactory.GetUserInformation(); %>
    <% Html.RenderPartial("LeftNavigation", Model.LeftNavigation); %>

    <div class="eight last"> 
        <% Html.RenderPartial("Message"); %>
        <div class="create-feature-form">
		    <div class="banner black full"> 
			    Anonymous Flirts Sent Within the <%= UniversityHelper.GetMainUniversity(myUserInfo.Details).UniversityName %>
		    </div> 
            <div class="padding-col">
                <ul> 
                    <% foreach (AnonymousFlirt myFlirt in Model.Get()) { %>
                            <div class="mb15">
			                     <span class="red"><%= myFlirt.Adjetive %></span>
                                 <span class="blu"><%= myFlirt.SomethingDelicious %></span>
                                 <span class="green"><%= myFlirt.Animal %></span>, that
                                 <span class="black">
                                    <%= myFlirt.HairColor.Equals("Dunno") ? string.Empty : myFlirt.HairColor %> Haired
                                    <%= myFlirt.Gender %> 
                                    <%= string.IsNullOrEmpty(myFlirt.Location) ? string.Empty : " i saw at " + myFlirt.Location %> 
                                    <span class="gray tiny"><%= LocalDateHelper.ToLocalTime(myFlirt.DateTimeStamp) %></span>
                                    <br />
                                    <%= myFlirt.Post %>
                                </span>
                            </div>
                    <% } %>
                </ul> 
            </div>
        </div>
	</div>
</asp:Content>
