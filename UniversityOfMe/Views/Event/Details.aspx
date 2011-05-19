<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInWrapperModel<Event>>" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>
<%@ Import Namespace="UniversityOfMe.UserInformation" %>
<%@ Import Namespace="Social.Generic.Models" %>
<%@ Import Namespace="Social.Admin.Helpers" %>
<%@ Import Namespace="Social.Generic.Helpers" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Details
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% Html.RenderPartial("LeftNavigation", Model.LeftNavigation); %>

    <% Html.RenderPartial("Message"); %>
    <% Html.RenderPartial("Validation"); %>


    <fieldset>
        <legend>Fields</legend>
        
        <div class="display-label">Id</div>
        <div class="display-field"><%: Model.Get().Id %></div>
        
        <div class="display-label">UniversityId</div>
        <div class="display-field"><%: Model.Get().UniversityId%></div>
        
        <div class="display-label">UserId</div>
        <div class="display-field"><%: Model.Get().UserId%></div>
        
        <div class="display-label">DateStart</div>
        <div class="display-field"><%: String.Format("{0:g}", Model.Get().StartDate)%></div>
        
        <div class="display-label">DateEnd</div>
        <div class="display-field"><%: String.Format("{0:g}", Model.Get().EndDate)%></div>
        
        <div class="display-label">Information</div>
        <div class="display-field"><%: Model.Get().Information%></div>
        
        <div class="display-label">EntireSchool</div>
        <div class="display-field"><%: Model.Get().EntireSchool%></div>
        
        <div class="display-label">Deleted</div>
        <div class="display-field"><%: Model.Get().Deleted%></div>
        
        <div class="display-label">DeletedUserId</div>
        <div class="display-field"><%: Model.Get().DeletedUserId%></div>
        <% UserInformationModel<User> myUserInfo = UserInformationFactory.GetUserInformation(); %>
        <% bool myAllowedToEdit = Model.Get().UserId == myUserInfo.Details.Id || PermissionHelper<User>.AllowedToPerformAction(myUserInfo, SocialPermission.Edit_Any_Event); %>
        <% bool myAllowedToDelete = Model.Get().UserId == myUserInfo.Details.Id || PermissionHelper<User>.AllowedToPerformAction(myUserInfo, SocialPermission.Delete_Any_Event); %>
        
        <% if (myAllowedToDelete) { %>
            <% using (Html.BeginForm("Delete", "Event", new { id = Model.Get().Id })) {%>
                <input type="submit" value="Delete" />
            <% } %>
        <% } %>

        <% if (myAllowedToEdit) { %>
            <%= Html.ActionLink("Edit", "Edit", "Event", new { id = Model.Get().Id }, null)%>
        <% } %>

        <% using (Html.BeginForm("Create", "EventBoard")) {%>
            <%= Html.Hidden("EventId", Model.Get().Id)%>
            <%= Html.ValidationMessage("BoardMessage", "*")%>
            <%= Html.TextArea("BoardMessage")%>

            <p>
                <input type="submit" value="Create" />
            </p>
        <% } %>

        Board:

        <% foreach (EventBoard myEventBoard in Model.Get().EventBoards.OrderByDescending(e => e.DateTimeStamp)) { %>
            Posted By: <%= NameHelper.FullName(myEventBoard.User) %><br />
            Date Time Stamp: <%= myEventBoard.DateTimeStamp %><br />      
            Message: <%= myEventBoard.Message %><br /><br />
        <% } %>

    </fieldset>
</asp:Content>

