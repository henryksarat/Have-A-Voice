<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.View.LoggedInWrapperModel<BoardModel>>" %>
<%@Import Namespace="HaveAVoice.Models" %>
<%@Import Namespace="HaveAVoice.Models.View" %>
<%@Import Namespace="HaveAVoice.Helpers" %>
<%@Import Namespace="HaveAVoice.Helpers.UserInformation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	View
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<% Html.RenderPartial("UserPanel", Model.NavigationModel); %>
    <div class="col-3 m-rgt left-nav">
        <% Html.RenderPartial("LeftNavigation"); %>
    </div>

	<div class="col-21">
		<%= Html.Encode(ViewData["Message"]) %>
		<%= Html.Encode(TempData["Message"]) %>
		<%= Html.ValidationSummary("Your reply wasn't posted. Please correct the errors and try again.") %>
		<div class="clear">&nbsp;</div>
		
		<!-- BOARD ACTIVITY [Images] //-->
		<div class="board-image m-btm10">
			<div class="col-6 user-info">
				<div class="p-a5">
					<div class="col-2 center m-rgt10">
						<img src="http://upload.wikimedia.org/wikipedia/commons/4/41/Jesse_Jane_2010.jpg" alt="Cindy Taylor" class="profile" />
					</div>
					<a class="name" href="#">Cindy Taylor</a>
					added new photos
					<div class="clear">&nbsp;</div>
					<h1>Red Carpet Awards</h1>
					23 photos, 4 new, 3 friends tagged
				</div>
				<div class="col-6 link">
					<div class="col-2 center">
						<a href="#" class="comment">Comment</a>
					</div>
					<div class="col-2 center">
						<a href="#" class="like">Like</a>
					</div>
					<div class="col-2 center">
						<a href="#" class="dislike">Dislike</a>
					</div>
				</div>
				
				<div class="clear">&nbsp;</div>
			</div>
			<div class="col-12">
				<div class="col-5 photo" style="background: url('http://twittchicks.com/wp-content/uploads/2009/03/jesse-jane.jpg') top center no-repeat;">
					PHOTO 1
				</div>
				<div class="col-3 photo" style="background: url('http://upload.wikimedia.org/wikipedia/commons/a/a4/Jesse_Jane_DSC_0112.JPG') top center no-repeat;">
					PHOTO 2
				</div>
				<div class="col-4 photo" style="background: url('http://images.starpulse.com/pictures/2006/10/04/previews/Jesse%20Jane-SGG-022374.jpg') top center no-repeat;">
					PHOTO 3
				</div>
			</div>
			<div class="col-3 date-tile">
				<div class="p-a5">
					<span>9:12</span> pm
				</div>
			</div>
			<div class="clear">&nbsp;</div>
		</div>
		<div class="clear">&nbsp;</div>
		

		<!-- BOARD ACTIVITY [Reply] //-->
		<div class="board-reply">
			<div class="col-19">
				<div class="p-a5">
					<div class="col-2">
						<img src="/UserPictures/no_profile_picture.jpg" alt="SELF" class="profile" />
					</div>
					<div class="m-lft col-14 m-rgt">
					    <% using (Html.BeginForm("Create", "BoardReply", new { boardId = Model.Model.Board.Id })) { %>
				            <%= Html.ValidationMessage("Message", "*") %>
				            <%= Html.TextArea("Message") %>
				            <div class="clear">&nbsp;</div>
				            <div class="right m-top10">
				            	<input type="submit" value="Post" />
				            </div>
					    <% } %>
					</div>
					<div class="alpha col-3 omega">
						<div class="p-v5">
							<div class="date-tile">
								<span>8:23</span> PM
							</div>
						</div>
					</div>
					<div class="clear">&nbsp;</div>
				</div>
				<div class="clear">&nbsp;</div>
			</div>
			<div class="clear">&nbsp;</div>
		</div>
		
    <% using (Html.BeginForm()) { %>
        <p>
            <%= Model.Model.Board.Message %>
        </p>

        <% foreach (BoardReply reply in Model.Model.BoardReplies) { %>
		<!-- BOARD ACTIVITY [Message] //-->
			<div class="row">
				<div class="col-2 center">
					<img src="http://images.pictureshunt.com/pics/e/eva_angelina-4773.jpg" alt="Eva Angelina" class="profile" />
				</div>
				<div class="col-16 m-btm10">
					<div class="m-lft col-16 comment">
						<span class="speak-lft">&nbsp;</span>
						<div class="p-a10">
							<a class="name" href="#">Eva Angelina</a>
							<%= reply.Message %>
							<div class="clear">&nbsp;</div>
	
							<div class="spacer-10">&nbsp;</div>
								
							<div class="clear">&nbsp;</div>
							<div class="options">
								<div class="col-6 center">
					                <% UserInformationModel myUserInfo = HAVUserInformationFactory.GetUserInformation(); %>
					                <% if (reply.User.Id == myUserInfo.Details.Id
					                       || HAVPermissionHelper.AllowedToPerformAction(myUserInfo, HAVPermission.Edit_Any_Board_Reply)) { %>
					                    <%= Html.ActionLink("Edit", "Edit", "BoardReply", new { id = reply.Id }, null)%>
					                <% } %>
					                <% if (reply.User.Id == myUserInfo.Details.Id
					                       || Model.Model.Board.OwnerUserId == myUserInfo.Details.Id
					                       || HAVPermissionHelper.AllowedToPerformAction(myUserInfo, HAVPermission.Edit_Any_Board_Reply)) { %>
					                    <%= Html.ActionLink("Delete", "Delete", "BoardReply", new { boardId = Model.Model.Board.Id, boardReplyId = reply.Id }, null)%>
					                <% } %>
									&nbsp;
								</div>
								<div class="col-9">
									<div class="col-3 center">
										<a href="#" class="comment">COMMENT</a>
									</div>
									<div class="col-3 center">
										<a href="#" class="like">LIKE</a>
									</div>
									<div class="col-3 center">
										<a href="#" class="dislike">DISLIKE</a>
									</div>
								</div>
							</div>
							<div class="clear">&nbsp;</div>
							<div class="spacer-10">&nbsp;</div>
							<div class="clear">&nbsp;</div>
						</div>
					</div>
				</div>
				<div class="col-3">
					<div class="p-a5">
						<div class="date-tile">
							<span>3:47</span> AM
						</div>
					</div>
				</div>
				<div class="clear">&nbsp;</div>
			</div>
	    <% } %>
	</div>
</asp:Content>