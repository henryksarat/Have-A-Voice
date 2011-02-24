<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.View.LoggedInWrapperModel<Board>>" %>
<%@Import Namespace="HaveAVoice.Models" %>
<%@Import Namespace="HaveAVoice.Models.View" %>
<%@Import Namespace="HaveAVoice.Helpers" %>
<%@Import Namespace="HaveAVoice.Helpers.UserInformation" %>
<%@Import Namespace="HaveAVoice.Services.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	View
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<% Html.RenderPartial("UserPanel", Model.NavigationModel); %>
    <div class="col-3 m-rgt left-nav">
        <% Html.RenderPartial("LeftNavigation", Model.NavigationModel); %>
    </div>

	<div class="col-21">
        <% Html.RenderPartial("Message"); %>
        <% Html.RenderPartial("Validation"); %>
		<div class="clear">&nbsp;</div>

        Board message: <br />
        <%= Model.Model.Message %>
        <%= PhotoHelper.ProfilePicture(Model.Model.PostedByUser) %><br />
        By: <%= NameHelper.FullName(Model.Model.PostedByUser) %><br />
        
        Replies: <br />
        <% foreach (BoardReply myReply in Model.Model.BoardReplies) { %>
            Reply text: <%= myReply.Message %> <br />
            Reply author: <%= NameHelper.FullName(myReply.User) %> <br />
            Profile pic: <%= PhotoHelper.ProfilePicture(myReply.User) %> <br /><br />
        <% } %>    	
        
		<% using (Html.BeginForm("Create", "BoardReply", new { ownerUserId = Model.Model.OwnerUserId, boardId = Model.Model.Id })) { %>
			    <%= Html.ValidationMessage("BoardReply", "*")%>
			    <%= Html.TextArea("BoardReply")%>
		        <input type="submit" value="Post" />
		        <div class="clear">&nbsp;</div>
		<% } %>	
	
	</div>
</asp:Content>