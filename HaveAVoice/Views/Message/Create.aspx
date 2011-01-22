<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.MessageWrapper>" %>
<%@ Import Namespace="HaveAVoice.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Send a Message
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2><%= Html.Encode("Send a message to: " + Model.ToUserName) %></h2>
    <img src="<%=Model.ToUserProfilePictureUrl %>" width="100px" height="100px" />
    <%= Html.ValidationSummary("Send was unsuccessful. Please correct the errors and try again.") %>

    <% using (Html.BeginForm()) { %>
        <%= Html.Hidden("ToUserId", Model.ToUserId)%>
        <%= Html.Hidden("ToUserName", Model.ToUserName)%>
        <%= Html.Hidden("ToUserProfilePictureUrl", Model.ToUserProfilePictureUrl)%>
        <p>
             <%= Html.Encode(ViewData["Message"]) %>
        </p>
            <p>
                <%= Html.TextBox("Subject", "", new { style = "width:300px;" })%>
                <%= Html.ValidationMessage("Subject", "*") %>
            </p>
                <%= Html.TextArea("Body", new { style = "width:300px; height: 200px" })%>
                <%= Html.ValidationMessage("Body", "*") %>
            </p>
            <p>
                <input type="submit" value="Send" />
            </p>
    <% } %>

    <div>
        <%=Html.ActionLink("Back to List", "Index") %>
    </div>

</asp:Content>

