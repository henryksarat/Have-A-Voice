<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Social.BaseWebsite.Models.ILoggedInModel<UniversityOfMe.Models.User>>" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Inbox
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Inbox</h2>
    <% Html.RenderPartial("Message"); %>
    <% Html.RenderPartial("Validation"); %>

    <% using (Html.BeginForm()) {%>
        <%= Html.Hidden("ToUserId", Model.Get().Id) %>


        Sending a message to: <br />
        Name: <%= NameHelper.FullName(Model.Get()) %><br />
        Profile Picture: <%= PhotoHelper.ProfilePicture(Model.Get()) %><br />
        
        <div class="editor-label">
            <%: Html.Label("Subject") %>
        </div>
        <div class="editor-field">
            <%: Html.TextBox("Subject")%>
            <%: Html.ValidationMessage("Subject", "*")%>
        </div>

        <div class="editor-label">
            <%: Html.Label("Body") %>
        </div>
        <div class="editor-field">
            <%: Html.TextArea("Body", null, 10, 30, null)%>
            <%: Html.ValidationMessage("Body", "*")%>
        </div>

        <p>
            <input type="submit" value="Send" />
        </p>
    <% } %>
    </table>
</asp:Content>

