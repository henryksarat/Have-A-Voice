<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.View.BoardModel>" %>
<%@Import Namespace="HaveAVoice.Models" %>
<%@Import Namespace="HaveAVoice.Models.View" %>
<%@Import Namespace="HaveAVoice.Helpers" %>
<%@Import Namespace="HaveAVoice.Helpers.UserInformation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	View
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Viewing Board</h2>
    <p>
        <%= Html.ValidationSummary("Your reply wasn't posted. Please correct the errors and try again.") %>
    </p>
    <p>
        <%= Html.Encode(ViewData["Message"]) %>
    </p>
    <p>
        <%= Html.Encode(TempData["Message"]) %>
    </p>

    <% using (Html.BeginForm()) { %>
        <p>
            <%= Model.Board.Message %>
        </p>

        <% foreach (BoardReply reply in Model.BoardReplies) { %>
            <p>
                <%= reply.Message %> 
                <% UserInformationModel myUserInfo = HAVUserInformationFactory.GetUserInformation(); %>
                <% if (reply.User.Id == myUserInfo.Details.Id
                       || HAVPermissionHelper.AllowedToPerformAction(myUserInfo, HAVPermission.Edit_Any_Board_Reply)) { %>
                    <%= Html.ActionLink("Edit", "Edit", "BoardReply", new { id = reply.Id }, null)%>
                <% } %>
                <% if (reply.User.Id == myUserInfo.Details.Id
                       || Model.Board.OwnerUserId == myUserInfo.Details.Id
                       || HAVPermissionHelper.AllowedToPerformAction(myUserInfo, HAVPermission.Edit_Any_Board_Reply)) { %>
                    <%= Html.ActionLink("Delete", "Delete", "BoardReply", new {  boardId = Model.Board.Id, boardReplyId = reply.Id }, null)%>
                <% } %>
            </p>
        <%}%>
    <% } %>

    <p>
    Post a reply
    </p>

    <% using (Html.BeginForm("Create", "BoardReply", new { boardId = Model.Board.Id })) {%>
        <p>
            <%= Html.TextArea("Message", new { style = "width:300px; height: 200px" })%>
            <%= Html.ValidationMessage("Message", "*")%>
        </p>  
        <p>
            <input type="submit" value="Post" />
        </p>  
    <%} %>

</asp:Content>

