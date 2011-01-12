<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInWrapperModel<UserProfileModel>>" %>
<%@ Import Namespace="HaveAVoice.Helpers.UserInformation" %>
<%@ Import Namespace="HaveAVoice.Helpers.Enums" %>
<%@ Import Namespace="HaveAVoice.Models.View" %>
<%@ Import Namespace="HaveAVoice.Services.Helpers" %>
<%@ Import Namespace="HaveAVoice.Models" %>


<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Profile
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% Html.RenderPartial("UserPanel", Model.NavigationModel); %>    
    <div class="col-3 m-rgt left-nav">
        <% Html.RenderPartial("LeftNavigation"); %>
    </div>

    <% if (Model.NavigationModel.SiteSection == SiteSection.MyProfile) { %>

    <div class="col-21">
        <%= Html.ValidationSummary("Create was unsuccessful. Please correct the errors and try again.") %>

        <% FeedItem myNextFeedItem = Model.Model.GetNextItem(); %>
        <% int cnt = 0; %>

        <% while(myNextFeedItem != FeedItem.None) { %>
            <%  myNextFeedItem = Model.Model.GetNextItem(); %>
            <% if(myNextFeedItem == FeedItem.Issue) { %>
                <% IssueFeedModel myIssue = Model.Model.GetNextIssue(); %>
                <div class="<% if(cnt % 2 == 0) { %>row<% } else { %>alt<% } %>">
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
							    <div class="spacer-10">&nbsp;</div>
							    <div class="clear">&nbsp;</div>
                                <% foreach (var item in myIssue.IssueReplys) { %>
                                    <%= item.Reply %>
                                <% } %>
							    <div class="options">
								    <div class="col-6">&nbsp;</div>
								    <div class="col-9">
									    <div class="col-3 center">
										    <% if (myIssue.TotalReplys == 0) { %>
											    <a href="#" class="comment">
													    Reply
											    </a>
										    <% } else { %>
											    <span class="comment"><%= myIssue.TotalReplys %> 
													    Repl<% if (myIssue.TotalReplys > 1) { %>ies<% } else { %>y<% } %>
											    </span>
										    <% } %>
									    </div>
									    <div class="col-3 center">
										    <% if (myIssue.HasDisposition) { %>
											    <span class="like">
												    <%= myIssue.TotalLikes %>
												    <% if (myIssue.TotalLikes == 1) { %>
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
										    <% if (myIssue.HasDisposition) { %>
											    <span class="dislike">
												    <%= myIssue.TotalDislikes %>
												    <% if (myIssue.TotalDislikes == 1) { %>
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
							    <span><%= myIssue.DateTimeStamp.ToString("MMM").ToUpper() %></span> <%= myIssue.DateTimeStamp.ToString("dd") %>
						    </div>
					    </div>
				    </div>
				    <div class="clear">&nbsp;</div>
			    </div>
			    <div class="clear">&nbsp;</div>
			    <div class="spacer-10">&nbsp;</div>
			    <div class="clear">&nbsp;</div>
			    
	    		<div class="board-reply">
                    <% foreach (var item in myIssue.IssueReplys) { %>
                        <% = item.Reply %><br />
                    <% } %>
					<div class="push-2 col-19">
						<div class="p-a5">
							<div class="col-2">
								<img src="<%= Model.NavigationModel.ProfilePictureUrl %>" alt="<%= Model.NavigationModel.User.Username %>" class="profile" />
							</div>
							<div class="m-lft col-14 m-rgt">
							    <% using (Html.BeginForm("Create", "IssueReply", new { issueId = myIssue.Id, disposition = 1, anonymous = false })) { %>
						            <%= Html.ValidationMessage("Reply", "*")%>
						            <%= Html.TextArea("Reply")%>
						            <div class="clear">&nbsp;</div>
						            <div class="right m-top10">
						            	<input type="submit" value="Post" />
						            </div>
							    <% } %>
							</div>
							<!--
							<div class="alpha col-3 omega">
								<div class="p-v5">
									<div class="date-tile">
										<span>8:23</span> PM
									</div>
								</div>
							</div>
							//-->
							<div class="clear">&nbsp;</div>
						</div>
						<div class="clear">&nbsp;</div>
					</div>
					<div class="clear">&nbsp;</div>
				</div>
			    <div class="spacer-10">&nbsp;</div>
            <% } else if (myNextFeedItem == FeedItem.IssueReply) {%>
                <% IssueReplyFeedModel myIssueReply = Model.Model.GetNextIssueReply(); %>
			    <div class="<% if(cnt % 2 == 0) { %>row<% } else { %>alt<% } %>">
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
											    <a href="#" class="comment">
													Comment
											    </a>
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
									    <div class="col-3 center">
										    <% if (myIssueReply.HasDisposition) { %>
											    <span class="dislike">
												    <%= myIssueReply.TotalDislikes%>
												    <% if (myIssueReply.TotalDislikes == 1) { %>
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
							    <span><%= myIssueReply.DateTimeStamp.ToString("MMM").ToUpper()%></span> <%= myIssueReply.DateTimeStamp.ToString("dd") %>
						    </div>
					    </div>
				    </div>
				    <div class="clear">&nbsp;</div>
			    </div>
			    <div class="clear">&nbsp;</div>
			    <div class="spacer-10">&nbsp;</div>
			    <div class="clear">&nbsp;</div>
			    
	    		<div class="board-reply">
                    <% foreach (var item in myIssueReply.IssueReplyComments) { %>
                        <% = item.Comment %><br />
                    <% } %>
					<div class="push-2 col-19">
						<div class="p-a5">
							<div class="col-2">
								<img src="<%= Model.NavigationModel.ProfilePictureUrl %>" alt="<%= Model.NavigationModel.User.Username %>" class="profile" />
							</div>
							<div class="m-lft col-14 m-rgt">
							    <% using (Html.BeginForm("Create", "IssueReplyComment", new { issueReplyId = myIssueReply.Id })) { %>
						            <%= Html.ValidationMessage("Comment", "*")%>
						            <%= Html.TextArea("Comment")%>
						            <div class="clear">&nbsp;</div>
						            <div class="right m-top10">
						            	<input type="submit" value="Post" />
						            </div>
							    <% } %>
							</div>
							<!--
							<div class="alpha col-3 omega">
								<div class="p-v5">
									<div class="date-tile">
										<span>8:23</span> PM
									</div>
								</div>
							</div>
							//-->
							<div class="clear">&nbsp;</div>
						</div>
						<div class="clear">&nbsp;</div>
					</div>
					<div class="clear">&nbsp;</div>
				</div>
            <% } %>
            <% cnt++; %>
        <% } %>
    <% } else if (Model.NavigationModel.SiteSection == SiteSection.Profile) { %>
        <% FeedItem myNextFeedItem = Model.Model.GetNextItem(); %>
        <% int cnt = 0; %>
        <% while(myNextFeedItem != FeedItem.None) { %>
            <%  myNextFeedItem = Model.Model.GetNextItem(); %>
            <% if (myNextFeedItem == FeedItem.Issue) { %>
                <% IssueFeedModel myIssue = Model.Model.GetNextIssue(); %>
            <% } else if (myNextFeedItem == FeedItem.IssueReply) { %>
		        <% IssueReplyFeedModel myIssueReply = Model.Model.GetNextIssueReply(); %>
            <% }  else if (myNextFeedItem == FeedItem.Photo) { %>
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
            <% } else if (myNextFeedItem == FeedItem.Board) { %>
                <% BoardFeedModel myBoard = Model.Model.GetNextBoard(); %>
	            <!-- BOARD ACTIVITY [Message] //-->
	            <div class="row">
		            <div class="col-2 center">
			            <img src="<%= myBoard.ProfilePictureUrl %>" alt="<%= myBoard.Username %>" class="profile" />
		            </div>
		            <div class="col-16 m-btm10">
			            <div class="m-lft col-16 comment">
				            <span class="speak-lft">&nbsp;</span>
				            <div class="p-a10">
					            <a class="name" href="#"><%= myBoard.Username%></a>
                                    <%= myBoard.Message%>
					            <div class="clear">&nbsp;</div>

					            <div class="spacer-10">&nbsp;</div>
							
					            <div class="clear">&nbsp;</div>
					            <div class="options">
						            <div class="col-6">&nbsp;</div>
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

	            <!-- BOARD ACTIVITY [Reply] //-->
	            <% foreach (BoardReply myReply in myBoard.BoardReplys) { %>
                    <div class="board-reply">
		                <div class="push-2 col-19">
			                <div class="p-a5">
				                <div class="col-2">
					            <img src="<%= myBoard.ProfilePictureUrl %>" alt="<%= myBoard.Username %>" class="profile" />
                                <%= myReply.Message %>
				            </div>
				                <div class="m-lft col-14 m-rgt">
					            <% using (Html.BeginForm("Create", "BoardReply", new { boardId = myBoard.Id })) { %>
				                    <%= Html.ValidationMessage("Message", "*")%>
				                    <%= Html.TextArea("Message")%>
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
                <% } %>
            <% } %>
            <% cnt++; %>
        <% } %>
    <% } %>


</asp:Content>
