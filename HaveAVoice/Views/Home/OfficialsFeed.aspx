<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.View.LoggedInModel<FeedModel>>" %>
<%@ Import Namespace="HaveAVoice.Helpers.UserInformation" %>
<%@ Import Namespace="HaveAVoice.Models.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Logged In
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% Html.RenderPartial("UserPanel", Model.UserModel); %>    
    <div class="col-3 left-nav">
        <% Html.RenderPartial("LeftNavigation"); %>
    </div>
    
    <div class="col-21">
        <%= Html.ValidationSummary("Create was unsuccessful. Please correct the errors and try again.") %>

        <h2>Welcome, <%= Model.UserModel.User.Username %></h2>
    
        <br /><br />
        FanFeed:<br />
        <% foreach (var item in Model.Models) { %>
            Username: <%= item.Username %>
            IssueReply: <%= item.Body %><br /><br />
        <% } %>
    </div>
</asp:Content>