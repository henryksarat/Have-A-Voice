<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<ProfileModel>" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>
<%@ Import Namespace="UniversityOfMe.UserInformation" %>
<%@ Import Namespace="Social.Generic.Models" %>
<%@ Import Namespace="UniversityOfMe.Helpers.Functionality" %>
<%@ Import Namespace="UniversityOfMe.Helpers.Search" %>
<%@ Import Namespace="Social.Generic.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=NameHelper.FullName(Model.User) %> | Profile
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MetaDescriptionHolder" runat="server">
	<%= UniversityOfMe.Helpers.MetaHelper.MetaDescription(NameHelper.FullName(Model.User) + "'s Profile") %>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MetaKeywordsHolder" runat="server">
	<%= UniversityOfMe.Helpers.MetaHelper.MetaKeywords(NameHelper.FullName(Model.User) + ", " + UniversityHelper.GetMainUniversity(Model.User).UniversityName) %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<script>
    $(document).ready(function () {
        $("#search").bind("keydown", function (event) {
            // track enter key
            var keycode = (event.keyCode ? event.keyCode : (event.which ? event.which : event.charCode));
            if (keycode == 13) { // keycode for enter key
                var myType = $('.active-filter').attr('id');
                var mySearch = $('#search').val();
                $.post("/Search/DoSearch", { searchType: myType, searchString: mySearch, page: 1 });
                return false;
            } else {
                return true;
            }
        });
        $("#search").click(function () {
            $("#search").val("");
        });
    });

    var isShown = false;

    $(function () {
        $("a.search").click(function () {
            if ($("ul.options").is(":visible")) {
                $("div.select").css({ borderColor: "transparent", borderWidth: "0px", borderStyle: "none", top: "9px", right: "16px" });
                $("ul.options").slideUp("fast");
                isShown = false;
            } else {
                $("div.select").css({ borderColor: "#cdcdcd", borderWidth: "1px", borderStyle: "solid", top: "8px", right: "15px" });
                $("ul.options").slideDown("fast");
                isShown = true;
            }
            return false;
        });
        $("ul.options a").click(function () {
            $("ul.options li").removeClass("active-filter");
            $(this).parent("li").addClass("active-filter");

            return false;
        });
        $("body").click(function () {
            if (isShown) {
                $("div.select").css({ borderColor: "transparent", borderWidth: "0px", borderStyle: "none", top: "9px", right: "16px" });
                $("ul.options").slideUp("fast");
                isShown = false;
            }
        })
    });
</script> 


<% UserInformationModel<User> myLoggedInUser = UserInformationFactory.GetUserInformation(); %>
<% bool myViewingOwn = true; %>
<% if(myLoggedInUser != null) { %>
    <% myViewingOwn = Model.User.Id == myLoggedInUser.UserId; %>
<% } %>
<% bool myAllowedToView = PrivacyHelper.IsAllowed(Model.User, PrivacyAction.DisplayProfile, myLoggedInUser); %> 

<div class="row"> 
	<div class="four"> 
		<div class="banner full"> 
		<div class="search clearfix"> 
			<input id="search" type="text" class="inpt" value="type in your query and hit enter" /> 
			<div class="select"> 
				<a href="#" class="search">Search</a> 
				<ul class="options"> 
					<li id="<%= SearchFilter.All %>" class="active-filter"> 
						<a href="#" class="all">All</a> 
					</li> 
					<li id="<%= SearchFilter.User %>"> 
						<a href="#" class="people"><%= SearchFilter.User.ToString() %></a> 
					</li> 
					<li id="<%= SearchFilter.Professor %>"> 
						<a href="#" class="professor"><%= SearchFilter.Professor.ToString() %></a> 
					</li> 
					<li id="<%= SearchFilter.Class %>"> 
						<a href="#" class="case"><%= SearchFilter.Class.ToString() %></a> 
					</li> 
					<li  id="<%= SearchFilter.Event %>"> 
						<a href="#" class="cal"><%= SearchFilter.Event.ToString() %></a> 
					</li> 
					<li id="<%= SearchFilter.Textbook %>"> 
						<a href="#" class="text"><%= SearchFilter.Textbook.ToString() %></a> 
					</li> 
					<li id="<%= SearchFilter.GeneralPosting %>"> 
						<a href="#" class="paper"><%= SearchFilter.GeneralPosting.ToString() %></a> 
					</li> 
					<li id="<%= SearchFilter.Organization %>"> 
						<a href="#" class="org"><%= SearchFilter.Organization.ToString() %></a> 
					</li> 
				</ul> 
			</div>
		</div> 
		<span class="corner"></span> 
		</div> 
        <div class="imagedisplay">
    		<img src="<%= PhotoHelper.OriginalProfilePicture(Model.User) %>" class="mt34 mb15" alt="Anca Foster" /> 
        </div>  
		<div class="prof-info"> 
			<p><label>Name:</label><%=NameHelper.FullName(Model.User) %></p> 
			<p><label>University:</label><%= UniversityHelper.GetMainUniversity(Model.User).UniversityName %></p> 
            <% if (myAllowedToView) { %>    
                <p><label>Birthdate:</label><%= String.Format("{0:dd MMM yyyy}", Model.User.DateOfBirth) %></p> 
			    <p><label>Relationship:</label><%= Model.User.RelationshipStatu != null ? Model.User.RelationshipStatu.DisplayName : "NA" %></p> 
                
                <% if (!string.IsNullOrEmpty(Model.User.Job)) { %>
                    <p><label>Job:</label><%= Model.User.Job%></p> 
                <% } %>

                 <% if (!string.IsNullOrEmpty(Model.User.City) && !string.IsNullOrEmpty(Model.User.State)) { %>
                    <p><label>Location:</label><%= Model.User.City + ", " + Model.User.State %> </p> 
                <% } else if (!string.IsNullOrEmpty(Model.User.City)) { %>
                    <p><label>Location:</label><%= Model.User.City %> </p> 
                <% } else if (!string.IsNullOrEmpty(Model.User.State)) { %>
                    <p><label>Location:</label><%= Model.User.State %> </p> 
                <% } %>

                <% if (!string.IsNullOrEmpty(Model.User.Website)) { %>
                    <p><label>Website:</label><%= Model.User.Website%> </p> 
                <% } %>

                 <% if (!string.IsNullOrEmpty(Model.User.AboutMe)) { %>
                    <p><label>About Me:</label><%= Model.User.AboutMe %> </p> 
                <% } %>

                <div>
                    <a class="feedlink" href="<%= URLHelper.FriendListUrl(Model.User.Id) %>">Total Friends: <%= Model.FriendCount %></a>
                    <div class="wp100 center">
                        <% foreach(User myFriend in Model.FriendToShow) { %>
                            <div style="display:inline-block; margin-left: 15px; width: 100px; text-align: center">
                                <a class="feedlink" href="<%= URLHelper.ProfileUrl(myFriend) %>">
                                    <%= NameHelper.FullName(myFriend) %><br />
                                </a>
                                <a href="<%= URLHelper.ProfileUrl(myFriend) %>">
                                    <img src="<%= PhotoHelper.ProfilePicture(myFriend) %>" title="<%= NameHelper.FullName(myFriend) %>" class="profile med" />
                                </a>
                            </div>
                        <% } %>
                    </div>
                </div>

                <% if (Model.LatestBadge != null) { %>
                    <div style="width:100%;">
                        <span class="left">
                            <a class="feedlink" href="<%= URLHelper.BadgeListUrl(Model.User.Id) %>">Latest badge earned (View all):</a>
                        </span>
                        <div class="wp100 center">
                            <img src="<%= URLHelper.BadgeUrl(Model.LatestBadge.Image) %>" /><br />
                            <%= Model.LatestBadge.Description%>
                        </div>
                    </div>
                <% } %>
            <% } %>
			<div class="gray bold">Latest Activity:</div>
			<ul> 
                <% bool myHasActivity = false; %>
                <% foreach (UserFeedModel myFeedModel in UserFeed.GetUserFeed(Model.User, 10)) { %>
				    <li class="<%= myFeedModel.CssClass %>"> 
					    <%= myFeedModel.FeedString %>
                        <% myHasActivity = true; %>
				    </li> 
                <% } %>

                <% if (!myHasActivity) { %>
                    They have no activity.
                <% } %>
			</ul> 
		</div> 
	</div> 
				
	<div class="eight last"> 
        <% Html.RenderPartial("Message"); %>
		<div class="banner black full red-top small"> 
			<span class="mine"><%=NameHelper.FullName(Model.User) %></span> 
            <% if (!myViewingOwn) { %>
			    <div class="buttons"> 
                    <%= Html.ActionLink("Anonymously Flirt", "CreateWithTaggedUser", "Flirt", new { taggedUserId = Model.User.Id }, new { @class = "flirt mr10" })%>
                    <% if (FriendHelper.IsFriend(Model.User, myLoggedInUser.Details)) { %>
                        <%= Html.ActionLink("Remove from friends", "Delete", "Friend", new { id = Model.User.Id }, new { @class = "defriend mr10" })%>
                    <% } %>
                    <%= Html.ActionLink("Send beer", "SendItem", "SendItems", new { id = Model.User.Id, sendItem = SendItemOptions.BEER }, new { @class = "beer mr10" })%>
                    <%= Html.ActionLink("Send PM", "Create", "Message", new { id = Model.User.Id }, new { @class = "pm mr10" })%>				
                    <% if (!FriendHelper.IsFriend(Model.User, myLoggedInUser.Details)) { %>
                        <%= Html.ActionLink("Add as friend", "Add", "Friend", new { id = Model.User.Id }, new { @class = "addfriend" })%>
                    <% } %>
			    </div> 
            <% } %>
		</div> 

        <% Html.RenderPartial("Validation"); %>
 
        <% if (myAllowedToView) { %>
            <% if (Model.HasUserStatuses) { %>
                <div class="profile-form">
			        <div class="banner full"> 
				        WHAT'S <%= Model.User.Gender.Equals(Social.Generic.Constants.Gender.MALE) ? "HE" : "SHE"%> DOING
			        </div>            
                    <div class="padding-col">
                        <% foreach (UserStatus myUserStatus in Model.UserStatuses) { %>
                            <div class="small">
                                <%= NameHelper.FullName(myUserStatus.User) + " is " + myUserStatus.Status + " at " + LocalDateHelper.ToLocalTime(myUserStatus.DateTimeStamp)%>
                                <% if (UserStatusHelper.IsAllowedToDelete(myLoggedInUser, myUserStatus)) {  %>
                                    <%= Html.ActionLink("Delete", "Delete", "UserStatus", new { id = myUserStatus.Id, sourceController = "Profile", sourceAction = "Show", sourceId = Model.User.Id }, new { @class = "profiledelete" })%>
                                <% } %>
                            </div>
                        <% } %>
                    </div>
                </div>
            <% } %>

            <div class="profile-form">
		        <div class="create clearfix"> 
			        <div class="banner full mt50"> 
				        BOARD
			        </div> 
                <div class="padding-col">
                    <% if (!myViewingOwn) { %>
                        <% using (Html.BeginForm("Create", "Board", FormMethod.Post)) { %>
                            <%= Html.Hidden("SourceUserId", Model.User.Id)%>

                            <%: Html.TextArea("BoardMessage", null, 3, 0, new { @class = "full" })%>
                            <%: Html.ValidationMessage("BoardMessage", "*", new { @class = "req" })%>
								
			            <div class="frgt mt13"> 
				            <input type="submit" class="frgt btn site" name="post" value="Post" /> 
			            </div> 
	                    <% } %>
                    <% } %>
                    <% bool myHasBoardActivity = false; %>                
                    <% IEnumerable<Board> myAllBoards = Model.Boards.Where(b => !b.Deleted).OrderByDescending(b => b.DateTimeStamp);  %>
                    <% foreach (Board myBoard in myAllBoards.Take<Board>(5)) { %>
                        <% myHasBoardActivity = true; %>
		                <div class="board"> 
			            <div class="prfl clearfix"> 
				            <div class="pCol"> 
                                <a href="<%= URLHelper.ProfileUrl(myBoard.PostedByUser) %>">
					                <img src="<%= PhotoHelper.ProfilePicture(myBoard.PostedByUser) %>" class="profile big" /> 
                                </a>
				            </div> 
				            <div class="cCol"> 
					            <div class="red bld"> 
						            <div class="frgt"> 
							            <span class="gray delete nrm"> 
								            <%= LocalDateHelper.ToLocalTime(myBoard.DateTimeStamp, "{0:MMMM dd, yyyy h:mm tt}")%>
							            </span> 
						            </div> 
						            <%= NameHelper.FullName(myBoard.PostedByUser)%> 
                                    <% if (BoardHelper.IsAllowedToDelete(myLoggedInUser, myBoard)) {  %>
                                        <%= Html.ActionLink("Delete", "Delete", "Board", new { sourceId = Model.User.Id, boardId = myBoard.Id, sourceController = "Profile", sourceAction = "Show" }, new { @class = "small nrm" })%>
                                    <% } %>
					            </div> 
					            <%= myBoard.Message%>
					            <div class="create clearfix"> 
                                    <% using (Html.BeginForm("Create", "BoardReply", FormMethod.Post)) { %>
                                            <%= Html.Hidden("BoardId", myBoard.Id)%>
                                            <%= Html.Hidden("SourceId", Model.User.Id)%>
                                            <%= Html.Hidden("SiteSection", SiteSection.Profile)%>

                                            <%= Html.TextArea("BoardReply", null, 2, 0, new { @class = "full" })%>
                                            <%= Html.ValidationMessage("BoardReply", "*", new { @class = "req" })%>

						            <div class="frgt mt13"> 
							            <input type="submit" class="frgt btn site" name="post" value="Reply" /> 
						            </div> 
	                                <% } %>
					            </div> 
				            </div>							
			            </div> 
                
                        <% int myCurrentBoardReply = 0; %>
                        <% bool myPostedLink = false; %>
                        <% foreach (BoardReply myReply in myBoard.BoardReplies.Where(br => !br.Deleted).OrderByDescending(br => br.DateTimeStamp)) { %>						
                            <% if(myBoard.BoardReplies.Count > 2) { %>
                                <% if(!myPostedLink) { %>
            		                <div class="viewall left"> 
                                        <a href="<%= URLHelper.BoardDetailsUrl(myBoard) %>">View all <%= myBoard.BoardReplies.Count %> replies</a> 
                                        <% myPostedLink = true; %>
		                            </div> 
                                <% } %>
                                <% if (myCurrentBoardReply <= 1) { %>
                                    <% myCurrentBoardReply++; %>
                                <% } else { %>
                                    <% myPostedLink = false; %>
                                    <% break; %>
                                <% } %>
                            <% } %>

			            <div class="prfl reply clearfix"> 
				            <div class="pCol"> 
                                <a href="<%= URLHelper.ProfileUrl(myReply.User) %>">
					                <img src="<%= PhotoHelper.ProfilePicture(myReply.User) %>" class="profile med" /> 
                                </a>
				            </div> 
				            <div class="cCol"> 
					            <div class="red bld"> 
						            <div class="frgt"> 
							            <span class="gray small nrm"> 
								            <!-- <a href="#" class="">Edit</a> 
								            |
								            <a href="#" class="mr20">Remove</a> -->
								            <%= LocalDateHelper.ToLocalTime(myReply.DateTimeStamp, "{0:MMMM dd, yyyy h:mm tt}")%>
							            </span> 
						            </div> 
						            <%= NameHelper.FullName(myReply.User)%>
                                    <% if (BoardReplyHelper.IsAllowedToDelete(myLoggedInUser, myReply)) {  %>
                                        <%= Html.ActionLink("Delete", "Delete", "BoardReply", new { sourceId = Model.User.Id, boardReplyId = myReply.Id, sourceController = "Profile", sourceAction = "Show" }, new { @class = "small nrm" })%>
                                    <% } %>
					            </div> 
					            <%= myReply.Message%>
				            </div> 
			            </div> 
                        <% } %>
		            </div> 
                    <% } %>
                    <% if(!myHasBoardActivity) { %>
                        <div class="center small bold">
                            The user has no postings to their board.
                        </div>
                    <% } %>

                    <div class="viewall"> 
		                <% if(!Model.ShowAllBoards) { %>
                            <a href="<%= URLHelper.ProfileUrlForAllBoards(Model.User, Model.ShowAllPhotoAlbums) %>">View entire board</a> 
                        <% } %>
		            </div> 
                </div>
            </div>
        </div>
                
			
            <div class="profile-form">
		        <div class="banner full mt50"> 
			        PHOTOS
		        </div> 
                <div class="padding-col">
                    <% bool myHasPhotos = false; %>
                    <% foreach (PhotoAlbum myAlbum in Model.PhotoAlbums) { %>
		                <div class="album"> 
			            <a href="<%= URLHelper.PhotoAlbumDetailsUrl(myAlbum) %>"> 
				            <img src="<%= PhotoHelper.PhotoAlbumCover(myAlbum) %>" alt="photo" /> 
				            <br /> 
				            <%= myAlbum.Name%>
                            <% myHasPhotos = true;%>
			            </a> 
		            </div> 
                    <% } %>
                    <% if (!myHasPhotos) { %>
                        <div class="center small bold">
                            The user has no photos.
                        </div>
                    <% } %>
                </div>
            </div>
        <% } else { %>
            <div class="center small bold">
                The user's privacy settings disallow you from viewing their profile.
            </div>
        <% } %>
	</div> 
</asp:Content>
