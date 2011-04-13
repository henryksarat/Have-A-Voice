<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<UniversityOfMe.Models.Event>" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.UserInformation" %>
<%@ Import Namespace="Social.Generic.Models" %>
<%@ Import Namespace="Social.Admin.Helpers" %>
<%@ Import Namespace="Social.Generic.Helpers" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Details
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Details</h2>

    <% Html.RenderPartial("Message"); %>
    <% Html.RenderPartial("Validation"); %>


    <fieldset>
        <legend>Fields</legend>
        
        <div class="display-label">Id</div>
        <div class="display-field"><%: Model.Id %></div>
        
        <div class="display-label">UniversityId</div>
        <div class="display-field"><%: Model.UniversityId %></div>
        
        <div class="display-label">UserId</div>
        <div class="display-field"><%: Model.UserId %></div>
        
        <div class="display-label">DateStart</div>
        <div class="display-field"><%: String.Format("{0:g}", Model.StartDate) %></div>
        
        <div class="display-label">DateEnd</div>
        <div class="display-field"><%: String.Format("{0:g}", Model.EndDate) %></div>
        
        <div class="display-label">Information</div>
        <div class="display-field"><%: Model.Information %></div>
        
        <div class="display-label">EntireSchool</div>
        <div class="display-field"><%: Model.EntireSchool %></div>
        
        <div class="display-label">Deleted</div>
        <div class="display-field"><%: Model.Deleted %></div>
        
        <div class="display-label">DeletedUserId</div>
        <div class="display-field"><%: Model.DeletedUserId %></div>
        <% UserInformationModel<User> myUserInfo = UserInformationFactory.GetUserInformation(); %>
        <% bool myAllowedToEdit = Model.UserId == myUserInfo.Details.Id || PermissionHelper<User>.AllowedToPerformAction(myUserInfo, SocialPermission.Edit_Any_Event); %>
        <% bool myAllowedToDelete = Model.UserId == myUserInfo.Details.Id || PermissionHelper<User>.AllowedToPerformAction(myUserInfo, SocialPermission.Delete_Any_Event); %>
        
        <% if (myAllowedToDelete) { %>
            <% using (Html.BeginForm("Delete", "Event", new { id = Model.Id })) {%>
                <input type="submit" value="Delete" />
            <% } %>
        <% } %>

        <% if (myAllowedToEdit) { %>
            <%= Html.ActionLink("Edit", "Edit", "Event", new { id = Model.Id }, null)%>
        <% } %>

        <% using (Html.BeginForm("Create", "EventBoard")) {%>
            <%= Html.Hidden("EventId", Model.Id) %>
            <%= Html.ValidationMessage("BoardMessage", "*")%>
            <%= Html.TextArea("BoardMessage")%>

            <p>
                <input type="submit" value="Create" />
            </p>
        <% } %>

        Board:

        <% foreach (EventBoard myEventBoard in Model.EventBoards.OrderByDescending(e => e.DateTimeStamp)) { %>
            Posted By: <%= NameHelper.FullName(myEventBoard.User) %><br />
            Date Time Stamp: <%= myEventBoard.DateTimeStamp %><br />      
            Message: <%= myEventBoard.Message %><br /><br />
        <% } %>

    </fieldset>
</asp:Content>

