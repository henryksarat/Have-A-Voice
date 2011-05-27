<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInWrapperModel<Club>>" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="UniversityOfMe.Helpers.Functionality" %>
<%@ Import Namespace="UniversityOfMe.UserInformation" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	University Of Me - <%= Model.Get().Name %> at <%= Model.University.UniversityName %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% Html.RenderPartial("LeftNavigation", Model.LeftNavigation); %>


    <% Html.RenderPartial("Message"); %>
    <% Html.RenderPartial("Validation"); %>

	<div class="eight last"> 
		<div class="banner black full red-top small"> 
			<span class="club"><%= Model.Get().Name %></span> 
		</div> 
					
		<table border="0" cellpadding="0" cellspacing="0" class="listing mb32"> 
			<tr> 
				<td rowspan="4"> 
					<img src="<%= PhotoHelper.ClubPhoto(Model.Get()) %>" class="club" /> 
				</td> 
				<td> 
					<label for="memcount">Num. of Members:</label> 
				</td> 
				<td> 
					<%= Model.Get().ClubMembers.Count %>
				</td> 
			</tr> 
			<tr> 
				<td> 
					<label for="desc">CLUB TYPE:</label> 
				</td> 
				<td> 
					<%= Model.Get().ClubTypeDetails.DisplayName %>
				</td> 
			</tr> 
			<tr> 
				<td> 
					<label for="desc">DESCRIPTION:</label> 
				</td> 
				<td> 
					<%= Model.Get().Description %>
				</td> 
			</tr> 
		</table> 
					
		<div class="banner title"> 
			CURRENT MEMBERS JOINED <%= Model.Get().Name %>
		</div> 
		<div class="box sm group"> 
			<ul class="members"> 
                <% if (Model.Get().ClubMembers.Count == 0) { %>
                    There are no members apart of this club
                <% } %>
                <% foreach (ClubMember myMember in Model.Get().ClubMembers) { %>
			        <li> 
				        <a href="/<%= myMember.ClubMemberUser.ShortUrl %>"><img src="<%= PhotoHelper.ProfilePicture(myMember.ClubMemberUser) %>" class="profile med flft mr9" /></a>
				        <span class="name"><%= NameHelper.FullName(myMember.ClubMemberUser)%></span> 
				        Student
			        </li>
                <% } %> 
			</ul> 
			<a href="#" class="viewall">View all members</a> 
			<div class="clearfix"></div> 
		</div> 
					
		<div class="banner full"> 
			CLUB BOARD
		</div> 
					
		<div id="review"> 
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
						
			<div class="clearfix"></div> 
						
			<div class="review"> 
				<table border="0" cellpadding="0" cellspacing="0"> 
					<% foreach (ClubBoard myClubBoard in Model.Get().ClubBoards.OrderByDescending(b => b.DateTimeStamp)) { %>
                        <tr> 
							<td class="avatar">
								<a href="/<%= myClubBoard.User.ShortUrl %>"><img src="<%= PhotoHelper.ProfilePicture(myClubBoard.User) %>" class="profile big mr22" /></a>
							</td> 
							<td> 
								<div class="red bld"><%= NameHelper.FullName(myClubBoard.User) %>
									<span class="gray small nrm"><%= DateHelper.ToLocalTime(myClubBoard.DateTimeStamp) %></span> 
								</div> 
								<%= myClubBoard.Message %>
							</td> 
						</tr> 
                    <% } %>
				</table> 
				<div class="flft mr22"> 
								
				</div> 
 
				<div class="clearfix"></div> 
			</div> 
		</div> 
	</div> 
</asp:Content>
