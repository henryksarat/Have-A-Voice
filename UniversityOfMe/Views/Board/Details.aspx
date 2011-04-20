<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInWrapperModel<Board>>" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	UniversityOf.Me - <%=NameHelper.FullName(Model.User) %>'s Profile
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% Html.RenderPartial("Message"); %>
    <% Html.RenderPartial("Validation"); %>

    Board:<br />
    Name: <%= NameHelper.FullName(Model.Get().PostedByUser) %><br />
    Message: <%= Model.Get().Message %>
    
    <br /><br />

    Board:<br /><br />
    <% foreach (BoardReply myBoardReply in Model.Get().BoardReplies.OrderByDescending(br => br.DateTimeStamp)) { %>
        PostedBy: <%= NameHelper.FullName(myBoardReply.User) %><br />
        Message: <%= myBoardReply.Message %><br /><br />
    <% } %>

    <% using (Html.BeginForm("Create", "BoardReply", FormMethod.Post)) { %>
            <%= Html.Hidden("BoardId", Model.Get().Id) %>
            <%= Html.Hidden("SourceId", Model.Get().Id) %>
            <%= Html.Hidden("SiteSection", SiteSection.Board) %>

            <div class="editor-label">
                <%: Html.Label("Reply") %>
            </div>
            <div class="editor-field">
                <%: Html.TextArea("BoardReply", null, 10, 30, null)%>
                <%: Html.ValidationMessage("BoardReply", "*")%>
            </div>

			<input type="submit" value="Post Reply" />
	<% } %>
</asp:Content>
