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
								    <h1><a href="#"><%= myIssue.Title %></a></h1>
							    <br />
							    <%= myIssue.Description %>

							    <div class="clear">&nbsp;</div>							
							    <div class="spacer-10">&nbsp;</div>
							    <div class="clear">&nbsp;</div>

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
			    <div class="spacer-10">&nbsp;</div>
			    <% break; cnt++; %>
            <% } %>
        <% } %>
    <% } else if (Model.NavigationModel.SiteSection == SiteSection.Profile) { %>
        It's <%= Model.NavigationModel.User.Username %> profil page!!
    <% } %>


</asp:Content>
