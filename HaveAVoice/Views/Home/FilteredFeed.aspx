﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.View.LoggedInModel<FeedModel>>" %>
<%@ Import Namespace="HaveAVoice.Helpers.UserInformation" %>
<%@ Import Namespace="HaveAVoice.Models.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Logged In
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% Html.RenderPartial("UserPanel", Model.UserModel); %>    
    <div class="col-3 m-rgt left-nav">
        <% Html.RenderPartial("LeftNavigation"); %>
    </div>
    
    <div class="col-21">
        <%= Html.ValidationSummary("Create was unsuccessful. Please correct the errors and try again.") %>
        <div class="clear">&nbsp;</div>

		<div class="filter">
			<% using (Html.BeginForm("FilteredFeed", "Home", FormMethod.Post, new { @class = "create btint-6" })) { %>
			<div class="col-2">
				<div class="p-h5 fnt-14 c-white">
					<b>Filter By</b>
				</div>
			</div>
			<div class="col-2 m-rgt right c-white">
				<label for="Zip">Zip:</label>
			</div>
			<div class="col-3">
				<%= Html.TextBox("Zip") %>
			</div>
			<div class="col-2 m-rgt right c-white">
				<label for="City">City:</label>
			</div>
			<div class="col-3">
				<%= Html.TextBox("City") %>
			</div>
			<div class="col-2 m-rgt right c-white">
				<label for="State">State:</label>
			</div>
			<div class="col-4">
				&nbsp;
				<% /* = Html.DropDownList("State", Model.States()) */ %>
			</div>
			<div class="col-3 center">
				<input type="submit" value="Search" class="create" />
			</div>
			<% } %>
			<div class="clear">&nbsp;</div>
		</div>
		<div class="clear">&nbsp;</div>
		<div class="spacer-10">&nbsp;</div>
		
		<% int cnt = 0; %>
        <% foreach (var item in Model.Models) { %>
        
			<div class="<% if(cnt % 2 == 0) { %>row<% } else { %>alt<% } %>">
				<div class="col-2 center">
					<img src="<%= item.ProfilePictureUrl %>" alt="<%= item.Username %>" class="profile" />
				</div>
				<div class="col-16">
					<div class="m-lft col-16 comment">
						<span class="speak-lft">&nbsp;</span>
						<div class="p-a10">
							<% if (item.IssueType == HaveAVoice.Helpers.Enums.IssueType.Issue) { %>
								<h1><a href="#"><%= item.Title %></a></h1>
							<% } else { %>
								<a class="name" href="#"><%= item.Username %></a>
							<% } %>
							<br />
							<%= item.Body %>
							<div class="clear">&nbsp;</div>
							
							<div class="spacer-10">&nbsp;</div>
							<div class="options">
								<div class="col-6">&nbsp;</div>
								<div class="col-9">
									<div class="col-3 center">
										<% if (item.TotalReplys == 0) { %>
											<a href="#" class="comment">Comment</a>
										<% } else { %>
											<span class="comment"><%= item.TotalReplys %> 
												Comment<% if (item.TotalReplys > 1) { %>s<% } %>
											</span>
										<% } %>
									</div>
									<div class="col-3 center">
										<% if (item.HasDisposition) { %>
											<span class="like">
												<%= item.TotalLikes %>
												<% if (item.TotalLikes == 1) { %>
													Person Likes
												<% } else { %>
													People Like
												<% } %>
												This
											</span>
										<% } else { %>
											<a href="#" class="like">Like</a>
										<% } %>
									</div>
									<div class="col-3 center">
										<% if (item.HasDisposition) { %>
											<span class="dislike">
												<%= item.TotalDislikes %>
												<% if (item.TotalDislikes == 1) { %>
													Person Dislikes
												<% } else { %>
													People Dislike 
												<% } %>
												This
											</span>
										<% } else { %>
											<a href="#" class="dislike">Dislike</a>
										<% } %>
									</div>
								</div>
							</div>
						</div>
					</div>
				</div>
				<div class="col-3">
					<div class="p-a5">
						<div class="date-tile">
							<span><%= item.DateTimeStamp.ToString("MMM").ToUpper() %></span> <%= item.DateTimeStamp.ToString("dd") %>
						</div>
					</div>
				</div>
				<div class="clear">&nbsp;</div>
			</div>
			<div class="spacer-10">&nbsp;</div>
        <% } %>
    </div>
</asp:Content>