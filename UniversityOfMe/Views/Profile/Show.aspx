<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<User>" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>
<%@ Import Namespace="UniversityOfMe.UserInformation" %>
<%@ Import Namespace="Social.Generic.Models" %>
<%@ Import Namespace="UniversityOfMe.Helpers.Functionality" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	UniversityOf.Me - <%=NameHelper.FullName(Model) %>'s Profile
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<% UserInformationModel<User> myLoggedInUser = UserInformationFactory.GetUserInformation(); %>
<% bool myViewingOwn = Model.Id == myLoggedInUser.UserId; %>

<div class="row"> 
	<div class="four"> 
		<div class="banner full"> 
			<input type="search" name="search" id="search" /> 
			<span class="corner"></span> 
		</div> 
 
		<img src="<%= PhotoHelper.OriginalProfilePicture(Model) %>" class="mt34 mb15" alt="Anca Foster" /> 
		<div class="prof-info"> 
			<p><label>Name:</label><%=NameHelper.FullName(Model) %></p> 
			<p><label>Birthdate:</label><%= String.Format("{0:dd MMM yyyy}", Model.DateOfBirth) %></p> 
			<p><label>Relationship:</label><%= Model.RelationshipStatu != null ? Model.RelationshipStatu.DisplayName : "NA" %></p> 
			<p><label>University:</label><%= UniversityHelper.GetMainUniversity(Model).UniversityName %></p> 
			<ul> 
                <% foreach (UserFeedModel myFeedModel in UserFeed.GetUserFeed(Model, 10)) { %>
                    <% if(myFeedModel.FeedType == FeedType.Textbook) { %>
				        <li class="<%= myFeedModel.CssClass %>"> 
					        <%= myFeedModel.FeedString %>
				        </li> 
                    <% } %>
                <% } %>
			</ul> 
		</div> 
	</div> 
				
	<div class="eight last"> 
        <% Html.RenderPartial("Message"); %>
        <% Html.RenderPartial("Validation"); %>
		<div class="banner black full red-top small"> 
			<span class="mine"><%=NameHelper.FullName(Model) %></span> 
            <% if (!myViewingOwn) { %>
			    <div class="buttons"> 
                    <% if (FriendHelper.IsFriend(Model, myLoggedInUser.Details)) { %>
                        <%= Html.ActionLink("Remove from friends", "Delete", "Friend", new { id = Model.Id }, new { @class = "defriend mr10" })%>
                    <% } %>
                    <%= Html.ActionLink("Send beer", "SendItem", "SendItems", new { id = Model.Id, sendItem = SendItemOptions.BEER }, new { @class = "beer mr10" })%>
                    <%= Html.ActionLink("Send PM", "Create", "Message", new { id = Model.Id }, new { @class = "pm mr10" })%>				
                    <% if (!FriendHelper.IsFriend(Model, myLoggedInUser.Details)) { %>
                        <%= Html.ActionLink("Add as friend", "Add", "Friend", new { id = Model.Id }, new { @class = "addfriend" })%>
                    <% } %>
			    </div> 
            <% } %>
		</div> 
 
		<div class="create clearfix"> 
			<div class="banner full mt50"> 
				BOARD
			</div> 
            <% if (!myViewingOwn) { %>
                <% using (Html.BeginForm("Create", "Board", FormMethod.Post)) { %>
                    <%= Html.Hidden("SourceUserId", Model.Id)%>

                    <%: Html.TextArea("BoardMessage", null, 3, 0, new { @class = "full" })%>
                    <%: Html.ValidationMessage("BoardMessage", "*")%>
								
			    <div class="frgt mt13"> 
				    <input type="submit" class="frgt btn site" name="post" value="Post" /> 
			    </div> 
	            <% } %>
            <% } %>
		</div> 
 
        <% foreach (Board myBoard in Model.Boards.OrderByDescending(b => b.DateTimeStamp)) { %>
		    <div class="board"> 
			    <div class="prfl clearfix"> 
				    <div class="pCol"> 
					    <img src="<%= PhotoHelper.ProfilePicture(Model) %>" class="profile big" /> 
				    </div> 
				    <div class="cCol"> 
					    <div class="red bld"> 
						    <div class="frgt"> 
							    <span class="gray small nrm"> 
								    <%= DateHelper.ToLocalTime(myBoard.DateTimeStamp, "{0:MMMM dd, yyyy h:mm tt}")%>
							    </span> 
						    </div> 
						    <%= NameHelper.FullName(Model) %>
					    </div> 
					    <%= myBoard.Message %>
					    <div class="create clearfix"> 
                            <% using (Html.BeginForm("Create", "BoardReply", FormMethod.Post)) { %>
                                    <%= Html.Hidden("BoardId", myBoard.Id)%>
                                    <%= Html.Hidden("SourceId", Model.Id) %>
                                    <%= Html.Hidden("SiteSection", SiteSection.Profile) %>

                                    <%= Html.TextArea("BoardReply", null, 2, 0, new { @class="full" })%>
                                    <%= Html.ValidationMessage("BoardReply", "*")%>

						    <div class="frgt mt13"> 
							    <input type="submit" class="frgt btn site" name="post" value="Reply" /> 
						    </div> 
	                        <% } %>
					    </div> 
				    </div>							
			    </div> 
                
                <% foreach (BoardReply myReply in myBoard.BoardReplies.OrderByDescending(br => br.DateTimeStamp)) { %>						
			    <div class="prfl reply clearfix"> 
				    <div class="pCol"> 
					    <img src="<%= PhotoHelper.ProfilePicture(myReply.User) %>" class="profile med" /> 
				    </div> 
				    <div class="cCol"> 
					    <div class="red bld"> 
						    <div class="frgt"> 
							    <span class="gray small nrm"> 
								    <!-- <a href="#" class="">Edit</a> 
								    |
								    <a href="#" class="mr20">Remove</a> -->
								    <%= DateHelper.ToLocalTime(myReply.DateTimeStamp, "{0:MMMM dd, yyyy h:mm tt}")%>
							    </span> 
						    </div> 
						    <%= NameHelper.FullName(myReply.User) %>
					    </div> 
					    <%= myReply.Message %>
				    </div> 
			    </div> 
                <% } %>
		    </div> 
        <% } %>

		<div class="viewall mb50"> 
			<a href="#">View All 23 Results</a> 
		</div> 
					
		<div class="banner full"> 
			PHOTOS
		</div> 
        <% foreach (PhotoAlbum myAlbum in Model.PhotoAlbums) { %>
		    <div class="album"> 
			    <a href="<%= URLHelper.PhotoAlbumDetailsUrl(myAlbum) %>"> 
				    <img src="<%= PhotoHelper.PhotoAlbumCover(myAlbum) %>" alt="photo" /> 
				    <br /> 
				    <%= myAlbum.Name%>
			    </a> 
		    </div> 
        <% } %>
	</div> 
</div> 
</asp:Content>
