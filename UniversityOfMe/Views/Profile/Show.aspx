<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInWrapperModel<User>>" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>
<%@ Import Namespace="UniversityOfMe.UserInformation" %>
<%@ Import Namespace="Social.Generic.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	UniversityOf.Me - <%=NameHelper.FullName(Model.Get()) %>'s Profile
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% Html.RenderPartial("Message"); %>
    <% Html.RenderPartial("Validation"); %>

    
    <% UserInformationModel<User> myLoggedInUser = UserInformationFactory.GetUserInformation(); %>
    Information:<br />
    ProfilePicture: <img src="<%= PhotoHelper.ProfilePicture(Model.Get()) %>" /><br />
    Full Name: <br /> <%=NameHelper.FullName(Model.Get()) %><br />
    University Affiliations: <br />
    <% foreach (string myUniversity in UniversityHelper.GetUniversityAffiliations(Model.Get())) { %>
        <%= myUniversity %> <br />
    <% } %><br /><br />
    <% if (!FriendHelper.IsFriend(Model.Get(), myLoggedInUser.Details)) { %>
        <%= Html.ActionLink("Add as friend", "Add", "Friend", new { id = Model.Get().Id }, null)%>
    <% } %><br />
    <%= Html.ActionLink("Message user", "Create", "Message", new { id = Model.Get().Id }, null)%>

    <br /><br />

    Post to Board:

    <% using (Html.BeginForm("Create", "Board", FormMethod.Post)) { %>
            <%= Html.Hidden("SourceUserId", Model.Get().Id) %>

            <div class="editor-label">
                <%: Html.Label("Board") %>
            </div>
            <div class="editor-field">
                <%: Html.TextArea("BoardMessage", null, 10, 30, null)%>
                <%: Html.ValidationMessage("BoardMessage", "*")%>
            </div>

			<input type="submit" value="Post Board" />
	<% } %>

    Board:<br /><br />
    <% foreach (Board myBoard in Model.Get().Boards.OrderByDescending(b => b.DateTimeStamp)) { %>
        <b>Board Message:</b><br />
        PostedBy: <%= NameHelper.FullName(myBoard.PostedByUser) %><br />
        Message: <a href="/Board/Details/<%= myBoard.Id %>"><%= myBoard.Message %></a><br /><br />
    
        <% foreach (BoardReply myBoardReply in myBoard.BoardReplies) { %>
            Board Reply:<br />
            Posted By: <%= NameHelper.FullName(myBoardReply.User) %><br />
            Reply: <%= myBoardReply.Message %><br /><br />
        <% } %>

        <% using (Html.BeginForm("Create", "BoardReply", FormMethod.Post)) { %>
                <%= Html.Hidden("BoardId", myBoard.Id)%>
                <%= Html.Hidden("SourceId", Model.Get().Id) %>
                <%= Html.Hidden("SiteSection", SiteSection.Profile) %>

                <div class="editor-label">
                    <%: Html.Label("Reply") %>
                </div>
                <div class="editor-field">
                    <%: Html.TextArea("BoardReply", null, 10, 30, null)%>
                    <%: Html.ValidationMessage("BoardReply", "*")%>
                </div>

			    <input type="submit" value="Post Reply" />
	    <% } %>
    <% } %>
</asp:Content>
