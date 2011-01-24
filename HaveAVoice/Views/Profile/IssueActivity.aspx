﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInWrapperModel<UserProfileModel>>" %>
<%@ Import Namespace="HaveAVoice.Helpers.Enums" %>
<%@ Import Namespace="HaveAVoice.Models.View" %>
<%@ Import Namespace="HaveAVoice.Services.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	IssueActivity
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% Html.RenderPartial("UserPanel", Model.NavigationModel); %>
    <div class="col-3 m-rgt left-nav">
        <% Html.RenderPartial("LeftNavigation"); %>
    </div>

    <div class="col-21">
    <% Html.RenderPartial("Message"); %>


    <% FeedItem myNextFeedItem = Model.Model.GetNextItem(); %>
    <% int cnt = 0; %>
    <% while (myNextFeedItem != FeedItem.None) { %>
            <% if(myNextFeedItem == FeedItem.Issue) { %>
                <% IssueFeedModel myIssue = Model.Model.GetNextIssue(); %>
                <div class="<% if(cnt % 2 == 0) { %>row<% } else { %>alt<% } %> m-btm10">
				    <div class="col-2 center">
					    <img src="<%= myIssue.ProfilePictureUrl %>" alt="<%= myIssue.Username %>" class="profile" />
				    </div>
				    <div class="col-16">
					    <div class="m-lft col-16 comment">
						    <span class="speak-lft">&nbsp;</span>
						    <div class="p-a10">
							    <h1><a href="/Issue/View/<%= myIssue.Id %>"><%= myIssue.Title %></a></h1>
							    <br />
							    <%= myIssue.Description %>

							    <div class="clear">&nbsp;</div>

							    <div class="col-15">
								    <div class="push-6 col-9 p-v10">
									    <div class="col-3 center">
										    <% if (myIssue.TotalReplys == 0) { %>
										    	&nbsp;
										    	<!--
											    <a href="#" class="comment">
													    Reply
											    </a>
											    //-->
										    <% } else { %>
											    <span class="comment"><%= myIssue.TotalReplys %> 
													    Repl<% if (myIssue.TotalReplys > 1) { %>ies<% } else { %>y<% } %>
											    </span>
										    <% } %>
									    </div>
								    </div>
								    <div class="clear">&nbsp;</div>
							    </div>
						    </div>
					    </div>
				    </div>
				    <div class="col-3">
					    <div class="p-a5">
						    <div class="date-tile">
							    <span><%= myIssue.DateTimeStamp.ToString("MMM").ToUpper() %></span> <%= myIssue.DateTimeStamp.ToString("dd") %>
						    </div>
					    </div>
				    </div>
				    <div class="clear">&nbsp;</div>
			    </div>
			    <div class="clear">&nbsp;</div>
			    
			    <% int j = 0; %>
                <% foreach (var item in myIssue.IssueReplys) { %>
                	<div class="<% if (j % 2 == 0) { %>row<% } else { %>alt<% } %> reply push-2 col-19 m-btm10">
	                	<div class="col-2 center">
	                		<img src="<%= PhotoHelper.ProfilePicture(item.User) %>" alt="<%= item.User.Username %>" class="profile" />
	                		&nbsp;
	                	</div>
	                	<div class="m-lft col-14 comment">
	                		<span class="speak-lft">&nbsp;</span>
	                		<div class="p-a10">
	                			<a href="/Profile/Show/<%= item.User.Id %>" class="name"><%= item.User.Username %></a>
	                			<%= item.Reply %>
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
					<div class="clear">&nbsp;</div>
                    <% j++; %>
                <% } %>

	    		<div class="board-reply m-btm10">
					<div class="push-2 col-19">						
						<div class="col-2 center">
							<img src="<%= Model.NavigationModel.ProfilePictureUrl %>" alt="<%= Model.NavigationModel.User.Username %>" class="profile" />
						</div>
						<div class="m-lft col-14">
						    <% using (Html.BeginForm("Create", "IssueReply", new { issueId = myIssue.Id, disposition = 1, anonymous = false })) { %>
					            <%= Html.ValidationMessage("Reply", "*")%>
					            <%= Html.TextArea("Reply")%>
					            <div class="clear">&nbsp;</div>
					            <div class="right m-top10">
					            	<input type="submit" value="Post" />
					            </div>
						    <% } %>
						</div>
						<div class="clear">&nbsp;</div>
					</div>
					<div class="clear">&nbsp;</div>
				</div>
            <% } else if (myNextFeedItem == FeedItem.IssueReply) {%>
                <% IssueReplyFeedModel myIssueReply = Model.Model.GetNextIssueReply(); %>
			    <div class="<% if(cnt % 2 == 0) { %>row<% } else { %>alt<% } %> m-btm10">
				    <div class="col-2 center">
					    <img src="<%= myIssueReply.ProfilePictureUrl %>" alt="<%= myIssueReply.Username %>" class="profile" />
				    </div>
				    <div class="col-16">
					    <div class="m-lft col-16 comment">
						    <span class="speak-lft">&nbsp;</span>
						    <div class="p-a10">
						    	<%= Html.ActionLink(myIssueReply.Username, "Show", "Profile", myIssueReply.Username, new { @class = "name" }) %>
							    <br />
							    <%= myIssueReply.Reply %>

							    <div class="clear">&nbsp;</div>							
							    <div class="spacer-10">&nbsp;</div>
							    <div class="clear">&nbsp;</div>

							    <div class="options">
								    <div class="col-6">&nbsp;</div>
								    <div class="col-9">
									    <div class="col-3 center">
										    <% if (myIssueReply.TotalComments == 0) { %>
										    	&nbsp;
										    	<!--
											    <a href="#" class="comment">
													Comment
											    </a>
											    //-->
										    <% } else { %>
											    <span class="comment"><%= myIssueReply.TotalComments %> 
													Comment<% if (myIssueReply.TotalComments > 1) { %>s<% } %>
											    </span>
										    <% } %>
									    </div>
									    <div class="col-3 center">
										    <% if (myIssueReply.HasDisposition) { %>
											    <span class="like">
												    <%= myIssueReply.TotalLikes %>
												    <% if (myIssueReply.TotalLikes == 1) { %>
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
								    </div>
							    </div>
						    </div>
					    </div>
				    </div>
				    <div class="col-3">
					    <div class="p-a5">
						    <div class="date-tile">
							    <span><%= myIssueReply.DateTimeStamp.ToString("MMM").ToUpper()%></span> <%= myIssueReply.DateTimeStamp.ToString("dd") %>
						    </div>
					    </div>
				    </div>
				    <div class="clear">&nbsp;</div>
			    </div>

				<!-- CORRECTING COMMENT PLACEMENT -->
			    <% int j = 0; %>
                <% foreach (var item in myIssueReply.IssueReplyComments) { %>
                	<div class="<% if (j % 2 == 0) { %>row<% } else { %>alt<% } %> reply push-2 col-19 m-btm10">
	                	<div class="col-2 center">
	                		<img src="<%= PhotoHelper.ProfilePicture(item.User) %>" alt="<%= item.User.Username %>" class="profile" />
	                		&nbsp;
	                	</div>
	                	<div class="m-lft col-14 comment">
	                		<span class="speak-lft">&nbsp;</span>
	                		<div class="p-a10">
	                			<a href="#" class="name"><%= item.User.Username %></a>
	                			<%= item.Comment %>
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
					<div class="clear">&nbsp;</div>
                    <% j++; %>
                <% } %>

	    		<div class="board-reply m-btm10">
					<div class="push-2 col-19">
						<div class="col-2 center">
							<img src="<%= Model.NavigationModel.ProfilePictureUrl %>" alt="<%= Model.NavigationModel.User.Username %>" class="profile" />
						</div>
						<div class="m-lft col-14 m-rgt">
						    <% using (Html.BeginForm("Create", "IssueReplyComment", new { issueReplyId = myIssueReply.Id })) { %>
					            <%= Html.ValidationMessage("Comment", "*") %>
					            <%= Html.TextArea("Comment") %>
					            <div class="clear">&nbsp;</div>
					            <div class="right m-top10">
					            	<input type="submit" value="Post" />
					            </div>
						    <% } %>
						</div>
						<div class="clear">&nbsp;</div>
					</div>
					<div class="clear">&nbsp;</div>
				</div>
            <% } %>
            <% cnt++; %>
            <%  myNextFeedItem = Model.Model.GetNextItem(); %>
    <% } %>






</asp:Content>
