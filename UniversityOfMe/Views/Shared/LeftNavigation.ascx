<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<UniversityOfMe.Models.View.LeftNavigation>" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="UniversityOfMe.Helpers.Search" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>

<script>
    $(document).ready(function () {
        $("#datingtooltip a[title]").tooltip();
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

<div class="four"> 
	<div class="banner full mb72"> 
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

	<div class="banner title"> 
		NEW NOTIFICATIONS
	</div> 
	<div class="box"> 
        <% Html.RenderPartial("Notifications", Model.Notifications); %>
        <div>
            &nbsp;
            <%= Html.ActionLink("View all notifications", "List", "Notification", null, new { @class = "viewall" })%>
        </div>
	</div> 
				
    <% if(Model.HasDatingMatch()) { %>	
	    <div class="match"> 
            <%= Html.ActionLink("Close", "MarkAsSeen", "Dating", new { datingLogId = Model.DatingMatchMember.Id }, new { @class = "close" })%>
            <a href="/<%= Model.DatingMatchMember.AskedUser.ShortUrl %>"><img src="<%= PhotoHelper.ProfilePicture(Model.DatingMatchMember.AskedUser) %>" alt="<%= NameHelper.FullName(Model.DatingMatchMember.AskedUser) %>" title="<%= NameHelper.FullName(Model.DatingMatchMember.AskedUser) %>" class="profile med" /></a>
		    You got a dating match with<br /> 
		    <span class="normal mr14"><%= NameHelper.FullName(Model.DatingMatchMember.AskedUser) %></span> 
            
            <%= Html.ActionLink("Message", "Create", "Message", new { id = Model.DatingMatchMember.AskedUserId }, new { @class = "mail mr9" })%>
            <%= Html.ActionLink("Send beer", "SendItem", "SendItems", new { id = Model.DatingMatchMember.AskedUserId, sendItem = SendItemOptions.BEER }, new { @class = "beer" })%>
		    <span class="arrow"></span> 
	    </div> 
    <% } %>

    <% if(FeatureHelper.IsFeatureEnabled(Model.User, Features.DatingAsked)) { %> 
	    <div class="banner title"> 
		    UOFME RANDOM DATING
		    <div id="datingtooltip" class="buttons"> 
			        <a href="#" class="question" title="A dating match is made only if the person says yes too, if they say no they never know.">What is this?</a> 
			    <%= Html.ActionLink("Disable", "DisableFeature", "Profile", new { feature = Features.DatingAsked }, new { @class = "deny" })%>
		    </div> 
	    </div> 

	    <div class="box date"> 
            <% if (Model.HasDatingMember()) { %>	
                <a href="/<%= Model.DatingMember.ShortUrl %>"><img src="<%= PhotoHelper.ProfilePicture(Model.DatingMember) %>" alt="<%= NameHelper.FullName(Model.DatingMember) %>" title="<%= NameHelper.FullName(Model.DatingMember) %>" class="profile sm" /></a>
		        Would you date <%= NameHelper.FullName(Model.DatingMember)%> ?
		        <div class="mt6 center"> 
                    <% using (Html.BeginForm("Create", "Dating", FormMethod.Post)) {%>
                        <%= Html.Hidden("SourceUserId", Model.DatingMember.Id) %>
			            <span><button name="response" class="btn blue" value="true">Yes</button></span>
          
			            <span class="ml10"><button name="response" class="btn blue" value="false">No</button></span>
                    <% } %>
		        </div> 
            <% } else { %>
                There are currently no members to ask you about
            <% } %>
	    </div> 
    <% } %>
					
	<div class="banner title"> 
		NEWEST MEMBERS
	</div> 
	<div class="box member"> 
    <% foreach (User myNewestMember in Model.NewestUsersInUniversity) { %>
        <a href="<%= URLHelper.ProfileUrl(myNewestMember) %>"><img src="<%= PhotoHelper.ProfilePicture(myNewestMember) %>" title="<%= NameHelper.FullName(myNewestMember) %>" /></a>
    <% } %>
	</div> 
    <% if(Model.LatestUserBadge != null) { %>
        <div>
            <div class="banner title"> 
                LATEST EARNED BADGE
                <span class="frgt">
                    <a class="leftnavlink" href="<%= URLHelper.BadgeHideUrl(Model.LatestUserBadge.Id) %>">Hide</a> 
                    | 
                    <a class="leftnavlink" href="<%= URLHelper.BadgeListUrl() %>">View All</a>
                </span>
            </div> 
            <div class="box center"> 
            
                    <img src="<%= URLHelper.BadgeUrl(Model.Badge.Image) %>" /><br />
                    <%= Model.Badge.Description %>
            
            </div>
        </div>
    <% } %>
</div> 