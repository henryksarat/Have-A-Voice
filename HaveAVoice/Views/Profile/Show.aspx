<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.View.ProfileModel>" %>
<%@ Import Namespace="HaveAVoice.Helpers.UserInformation" %>
<%@ Import Namespace="HaveAVoice.Helpers.Enums" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Profile
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Profile</h2>
    <p>
        <%= Html.ValidationSummary("Board post was unsuccessful. Please correct the errors and try again.") %>
    </p>
    <p>
        <%= Html.Encode(ViewData["Message"]) %>
    </p>
    <p>
        <%= Html.Encode(TempData["Message"]) %>
    </p>

    <b><%= Model.User.Username %> <br /><br /></b><br /><br />

    <img src="<%=Model.ProfilePictureUrl %>" ><br />
    <% if (Model.FanStatus == FanStatus.Approved) { %>
        Already a fan!
    <% } else if (Model.FanStatus == FanStatus.Pending) { %>
        Your fan request is pending
    <% } else { %>
        <%=Html.ActionLink("Become a fan.", "Create", "Fan", new { id = Model.User.Id }, null)%>
    <% } %><br /><br />
    <%= Html.ValidationSummary("Unable to post. Please correct the error and try again.") %><br />
    
    Board<br />

        <% foreach (var item in Model.BoardMessages) { %>
            <% using (Html.BeginForm("PostReplyToBoardMessage", "Profile", new { profileUserId = Model.User.Id ,boardId = item.Id, postingUserId = HAVUserInformationFactory.GetUserInformation().Details.Id })) {%>
                <%= item.Message%><br /> 
                <%= item.User.Username%><%= item.DateTimeStamp%><br />
                <br />
                <%=Html.ActionLink("View", "View", "Board", new { id = item.Id }, null)%>
                <% if(item.PostedUserId == HAVUserInformationFactory.GetUserInformation().Details.Id || item.OwnerUserId == HAVUserInformationFactory.GetUserInformation().Details.Id) { %>
                    <%=Html.ActionLink("Delete", "Delete", "Board", new { profileUserId = Model.User.Id, boardId = item.Id }, null)%>
                <% } %>
                <% if(item.PostedUserId == HAVUserInformationFactory.GetUserInformation().Details.Id) { %>
                    <%=Html.ActionLink("Edit", "Edit", "Board", new { id = item.Id }, null)%>
                <% } %>
                <%= Html.TextArea("BoardReply")%><br /><br />
                <input type="submit" value="Post" /><br /><br />
            <%} %>
        <% } %>


    <%= Html.ActionLink("People who I am fans of.",  "FansOf", "Profile", new {id = Model.User.Id }, null)%><br />
    <%= Html.ActionLink("People who are fans of me.", "Fans", "Profile", new { id = Model.User.Id }, null)%><br /><br />

    Issue Replys<br />
    <% foreach (var item in Model.IssueReplys) { %>
        Issue=<%= item.Issue.Title %><br /> 
        IssueReply=<%= item.Reply%><%= item.DateTimeStamp %><br /><br />
    <% } %>


    <% using (Html.BeginForm("Create", "Board")) {%>
        <%= Html.Hidden("sourceUserId", Model.User.Id) %>
        <p>
            <%= Html.TextArea("Message", new { style = "width:300px; height: 200px" })%>
            <%= Html.ValidationMessage("Message", "*")%>
        </p>  
        <p>
            <input type="submit" value="Post" />
        </p>  
    <%} %>
</asp:Content>
