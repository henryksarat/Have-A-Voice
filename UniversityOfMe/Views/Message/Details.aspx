<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInWrapperModel<Message>>" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	UniversityOf.Me - Message
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <% Html.RenderPartial("LeftNavigation", Model.LeftNavigation); %>

    <% Html.RenderPartial("Message"); %>
    <% Html.RenderPartial("Validation"); %>

    <a href="<%= URLHelper.ProfileUrl(Model.Get().FromUser)  %>"><img src="<%= PhotoHelper.ProfilePicture(Model.Get().FromUser) %>" /></a>
    <%= NameHelper.FullName(Model.Get().FromUser)  %><br />
    Subject: <%= Model.Get().Subject %><br />
    Body: <%= Model.Get().Body %><br /><br />

    <% foreach (MessageReply myReply in Model.Get().MessageReplies) { %>    
        <a href="<%= URLHelper.ProfileUrl(myReply.User)  %>"><img src="<%= PhotoHelper.ProfilePicture(myReply.User) %>" /></a>
        <%= NameHelper.FullName(myReply.User)  %><br />
        <%= myReply.DateTimeStamp %><br />
        <%= myReply.Body %><br />
    <% } %>

    <% using (Html.BeginForm("CreateReply", "Message")) {%>
        <%= Html.Hidden("MessageId", Model.Get().Id) %>

        <div class="editor-label">
            <%= Html.Label("Reply")%>
        </div>
        <div class="editor-field">
            <%= Html.TextArea("Reply", null, 10, 30, null)%>
            <%= Html.ValidationMessage("Reply", "*")%>
        </div>

        <p>
            <input type="submit" value="Send" />
        </p>
    <% } %>
</asp:Content>
