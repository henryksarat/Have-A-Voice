<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInWrapperModel<Club>>" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="UniversityOfMe.Helpers.Functionality" %>
<%@ Import Namespace="UniversityOfMe.UserInformation" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%= Model.Get().Name %> at <%= Model.Get().University.UniversityName %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MetaDescriptionHolder" runat="server">
	<%= UniversityOfMe.Helpers.MetaHelper.MetaDescription(Model.Get().Name + " at " + Model.Get().University.UniversityName)%>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MetaKeywordsHolder" runat="server">
	<%= UniversityOfMe.Helpers.MetaHelper.MetaKeywords(Model.Get().Name + ", " + Model.Get().University.UniversityName)%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $("#close-admin").click(function () {
                $("#admin").hide();
            });
        });
    </script> 

    <% Html.RenderPartial("LeftNavigation", Model.LeftNavigation); %>

	<div class="eight last"> 
        <% Html.RenderPartial("Message"); %>
        <% Html.RenderPartial("Validation"); %>

		<div class="banner black full red-top small"> 
			<span class="club"><%= Model.Get().Name %></span> 
		    <div class="buttons"> 
			    <div class="flft mr26"> 
                    <% User myUser = UserInformationFactory.GetUserInformation() != null ? UserInformationFactory.GetUserInformation().Details : null; %>
                    <% if (ClubHelper.IsAdmin(myUser, Model.Get().Id) && Model.Get().Active) { %>
                        <%= Html.ActionLink("Edit", "Edit", "Club", new { id = Model.Get().Id }, new { @class = "edit mr22" })%>                
                        <%= Html.ActionLink("Set club as inactive", "Deactivate", "Club", new { clubId = Model.Get().Id }, new { @class = "remove" })%>
                    <% } else if (ClubHelper.IsAdmin(myUser, Model.Get().Id) && !Model.Get().Active) { %>
                        <%= Html.ActionLink("Set club as active", "Activate", "Club", new { clubId = Model.Get().Id }, new { @class = "add" })%>
                    <% } else if (ClubHelper.IsPending(myUser, Model.Get().Id)) { %>
                        <%= Html.ActionLink("Cancel my request to join", "Cancel", "ClubMember", new { clubId = Model.Get().Id }, new { @class = "remove" })%>
                    <% } else if (ClubHelper.IsMember(myUser, Model.Get().Id)) { %>
                        <%= Html.ActionLink("Quit organization", "Remove", "ClubMember", new { clubId = Model.Get().Id }, new { @class = "remove" })%>
                    <% } else { %>
                        <%= Html.ActionLink("Request to join organization", "RequestToJoin", "ClubMember", new { clubId = Model.Get().Id }, new { @class = "add" })%>
                    <% } %>
			    </div> 
            </div>
		</div> 
					
		<table border="0" cellpadding="0" cellspacing="0" class="listing mb32"> 
			<tr> 
				<td rowspan="5" class="valign-top"> 
					<img src="<%= PhotoHelper.ClubPhoto(Model.Get()) %>" class="club" /> 
				</td> 
				<td> 
					<label for="memcount">Num. of Members</label> 
				</td> 
				<td> 
					<%= Model.Get().ClubMembers.Where(cm => cm.Approved == UOMConstants.APPROVED).Count<ClubMember>()%>
				</td> 
			</tr> 
			<tr> 
				<td style="vertical-align:top;"> 
					<label for="desc">UNIVERSITY</label> 
				</td> 
				<td style="vertical-align:top;"> 
					<%= Model.Get().University.UniversityName %>
				</td> 
			</tr> 
			<tr> 
				<td style="vertical-align:top;"> 
					<label for="desc">CLUB TYPE</label> 
				</td> 
				<td style="vertical-align:top;"> 
					<%= Model.Get().ClubTypeDetails.DisplayName %>
				</td> 
			</tr> 
			<tr> 
				<td style="vertical-align:top;"> 
					<label for="desc">DESCRIPTION</label> 
				</td> 
				<td style="vertical-align:top;"> 
					<%= Model.Get().Description %>
				</td> 
			</tr> 
		</table> 
        <% if(ClubHelper.IsAdmin(myUser, Model.Get().Id)) { %>				
            <div id="admin">
                <div class="banner title">
                    Club Admin Feed
                    <div style="float:right"><a id="close-admin" class="hide-this" href="#">Hide this</a></div>
                </div>
                <div>
                    <ul class="notification">

                        <% foreach (ClubAdminFeed myFeed in ClubHelper.GetAdminFeed(Model.Get(), 5)) { %>
                            <% if (myFeed.FeedType == FeedType.Member) { %>
                                <% if (myFeed.Status == Status.Pending) { %>
                                    <li>
                                        <a href="<%= URLHelper.ProfileUrl(myFeed.MemberUser) %>"><%= NameHelper.FullName(myFeed.MemberUser)%></a> 
                                        wants to join this organization. 
                                        <%= Html.ActionLink("Approve/Deny", "Details", "ClubMember", new { clubId = myFeed.ClubId, clubMemberId = myFeed.ClubMemberId }, null)%>
                                        <span class="feed-time"><%= LocalDateHelper.ToLocalTime((DateTime)myFeed.DateTimeStamp)%></span> 
                                    </li>
                                <% } else if(myFeed.Status == Status.Denied) { %>
                                    <li>
                                        <a href="<%= URLHelper.ProfileUrl(myFeed.AdminUser) %>"><%= NameHelper.FullName(myFeed.AdminUser) %></a> 
                                        has denied 
                                        <a href="<%= URLHelper.ProfileUrl(myFeed.MemberUser) %>"><%= NameHelper.FullName(myFeed.MemberUser) %></a> 
                                        to join the organization.
                                        <span class="feed-time"><%= LocalDateHelper.ToLocalTime((DateTime)myFeed.DateTimeStamp)%></span> 
                                    </li>
                                <% } else if (myFeed.Status == Status.Approved) { %>
                                    <li>
                                        <a href="<%= URLHelper.ProfileUrl(myFeed.AdminUser) %>"><%= NameHelper.FullName(myFeed.AdminUser)%></a> 
                                        has approved 
                                        <a href="<%= URLHelper.ProfileUrl(myFeed.MemberUser) %>"><%= NameHelper.FullName(myFeed.MemberUser)%></a> 
                                        to join the organization.
                                        <span class="feed-time"><%= LocalDateHelper.ToLocalTime((DateTime)myFeed.DateTimeStamp)%></span> 
                                    </li>
                                <% } %>
                            <% } else if(myFeed.FeedType == FeedType.Edited) { %>
                                    <li>
                                        <a href="<%= URLHelper.ProfileUrl(myFeed.AdminUser) %>"><%= NameHelper.FullName(myFeed.AdminUser) %></a> 
                                        has updated the club details. 
                                        <span class="feed-time"><%= LocalDateHelper.ToLocalTime((DateTime)myFeed.DateTimeStamp)%></span> 
                                    </li>
                            <% } else if(myFeed.FeedType == FeedType.Deactivated) { %>
                                    <li>
                                        <a href="<%= URLHelper.ProfileUrl(myFeed.AdminUser) %>"><%= NameHelper.FullName(myFeed.AdminUser) %></a> 
                                        has deactivated the club.
                                        <span class="feed-time"><%= LocalDateHelper.ToLocalTime((DateTime)myFeed.DateTimeStamp)%></span> 
                                    </li>
                            <% } %>
                            <br />
                        <% } %>
                    </ul>
                </div>
            </div>	
        <% } %>
		<div class="banner title"> 
			CURRENT MEMBERS JOINED <%= Model.Get().Name %>
		</div> 
		<div class="box sm group"> 
			<ul class="members"> 
                <% if (Model.Get().ClubMembers.Where(cm => cm.Approved == UOMConstants.APPROVED).Count<ClubMember>() == 0) { %>
                    There are no members apart of this club
                <% } %>
                <% foreach (ClubMember myMember in Model.Get().ClubMembers.Where(cm => cm.Approved == UOMConstants.APPROVED)) { %>
			        <li> 
				        <a href="/<%= myMember.ClubMemberUser.ShortUrl %>"><img src="<%= PhotoHelper.ProfilePicture(myMember.ClubMemberUser) %>" class="profile med flft mr9" /></a>
				        <span class="name"><a class="itemlinked" href="<%= URLHelper.ProfileUrl(myMember.ClubMemberUser) %>"><%= NameHelper.FullName(myMember.ClubMemberUser)%></a></span> 
				        <%= myMember.Title %>
			        </li>
                <% } %> 
			</ul> 
            <%= Html.ActionLink("View all members", "List", "ClubMember", new { id = Model.Get().Id }, new { @class = "viewall" })%>
			<div class="clearfix"></div> 
		</div> 
					
		<div class="banner full"> 
			CLUB BOARD
		</div> 
					
		<div id="review"> 
            <% if (ClubHelper.IsMember(myUser, Model.Get().Id)) { %>
			    <div class="create"> 
                    <% using (Html.BeginForm("Create", "ClubBoard")) {%>
                        <%= Html.Hidden("ClubId", Model.Get().Id)%>
                        <%= Html.TextArea("BoardMessage", string.Empty, 6, 0, new { @class = "full" })%>
                        <%= Html.ValidationMessage("BoardMessage", "*")%>
						
					    <div class="frgt mt13"> 
						    <input type="submit" class="frgt btn site" name="post" value="Post to Club Board" /> 
					    </div> 
					    <div class="clearfix"></div> 
                    <% } %>
			    </div> 
            <% } %>
						
			<div class="clearfix"></div> 
			<% if(Model.Get().ClubBoards.Count == 0) { %>
		        <div class="review"> 
			        <table border="0" cellpadding="0" cellspacing="0"> 
                            <tr> 
						        <td class="avatar">
							      There are no club board posts
						        </td> 
					        </tr> 
			        </table>  
			        <div class="clearfix"></div> 
		        </div> 
            <% } %>
						
			<% foreach (ClubBoard myClubBoard in Model.Get().ClubBoards.OrderByDescending(b => b.DateTimeStamp)) { %>
		        <div class="review"> 
			        <table border="0" cellpadding="0" cellspacing="0"> 
                            <tr> 
						        <td class="avatar">
							        <a href="/<%= myClubBoard.User.ShortUrl %>"><img src="<%= PhotoHelper.ProfilePicture(myClubBoard.User) %>" class="profile big mr22" /></a>
						        </td> 
						        <td> 
							        <div class="red bld"><%= NameHelper.FullName(myClubBoard.User) %>
								        <span class="gray small nrm"><%= LocalDateHelper.ToLocalTime(myClubBoard.DateTimeStamp)%></span> 
							        </div> 
							        <%= myClubBoard.Message %>
						        </td> 
					        </tr> 
			        </table> 
			        <div class="flft mr22"> 
								
			        </div> 
 
			        <div class="clearfix"></div> 
		        </div> 
            <% } %>
		</div> 
	</div> 
</asp:Content>
